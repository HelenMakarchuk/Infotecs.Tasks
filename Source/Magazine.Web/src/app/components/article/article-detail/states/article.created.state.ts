import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { ArticleService } from "src/app/services/article/article.service";
import { ArticleDeletedState } from "./article.deleted.state";
import { ServerCommunicationService } from "src/app/contracts/service/server-communication.service";
import { ArticleEditableState } from "./article.editable.state";

/** Статья создана */
@Injectable({
    providedIn: 'root'
})
export class ArticleCreatedState extends ArticleState {

    constructor(protected articleService: ArticleService,
                protected serverCommunicationService: ServerCommunicationService) {
        super(articleService, serverCommunicationService);
    }

    create(): void {
        throw new Error("Article already created.");
    }

    update(): void {
        throw new Error("Article needs to be editable to update.");
    }

    delete(): void {
        this.articleService.deleteArticle(this.articleContext.article.id)
            .subscribe(
                () => {
                    this.articleContext.transitionTo(new ArticleDeletedState(this.articleService, this.serverCommunicationService));
                    this.articleContext.navigateToArticles();
                },
                () => alert('Error while deleting article')
            );
    }
}
