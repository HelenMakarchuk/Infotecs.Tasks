import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { Article } from "src/app/models/article/article";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleServiceUpdateEvent } from "src/app/events/article/article.service.update.event";
import { ServerCommunicationService } from "src/app/contracts/service/server-communication.service";
import { ArticleCreatedState } from "./article.created.state";

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
