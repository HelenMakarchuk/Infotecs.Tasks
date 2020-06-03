import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/article/services/article.service";
import { jsonProperty } from "ts-serializable";
import { EntityServiceEvent } from "../../contracts/entity.service.event";

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
        super(className);
        this.id = id;
    }

    abstract visit(service: ArticleService): void;
}
