import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ArticleEntity } from '../../articles/article';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  addArticle(article: ArticleEntity) {
    return this.http.post<ArticleEntity>(`${environment.apiUrl}/article`, article)
      .pipe(catchError(this.handleError));
  }

  deleteArticle(id: number) {
    return this.http.delete<ArticleEntity>(`${environment.apiUrl}/article/${id}`)
      .pipe(catchError(this.handleError));
  }

  updateArticle(article: ArticleEntity) {
    return this.http.put<ArticleEntity>(`${environment.apiUrl}/article/${article.id}`, article)
      .pipe(catchError(this.handleError));
  }

  getArticle(id: number) {
    return this.http.get<ArticleEntity>(`${environment.apiUrl}/article/${id}`)
      .pipe(catchError(this.handleError));
  }

  getArticles() {
    return this.http.get<ArticleEntity[]>(`${environment.apiUrl}/article`)
      .pipe(catchError(this.handleError));
  }

  handleError(error: HttpErrorResponse) {
    console.log(error);
    return throwError(error);
  }
}
