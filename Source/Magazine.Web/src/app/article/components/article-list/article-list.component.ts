import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../services/article.service';
import { Article } from '../../models/article';
import { ArticleListItem } from './article-list-item';
import { AuthorizationService, AuthenticationResultStatus } from 'src/app/authorization/authorization.service';
import { ReturnUrlType } from 'src/app/authorization/authorization.constants';

@Component({
    selector: 'app-article-list',
    templateUrl: './article-list.component.html',
    styleUrls: ['./article-list.component.less']
})
export class ArticleListComponent implements OnInit {
    articles: ArticleListItem[];
    selectedId: number;

    constructor(private route: ActivatedRoute,
                private router: Router,
                private articleService: ArticleService,
                private authorizeService: AuthorizationService) {

        articleService.onAdd.subscribe(id => {
            this.articleService.getArticle(id)
                .subscribe(article => {
                    this.articles.push(new ArticleListItem(article.id, article.title));
                })
        });

        articleService.onUpdate.subscribe(id => {
            // update
        });

        articleService.onDelete.subscribe(id => {
            // delete
        });
    }

    async ngOnInit() {
        try {
            await this.processLoginCallback();

            this.route.paramMap.subscribe(
                params => {
                    this.selectedId = +params.get('id');
                    this.articleService.getArticles()
                        .subscribe(
                            articles => {
                                this.articles = articles.map(article => new ArticleListItem(article.id, article.title));
                            },
                            () => alert('Error while fetching articles')
                        )
                });
        }
        catch(error) {
            console.log(error)
        }
    }

    navigateToArticle(article: Article = null) {
        if (article !== null) {
            this.router.navigate(['/article', article.id]);
            return;
        }

        this.router.navigate(['/articles/create']);
    }

    private async processLoginCallback(): Promise<void> {
        const url = window.location.href;

        const result = await this.authorizeService.completeSignIn(url);

        switch (result.status) {
            case AuthenticationResultStatus.Success:
                this.authorizeService.isUserAuthenticated = true;
                location.reload();
                break;
            case AuthenticationResultStatus.Fail:
                this.authorizeService.message.next(result.message);
                break;
            case AuthenticationResultStatus.Redirect:
                throw new Error('Should not redirect.');
        }
    }
}
