import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { throwError, Observable, Subject, from } from 'rxjs';
import { ServerCommunicationService } from 'src/app/server-communication/contracts/server-communication.service';
import { EntityService } from 'src/app/contracts/service/entity.service';
import { Comment } from 'src/app/comment/models/comment';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Injectable({
    providedIn: 'root'
})
export class CommentService extends EntityService {

    onAdd: Subject<number>;

    constructor(private http: HttpClient,
                private authService: AuthorizeService,
                private serverCommunicationService: ServerCommunicationService) {
        super();
        this.onAdd = new Subject<number>();
        this.serverCommunicationService.subscribe(this);
    }

    addComment(comment: Comment): Observable<Comment> {
        return this.http.post<Comment>(`${environment.apiUrl}/comment`, comment)
            .pipe(catchError(this.handleError));
    }

    deleteComment(id: number): Observable<Comment> {
        return this.http.delete<Comment>(`${environment.apiUrl}/comment/${id}`)
            .pipe(catchError(this.handleError));
    }

    updateComment(comment: Comment): Observable<Comment> {
        return this.http.put<Comment>(`${environment.apiUrl}/comment/${comment.id}`, comment)
            .pipe(catchError(this.handleError));
    }

    getComment(id: number): Observable<Observable<Comment>> {
        return this.authService.getAuthorizationHeaders()
            .pipe(
                map(headers => {
                    return this.http.get<Comment>(`${environment.apiUrl}/comment/${id}`, { headers: headers })
                .pipe(catchError(this.handleError));
                })
            );
    }

    getComments(): Observable<Observable<Comment[]>> {
        return from(this.authService.getAuthorizationHeaders())
            .pipe(
                map(headers => {
                    return this.http.get<Comment[]>(`${environment.apiUrl}/comment`, { headers: headers })
                    .pipe(catchError(this.handleError));
                })
            );
    }

    handleError(error: HttpErrorResponse) {
        console.log(error);
        return throwError(error);
    }
}
