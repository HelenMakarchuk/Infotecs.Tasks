import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css']
})
export class ArticleDetailComponent implements OnInit {

  article: ArticleEntity = { id: 0, title: '', body: '', teaser: null, accountId: 0, comments: null };
  isReadonly = true;
  updateButtonName: string = 'Edit';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService) { }

  ngOnInit() {
    debugger;
    this.route.paramMap.subscribe(params => {
      debugger;

      this.service.getArticle(+params.get('id'))
        .subscribe(
          val => {
            debugger;

            console.log("Value.", val);
            this.article = val;
          },
          response => {
            debugger;

            console.log("Error.", response);
          },
          () => {
            debugger;

            console.log("Completed.");
          }
        );
    });
  }

  navigateToArticles() {
    this.router.navigate(['/articles', { id: this.article.id }]);
  }

  updateArticle() {
    this.isReadonly = !this.isReadonly;
    this.updateButtonName = this.isReadonly === true ? 'Edit' : 'Save';

    if (this.isReadonly === false)
      return;

    debugger;
    this.service.updateArticle(this.article)
      .subscribe(
        val => {
          debugger;
          this.article = val;
        },
        response => {
          debugger;

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
          debugger;

          console.log("Value.", val);
          this.navigateToArticles();
        },
        response => {
          debugger;

          console.log("Error.", response);
        },
        () => {
          console.log("Completed.");
        }
      );
  }
}
