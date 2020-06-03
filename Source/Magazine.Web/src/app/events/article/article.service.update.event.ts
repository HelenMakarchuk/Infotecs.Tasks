import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceEvent } from "./article.service.event";

/** Событие "Обновление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceUpdateEvent extends ArticleServiceEvent {

    constructor(id: number) {
        super("ArticleServiceUpdateEvent", id);
    }

    visit(service: ArticleService): void {
        service.onUpdate.next(this.id);
    }
}
