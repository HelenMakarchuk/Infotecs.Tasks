import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Article } from '../../models/article';
import { ArticleService } from '../../services/article.service';
import { ApplicationNotificationService } from 'src/app/notification/services/application.notification.service';
import { ArticleState } from './states/article.state';
import { ArticleCreatedState } from './states/article.created.state';
import { ArticleNewState } from './states/article.new.state';
import { ArticleEditableState } from './states/article.editable.state';
import { filter } from 'rxjs/operators';
import { ServerCommunicationService } from 'src/app/server-communication/contracts/server-communication.service';
import { AuthorizationService } from 'src/app/authorization/authorization.service';
import { ArticleDeletedState } from './states/article.deleted.state';

@Component({
    selector: 'app-article-detail',
    templateUrl: './article-detail.component.html',
    styleUrls: ['./article-detail.component.less']
})
export class ArticleDetailComponent implements OnInit
{
    state: ArticleState;
    article: Article;

    constructor(private route: ActivatedRoute,
                private router: Router,
                private articleService: ArticleService,
                public authorizeService: AuthorizationService,
                private applicationNotificationService: ApplicationNotificationService,
                private serverCommunicationService : ServerCommunicationService) {

        this.article = { id: 0, title: '', body: '', teaser: null, account: null, accountId: 0 };
        this.transitionTo(new ArticleCreatedState(this.articleService, this.serverCommunicationService));

        this.articleService.onUpdate
            .pipe(filter(id => id === this.article.id))
            .subscribe(() => {
                this.applicationNotificationService.notify("This article was changed by another user. Refresh this article to get last changes.");
            });

        this.articleService.onDelete
            .pipe(filter(id => id === this.article.id))
            .subscribe(() => {
                this.applicationNotificationService.notify("This article was deleted by another user.");
                this.transitionTo(new ArticleDeletedState(this.articleService, this.serverCommunicationService));
            });
    }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                if (params.get('id') !== null) {
                    this.articleService.getArticle(+params.get('id'))
                        .subscribe(
                            article => {
                                if (article === null) {
                                    alert('Article was deleted.');
                                    this.navigateToArticles();
                                    return;
                                }

                                this.article = article;
                                this.transitionTo(new ArticleCreatedState(this.articleService, this.serverCommunicationService));
                            },
                            error => {
                                console.log(error);
                                alert('Error while opening article');
                                this.navigateToArticles();
                            }
                        );
                }
                else {
                    this.transitionTo(new ArticleNewState(this.articleService, this.serverCommunicationService, this.router));
                }
            }
        );
    }

    public transitionTo(state: ArticleState): void {
        this.state = state;
        this.state.setContext(this);
    }

    navigateToArticles() {
        this.router.navigate(['/articles']);
    }

    createArticle() {
        this.state.create();
    }

    editArticle() {
        this.transitionTo(new ArticleEditableState(this.articleService, this.serverCommunicationService));
    }

    updateArticle() {
        this.state.update();
    }

    deleteArticle() {
        this.state.delete();
    }
}
