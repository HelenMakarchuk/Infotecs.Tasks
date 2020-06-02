import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceEvent } from "./article.service.event";
import { Article } from "src/app/models/article/article";

/** Событие "Удаление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceDeleteEvent extends ArticleServiceEvent {

    /** Идентификатор статьи. */
    id: number;

    constructor(id: number) {
        super("ArticleServiceDeleteEvent");
        this.id = id;
    }

    visit(service: ArticleService): void {
        service.onDelete.next(this.id);
    }
}
