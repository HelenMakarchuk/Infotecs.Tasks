import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { ArticleService } from "src/app/article/services/article.service";
import { ArticleDeletedState } from "./article.deleted.state";
import { ServerCommunicationService } from "src/app/server-communication/contracts/server-communication.service";

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
                article => {
                    this.articleContext.transitionTo(new ArticleDeletedState(this.articleService, this.serverCommunicationService));
                    this.articleContext.navigateToArticles();
                },
                error => {
                    console.log(error);
                    alert('Error while deleting article');
                }
            );
    }
}
