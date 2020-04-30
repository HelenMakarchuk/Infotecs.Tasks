import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';

@Component({
  selector: 'app-new-article',
  templateUrl: './new-article.component.html',
  styleUrls: ['./new-article.component.css']
})
export class NewArticleComponent implements OnInit {

  article: ArticleEntity;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService
  ) { }

  ngOnInit() {
    debugger;
    this.article = { id: 0, title: '', body: '', teaser: null, accountId: 0, /*account: null,*/ comments: null };
  }

  navigateToArticles() {
    this.router.navigate(['/articles', { id: this.article.id }]);
  }

  createArticle() {
    this.service.addArticle(this.article)
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
