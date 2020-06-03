import { Injectable } from "@angular/core";
import { EntityServiceEvent } from "../../contracts/event/entity.service.event";
import { CommentService } from "src/app/services/comment/comment.service";

/** Событие сервиса сущности "Комментарий". */
@Injectable({
    providedIn: 'root'
})
export abstract class CommentServiceEvent extends EntityServiceEvent {

    /** Идентификатор комментария. */
    id: number;

    constructor(className: string,
                id: number) {
        super(className);
        this.id = id;
    }

    abstract visit(service: CommentService): void;
}
