import { switchMap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { ArticleService } from '../article.service';
import { Article } from '../article';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css']
})
export class ArticleDetailComponent implements OnInit {
  article$: Observable<Article>;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService
  ) { }


  ngOnInit() {
    this.article$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.service.getArticle(+params.get('id')))
    );
  }

  navigateToArticles(article: Article) {
    this.router.navigate(['/articles', { id: article.id }]);
  }

  updateArticle(article: Article) {
    this.service.updateArticle(article)
      .subscribe(
        val => {
          console.log("Value.", val);
          this.navigateToArticles(article);
        },
        response => {
          console.log("Error.", response);
        },
        () => {
          console.log("Completed.");
        }
    );
  }

  deleteArticle(article: Article) {
    this.service.deleteArticle(article.id)
      .subscribe(
        val => {
          console.log("Value.", val);
          this.navigateToArticles(article);
        },
        response => {
          console.log("Error.", response);
        },
        () => {
          console.log("Completed.");
        }
    );
  }
}
