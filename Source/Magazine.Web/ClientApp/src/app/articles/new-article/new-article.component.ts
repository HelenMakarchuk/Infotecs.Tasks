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

  article: Partial<Observable<Article>>;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService
  )
  {
    this.article = {};
  }

  createArticle(article: Article) {
    this.service.addArticle(article);
  }
}
