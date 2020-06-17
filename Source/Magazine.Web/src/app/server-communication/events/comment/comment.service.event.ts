import { Injectable } from "@angular/core";
import { CommentService } from "src/app/comment/services/comment.service";
import { EntityServiceEvent } from "../../contracts/entity.service.event";
import { CommentEventArgument } from "./comment-event-argument";

/** Событие сервиса сущности "Комментарий". */
@Injectable({
    providedIn: 'root'
})
export abstract class CommentServiceEvent extends EntityServiceEvent {

    eventArgument: CommentEventArgument;

    constructor(className: string,
                eventArgument: CommentEventArgument) {
        super(className);
        this.eventArgument = eventArgument;
    }

    abstract visit(service: CommentService): void;
}
