import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Article } from './article';
import { ConfigService } from '../config/config.service';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  addArticle(article: Article) {
    debugger;

    return this.http.post<Article>(`${ConfigService.settings.apiUrl}/article`, article)
      .pipe(catchError(this.handleError));
  }

  deleteArticle(id: number) {
    debugger;

    return this.http.delete<Article>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(catchError(this.handleError));
  }

  updateArticle(article: Article) {
    debugger;

    return this.http.put<Article>(`${ConfigService.settings.apiUrl}/article/${article.id}`, article)
      .pipe(catchError(this.handleError));
  }

  getArticle(id: number) {
    return this.http.get<Article>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getArticles() {
    return this.http.get<Article[]>(ConfigService.settings.apiUrl + '/article')
      .pipe(
        catchError(this.handleError)
      );
  }

  handleError(error: HttpErrorResponse) {
    debugger;

    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side errors
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side errors
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    window.alert(errorMessage);
    return throwError(errorMessage);
  }
}
