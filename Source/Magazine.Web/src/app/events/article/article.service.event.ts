import { Injectable } from "@angular/core";
import { EntityServiceEvent } from "../../contracts/event/entity.service.event";
import { ArticleService } from "src/app/services/article/article.service";
import { jsonProperty } from "ts-serializable";

/** Событие сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export abstract class ArticleServiceEvent extends EntityServiceEvent {

    @jsonProperty()
    /** Идентификатор статьи. */
    protected id: number;

    constructor(className: string,
                id: number) {
                    debugger;
        super(className);
        this.id = id;
    }

    abstract visit(service: ArticleService): void;
}
