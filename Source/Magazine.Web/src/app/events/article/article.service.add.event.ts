import { Injectable } from "@angular/core";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceEvent } from "./article.service.event";
import { Article } from "src/app/models/article/article";

/** Событие "Добавление" сервиса сущности "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleServiceAddEvent extends ArticleServiceEvent {

    /** Статья. */
    article: Article;

    constructor(article: Article) {
        super("ArticleServiceAddEvent");
        this.article = article;
    }

    visit(service: ArticleService): void {
        service.onAdd.next(this.article);
    }
}
