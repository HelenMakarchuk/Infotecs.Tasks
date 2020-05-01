import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../article.service';
import { ArticleEntity } from '../article';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  providers: [ArticleService],
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.less']
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
        debugger;
        this.selectedId = +params.get('id');
        return this.service.getArticles();
      }));

    //this.route.paramMap.subscribe(params => {
    //  debugger;

    //  this.selectedId = +params.get('id');

    //  return this.service.getArticles()
    //    .subscribe(
    //      val => {
    //        debugger;

    //        console.log("Value.", val);
    //        this.articles$ = val;
    //      },
    //      response => {
    //        debugger;

    //        console.log("Error.", response);
    //      },
    //      () => {
    //        debugger;

    //        console.log("Completed.");
    //      }
    //    );
    //});
  }

  navigateToArticle(isNew = false) {
    debugger;

    if (isNew === true)
      this.router.navigate(['/articles/create']);
    else
      this.router.navigate(['/article', this.selectedId]);
  }
}
