import { Injectable } from "@angular/core";
import { CommentServiceEvent } from "./comment.service.event";
import { Comment } from "src/app/comment/models/comment";
import { CommentService } from "src/app/comment/services/comment.service";

/** Событие "Добавление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class CommentServiceAddEvent extends CommentServiceEvent {

    constructor(id: number) {
        super("CommentServiceAddEvent", id);
    }

    visit(service: CommentService): void {
        service.onAdd.next(this.id);
    }
}
