import { Injectable } from "@angular/core";
import { CommentService } from "src/app/comment/services/comment.service";
import { EntityServiceEvent } from "../../contracts/entity.service.event";

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
