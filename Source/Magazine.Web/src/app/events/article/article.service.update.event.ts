import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceEvent } from "./article.service.event";
import { Article } from "src/app/models/article/article";

/** Событие "Обновление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceUpdateEvent extends ArticleServiceEvent {

    /** Статья. */
    article: Article;

    constructor(article: Article) {
        super("ArticleServiceUpdateEvent");
        this.article = article;
    }

    visit(service: ArticleService): void {
        service.onUpdate.next(this.article);
    }
}
