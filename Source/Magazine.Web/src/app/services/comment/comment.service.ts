import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { throwError, Observable, Subject } from 'rxjs';
import { ServerCommunicationService } from 'src/app/contracts/service/server-communication.service';
import { EntityService } from 'src/app/contracts/service/entity.service';
import { Comment } from 'src/app/models/comment/comment';

@Injectable({
    providedIn: 'root'
})
export class CommentService extends EntityService {

    private comments: Comment[];

    onAdd: Subject<Comment>;

    constructor(private http: HttpClient,
                private serverCommunicationService: ServerCommunicationService) {
        super();
        this.onAdd = new Subject<Comment>();
        this.serverCommunicationService.subscribe(this);
        this.onAdd.subscribe(comment => {
            this.comments.push(comment);
        });
    }

    addComment(comment: Comment) {
        return this.http.post<Comment>(`${environment.apiUrl}/comment`, comment)
            .pipe(catchError(this.handleError));
    }

    deleteComment(id: number) {
        return this.http.delete<Comment>(`${environment.apiUrl}/comment/${id}`)
            .pipe(catchError(this.handleError));
    }

    updateComment(comment: Comment) {
        return this.http.put<Comment>(`${environment.apiUrl}/comment/${comment.id}`, comment)
            .pipe(catchError(this.handleError));
    }

    getComment(id: number) {
        return this.http.get<Comment>(`${environment.apiUrl}/comment/${id}`)
            .pipe(catchError(this.handleError));
    }

    getComments(): Observable<Comment[]> {
        return this.http.get<Comment[]>(`${environment.apiUrl}/comment`)
            .pipe(catchError(this.handleError));
    }

    handleError(error: HttpErrorResponse) {
        console.log(error);
        return throwError(error);
    }
}
