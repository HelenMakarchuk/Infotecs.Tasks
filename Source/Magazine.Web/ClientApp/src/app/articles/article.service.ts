import { Injectable, inject, Component } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { ConfigService } from '../config/config.service';
import { ArticleEntity } from './article';
import { throwError } from 'rxjs';
import { FetchPipe } from '../fetch-pipe';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient,
              private fetchPipe: FetchPipe) { }

  addArticle(article: ArticleEntity) {
    debugger;

    return this.http.post<ArticleEntity>(`${ConfigService.settings.apiUrl}/article`, article)
      .pipe(catchError(this.handleError))
  }

  deleteArticle(id: number) {
    debugger;

    return this.http.delete<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(catchError(this.handleError))
  }

  updateArticle(article: ArticleEntity) {
    debugger;

    return this.http.put<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${article.id}`, article)
      .pipe(catchError(this.handleError))
  }

  getArticle(id: number) {
    debugger;

    return this.http.get<ArticleEntity>(ConfigService.settings.graphqlUrl,
      {
        params: new HttpParams()
          .append('query', `{ article(id: ${id}) { id, title, body, teaser, accountId } }`)
      })
      .pipe(catchError(this.handleError))
      .pipe(map(result => this.fetchPipe.transform(result, 'article')));
  }

  getArticles() {
    debugger;

    return this.http.get<ArticleEntity[]>(ConfigService.settings.graphqlUrl,
      {
        params: new HttpParams()
          .append('query', '{ articles { id, title } }')
      })
      .pipe(catchError(this.handleError))
      .pipe(map(result => this.fetchPipe.transform(result, 'articles')));
  }

  // TODO: handle-error.service.ts
  handleError(error: HttpErrorResponse) {
    debugger;

    console.log(error);
    return throwError(error);
  }
}
