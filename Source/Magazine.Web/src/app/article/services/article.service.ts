import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, mergeMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Article } from '../models/article';
import { throwError, Observable, Subject } from 'rxjs';
import { EntityService } from 'src/app/contracts/service/entity.service';
import { ServerCommunicationService } from 'src/app/server-communication/contracts/server-communication.service';
import { AuthorizationService } from 'src/app/authorization/authorization.service';

@Injectable({
    providedIn: 'root'
})
export class ArticleService extends EntityService {

    onAdd: Subject<number>;
    onUpdate: Subject<number>;
    onDelete: Subject<number>;

    constructor(private http: HttpClient,
                private authService: AuthorizationService,
                private serverCommunicationService: ServerCommunicationService) {
        super();
        this.onAdd = new Subject<number>();
        this.onUpdate = new Subject<number>();
        this.onDelete = new Subject<number>();
        this.serverCommunicationService.subscribe(this);
    }

    addArticle(article: Article): Observable<Article> {
        return this.authService.getAuthorizationHeaders()
            .pipe(
                mergeMap(headers => {
                    return this.http.post<Article>(`${environment.apiUrl}/article`, article, { headers: headers })
                        .pipe(catchError(this.handleError));
                })
            );
    }

    deleteArticle(id: number): Observable<Article> {
        return this.authService.getAuthorizationHeaders()
            .pipe(
                mergeMap(headers => {
                    return this.http.delete<Article>(`${environment.apiUrl}/article/${id}`, { headers: headers })
                        .pipe(catchError(this.handleError));
                })
            );
    }

    updateArticle(article: Article): Observable<Article> {
        return this.authService.getAuthorizationHeaders()
            .pipe(
                mergeMap(headers => {
                    return this.http.put<Article>(`${environment.apiUrl}/article/${article.id}`, article, { headers: headers })
                        .pipe(catchError(this.handleError));
                })
            );
    }

    getArticle(id: number): Observable<Article> {
        return this.http.get<Article>(`${environment.apiUrl}/article/${id}`)
            .pipe(catchError(this.handleError));
    }

    getArticles(): Observable<Article[]> {
        return this.http.get<Article[]>(`${environment.apiUrl}/article`)
            .pipe(catchError(this.handleError));
    }

    handleError(error: HttpErrorResponse) {
        console.log(error);
        return throwError(error);
    }
}
