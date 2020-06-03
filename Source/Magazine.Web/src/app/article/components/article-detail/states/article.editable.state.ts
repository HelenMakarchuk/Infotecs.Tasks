import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { Article } from "src/app/article/models/article";
import { ArticleService } from "src/app/article/services/article.service";
import { ArticleCreatedState } from "./article.created.state";
import { ServerCommunicationService } from "src/app/server-communication/contracts/server-communication.service";
import { ArticleServiceUpdateEvent } from "src/app/server-communication/events/article/article.service.update.event";

/** Раннее созданная статья в режиме редактирования */
@Injectable({
    providedIn: 'root'
})
export class ArticleEditableState extends ArticleState {

    constructor(protected articleService: ArticleService,
                protected serverCommunicationService: ServerCommunicationService) {
        super(articleService, serverCommunicationService);
    }

    create(): void {
        throw new Error("Article already created.");
    }

    update(): void {
        this.articleService.updateArticle(this.articleContext.article)
            .subscribe(
                (article: Article) => {
                    this.articleContext.article = article;
                    this.serverCommunicationService.send(new ArticleServiceUpdateEvent(article.id));
                    this.articleContext.transitionTo(new ArticleCreatedState(this.articleService, this.serverCommunicationService));
                },
                () => alert('Error while updating article')
            );
    }

    delete(): void {
        throw new Error("Article needs to be saved before deleting.");
    }
}
