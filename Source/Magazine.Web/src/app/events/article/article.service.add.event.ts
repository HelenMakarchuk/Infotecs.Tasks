import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceEvent } from "./article.service.event";
import { Article } from "src/app/models/article/article";

/** Событие "Добавление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceAddEvent extends ArticleServiceEvent {

    constructor(id: number) {
        super("ArticleServiceAddEvent", id);
    }

    visit(service: ArticleService): void {
        service.onAdd.next(this.id);
    }
}
