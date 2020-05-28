import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../../services/article/article.service';
import { ArticleEntity } from '../../../models/article/article';

@Component({
    selector: 'app-article-list',
    templateUrl: './article-list.component.html',
    styleUrls: ['./article-list.component.less']
})
export class ArticleListComponent implements OnInit {
    articles: ArticleEntity[];
    selectedId: number;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: ArticleService
    ) { }

    ngOnInit() {
        this.route.paramMap.subscribe(
            params => {
                this.selectedId = +params.get('id');
                this.service.getArticles()
                    .subscribe(
                        result => {
                            this.articles = result as ArticleEntity[];
                        },
                        () => alert('Error while opening article')
                    )
            });
    }

    navigateToArticle(article: ArticleEntity = null) {
        if (article !== null) {
            this.router.navigate(['/article', article.id]);
            return;
        }

        this.router.navigate(['/articles/create']);
    }
}
