import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { ArticleService } from '../article.service';
import { Article } from '../article';

@Component({
  selector: 'app-new-article',
  templateUrl: './new-article.component.html',
  styleUrls: ['./new-article.component.css']
})
export class NewArticleComponent {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private articleService: ArticleService
  ) { }

  createArticle(title: string): void {
    debugger;

    const newArticle: Article = {
      title: '', body: '', teaser: [], accountId: 1, account: null, comments: [], id: 0
    };

    this.articleService.addArticle(newArticle)
      .subscribe(
        val => {
          console.log("Value.", val);
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
