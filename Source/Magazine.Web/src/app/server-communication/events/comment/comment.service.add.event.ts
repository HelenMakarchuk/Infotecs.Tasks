import { Injectable } from "@angular/core";
import { CommentServiceEvent } from "./comment.service.event";
import { CommentService } from "src/app/comment/services/comment.service";
import { CommentEventArgument } from "./comment-event-argument";

/** Событие "Добавление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class CommentServiceAddEvent extends CommentServiceEvent {

    constructor(eventArgument: CommentEventArgument) {
        super("CommentServiceAddEvent", eventArgument);
    }

    visit(service: CommentService): void {
        service.onAdd.next(this.eventArgument);
    }
}
