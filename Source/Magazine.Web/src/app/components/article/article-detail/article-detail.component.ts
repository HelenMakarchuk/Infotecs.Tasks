import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Article } from '../../../models/article/article';
import { ArticleService } from '../../../services/article/article.service';
import { ApplicationNotificationService } from 'src/app/services/notification/application.notification.service';
import { ArticleState } from './states/article.state';
import { ArticleCreatedState } from './states/article.created.state';
import { ServerCommunicationService } from 'src/app/contracts/service/server-communication.service';
import { ArticleNewState } from './states/article.new.state';
import { ArticleEditableState } from './states/article.editable.state';
import { filter } from 'rxjs/operators';

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
                private applicationNotificationService: ApplicationNotificationService,
                private serverCommunicationService : ServerCommunicationService) {

        this.article = { id: 0, title: '', body: '', teaser: null, account: null, accountId: 0 };


        this.articleService.onUpdate
            .pipe(filter(id => id === this.article.id))
            .subscribe(() => {
                this.applicationNotificationService.notify("This article was changed by another user. Refresh this article to get last changes.");
            });

        this.articleService.onDelete
            .pipe(filter(id => id === this.article.id))
            .subscribe(() => {
                this.applicationNotificationService.notify("This article was deleted by another user.");
            });
    }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                if (params.get('id') !== null) {
                    this.articleService.getArticle(+params.get('id')).subscribe(
                        (article: Article) => {
                            this.article = article;
                            this.transitionTo(new ArticleCreatedState(this.articleService, this.serverCommunicationService));
                        },
                        () => alert('Error while opening article')
                    );
                }
                else {
                    this.transitionTo(new ArticleNewState(this.articleService, this.serverCommunicationService));
                }
            }
        );
    }

    public transitionTo(state: ArticleState): void {
        this.state = state;
        this.state.setContext(this);
    }

    navigateToArticles() {
        this.router.navigate(['/articles', { id: this.article !== null ? this.article.id : null }]);
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
