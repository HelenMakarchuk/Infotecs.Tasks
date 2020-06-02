import { Injectable } from "@angular/core";
import { EntityServiceEvent } from "../../contracts/event/entity.service.event";
import { ArticleService } from "src/app/services/article/article.service";

/** Событие сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export abstract class ArticleServiceEvent extends EntityServiceEvent {

    constructor(className: string) {
        super(className);
    }

    abstract visit(service: ArticleService): void;
}
