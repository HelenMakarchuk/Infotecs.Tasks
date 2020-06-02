import { Injectable } from "@angular/core";
import { EntityServiceEvent } from "../../contracts/event/entity.service.event";
import { CommentService } from "src/app/services/comment/comment.service";

/** Событие сервиса сущности "Комментарий". */
@Injectable({
    providedIn: 'root'
})
export abstract class CommentServiceEvent extends EntityServiceEvent {

    constructor(className: string) {
        super(className);
    }

    abstract visit(service: CommentService): void;
}
