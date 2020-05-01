import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.less']
})
export class ArticleDetailComponent implements OnInit {

  article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, accountId: 0, account: null, comments: null };
  isReadonly = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService) { }

  ngOnInit() {
    debugger;

    this.route.paramMap.subscribe(
      params => {
        if (params.get('id') !== null) {
          this.service.getArticle(+params.get('id'))
            .subscribe(
              value => {
                debugger;
                this.article = value;
                this.isReadonly = true;
              }
            );
        }
      }
    );
  }

  createArticle() {
    debugger;
    this.service.addArticle(this.article);
  }

  navigateToArticles() {
    debugger;

    this.router.navigate(['/articles', { id: this.article.id }]);
  }

  updateArticle() {
    debugger;
    this.isReadonly = !this.isReadonly;

    if (this.isReadonly === false)
      return;

    this.service.updateArticle(this.article)
      .subscribe(
        value => {
          debugger;
          this.article = value;
        },
        response => {
          debugger;
          console.log("Error.", response);
        }
      );
  }

  deleteArticle() {
    debugger;
    this.service.deleteArticle(this.article.id)
      .subscribe(
        () => {
          this.navigateToArticles();
        },
        response => {
          console.log("Error.", response);
        }
      );
  }
}
