import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../../services/article/article.service';
import { Article } from '../../../models/article/article';
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
        articleService.onAdd.subscribe(article => this.articles.push(new ArticleListItem(article.id, article.title)));
    }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                this.selectedId = +params.get('id');
                this.articleService.getArticles()
                    .subscribe(
                        result => {
                            this.articles = result.map(article => new ArticleListItem(article.id, article.title));
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
