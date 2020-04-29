import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ArticleService } from '../article.service';
import { Article } from '../article';

@Component({
  providers: [ArticleService],
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css']
})
export class ArticleListComponent implements OnInit {
  articles$: Observable<Article[]>;
  selectedId: number;

  constructor(
    private service: ArticleService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    debugger;
    this.articles$ = this.route.paramMap.pipe(
      switchMap(params => {
        this.selectedId = +params.get('id');
        return this.service.getArticles();
      })
    );
  }
}
