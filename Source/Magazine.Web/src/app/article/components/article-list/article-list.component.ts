import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../services/article.service';
import { Article } from '../../models/article';
import { ArticleListItem } from './article-list-item';

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
                private articleService: ArticleService) {
        articleService.onAdd.subscribe(id => {
            this.articleService.getArticle(id).subscribe(obArticle => {
                obArticle.subscribe(article => this.articles.push(new ArticleListItem(article.id, article.title)));
            })
        });

        articleService.onUpdate.subscribe(id => {
            // update
        });

        articleService.onDelete.subscribe(id => {
            // delete
        });
    }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                this.selectedId = +params.get('id');
                this.articleService.getArticles()
                    .subscribe(
                        result => {
                            result.subscribe(r => this.articles = r.map(article => new ArticleListItem(article.id, article.title)));
                        },
                        () => alert('Error while fetching articles')
                    )
            });
    }

    navigateToArticle(article: Article = null) {
        if (article !== null) {
            this.router.navigate(['/article', article.id]);
            return;
        }

        this.router.navigate(['/articles/create']);
    }
}
