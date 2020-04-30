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
    return this.http.get<ArticleEntity>(`${ConfigService.settings.apiUrl}/article/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getArticles() {
    return this.http.get<ArticleEntity[]>(`${ConfigService.settings.apiUrl}/article`)
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
