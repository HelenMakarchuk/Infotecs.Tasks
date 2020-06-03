import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { ArticleService } from "src/app/article/services/article.service";
import { ServerCommunicationService } from "src/app/server-communication/contracts/server-communication.service";

/** Статья удалена */
@Injectable({
    providedIn: 'root'
})
export class ArticleDeletedState extends ArticleState {

    constructor(protected articleService: ArticleService,
                protected serverCommunicationService: ServerCommunicationService) {
        super(articleService, serverCommunicationService);
    }

    create(): void {
        throw new Error("Article was deleted.");
    }

    update(): void {
        throw new Error("Article was deleted.");
    }

    delete(): void {
        throw new Error("Article already deleted.");
    }
}
