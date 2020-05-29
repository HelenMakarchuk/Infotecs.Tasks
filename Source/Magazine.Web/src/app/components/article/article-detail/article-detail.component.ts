import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleEntity } from '../../../models/article/article';
import { ArticleService } from '../../../services/article/article.service';
import { ServerCommunicationService } from '../../../contracts/service/server-communication.service';
import { ApplicationComponent } from '../../../contracts/component/application.component';

@Component({
    selector: 'app-article-detail',
    templateUrl: './article-detail.component.html',
    styleUrls: ['./article-detail.component.less']
})
export class ArticleDetailComponent extends ApplicationComponent implements OnInit
{

    article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, account: null, accountId: 0 };
    isReadonly = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private articleService: ArticleService,
        private serverCommunicationService: ServerCommunicationService) {
        super();

        serverCommunicationService.subscribe(this);
    }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                if (params.get('id') !== null) {
                    this.articleService.getArticle(+params.get('id'))
                        .subscribe(
                            result => {
                                this.article = result as ArticleEntity;
                                this.isReadonly = true;
                            },
                            () => alert('Error while opening article')
                        );
                }
            }
        );
    }

    navigateToArticles() {
        this.router.navigate(['/articles', { id: this.article !== null ? this.article.id : null }]);
    }

    createArticle() {
        this.isReadonly = true;

        this.articleService.addArticle(this.article)
            .subscribe(
                result => this.article = result as ArticleEntity,
                response => {
                    alert(`Error while creating article. ${response.error.Message}`);
                    this.isReadonly = false;
                }
            );
    }

    updateArticle() {
        this.isReadonly = !this.isReadonly;

        if (this.isReadonly === false)
            return;

        this.articleService.updateArticle(this.article)
            .subscribe(
                result => {
                    this.article = result as ArticleEntity;
                    this.serverCommunicationService.communicate(EventType.Update);
                },
                () => alert('Error while updating article')
            );
    }

    deleteArticle() {
        this.articleService.deleteArticle(this.article.id)
            .subscribe(
                () => this.navigateToArticles(),
                () => alert('Error while deleting article')
            );
    }
}
