import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/article/services/article.service";
import { ArticleServiceEvent } from "./article.service.event";

/** Событие "Удаление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceDeleteEvent extends ArticleServiceEvent {

    constructor(id: number) {
        super("ArticleServiceDeleteEvent", id);
    }

    visit(service: ArticleService): void {
        service.onDelete.next(this.id);
    }
}
