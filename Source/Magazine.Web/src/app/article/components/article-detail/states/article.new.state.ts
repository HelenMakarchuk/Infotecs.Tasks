import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { ArticleCreatedState } from "./article.created.state";
import { ArticleService } from "src/app/article/services/article.service";
import { ServerCommunicationService } from "src/app/server-communication/contracts/server-communication.service";
import { ArticleServiceAddEvent } from "src/app/server-communication/events/article/article.service.add.event";

/** Новая статья */
@Injectable({
    providedIn: 'root'
})
export class ArticleNewState extends ArticleState {

    constructor(protected articleService: ArticleService,
                protected serverCommunicationService: ServerCommunicationService) {
        super(articleService, serverCommunicationService);
    }

    create(): void {
        this.articleService.addArticle(this.articleContext.article).subscribe(
            article => {
                this.articleContext.article = article;
                this.serverCommunicationService.send(new ArticleServiceAddEvent(article.id));
                this.articleContext.transitionTo(new ArticleCreatedState(this.articleService, this.serverCommunicationService));
            },
            response => {
                alert(`Error while creating article. ${response.error.Message}`);
            }
        );
    }

    update(): void {
        throw new Error("Article needs to be created before updating.");
    }

    delete(): void {
        throw new Error("Article needs to be created before deleting.");
    }
}
