import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { ConfigService } from '../config/config.service';
import { ArticleEntity } from './article';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  addArticle(article: ArticleEntity) {
    debugger;

    return this.http.post<ArticleEntity>(`${ConfigService.settings.apiUrl}/article`, article)
      .pipe(catchError(this.handleError));
  }

  deleteArticle(id: number) {
    debugger;

    return this.http.delete<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(catchError(this.handleError));
  }

  updateArticle(article: ArticleEntity) {
    debugger;

    return this.http.put<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${article.id}`, article)
      .pipe(catchError(this.handleError));
  }

  getArticle(id: number) {
    debugger;

    return this.http.get<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getArticles() {
    debugger;

    return this.http.get<ArticleEntity[]>(`${ConfigService.settings.apiUrl}/article`)
      .pipe(
        catchError(this.handleError)
      );
  }

  // TODO: handle-error.service.ts
  handleError(error: HttpErrorResponse) {
    debugger;

    console.log(error);

    alert('Error while executing operation');

    return throwError(error);
  }
}
