import { Injectable } from "@angular/core";
import { ArticleState } from "./article.state";
import { ArticleCreatedState } from "./article.created.state";
import { Article } from "src/app/models/article/article";
import { ArticleService } from "src/app/services/article/article.service";
import { ServerCommunicationService } from "src/app/contracts/service/server-communication.service";

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
            (article: Article) => {
                this.articleContext.article = article;
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
