import { switchMap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css']
})
export class ArticleDetailComponent implements OnInit {

  article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, accountId: 0, account: null, comments: null };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService
  ) { }

  ngOnInit() {
    this.article = this.route.paramMap
      .pipe(switchMap((params: ParamMap) => this.service.getArticle(+params.get('id')))) as unknown as ArticleEntity;
  }

  navigateToArticles() {
    this.router.navigate(['/articles', { id: this.article.id }]);
  }

  updateArticle() {
    this.service.updateArticle(this.article)
      .subscribe(
        val => {
          console.log("Value.", val);
          this.navigateToArticles();
        },
        response => {
          console.log("Error.", response);
        },
        () => {
          console.log("Completed.");
        }
      );
  }

  deleteArticle() {
    this.service.deleteArticle(this.article.id)
      .subscribe(
        val => {
          console.log("Value.", val);
          this.navigateToArticles();
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
