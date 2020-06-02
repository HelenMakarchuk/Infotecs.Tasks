import { Injectable } from "@angular/core";
import { CommentServiceEvent } from "./comment.service.event";
import { Comment } from "src/app/models/comment/comment";
import { CommentService } from "src/app/services/comment/comment.service";

/** Событие "Добавление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class CommentServiceAddEvent extends CommentServiceEvent {

    /** Статья. */
    comment: Comment;

    constructor(comment: Comment) {
        super("CommentServiceAddEvent");
        this.comment = comment;
    }

    visit(service: CommentService): void {
        service.onAdd.next(this.comment);
    }
}
