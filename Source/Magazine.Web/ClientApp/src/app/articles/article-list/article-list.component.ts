import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';

@Component({
  providers: [ArticleService],
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css']
})
export class ArticleListComponent implements OnInit {
  articles$: Observable<ArticleEntity[]>;
  selectedId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ArticleService
  ) { }

  ngOnInit() {
    this.articles$ = this.route.paramMap.pipe(
      switchMap(params => {
        this.selectedId = +params.get('id');
        return this.service.getArticles();
      })
    );
  }

  navigateToNewArticle() {
    this.router.navigate(['/articles/create']);
  }
}
