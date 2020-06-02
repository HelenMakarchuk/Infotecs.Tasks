import { ArticleDetailComponent } from "../article-detail.component";
import { Article } from "src/app/models/article/article";
import { ArticleService } from "src/app/services/article/article.service";
import { ServerCommunicationService } from "src/app/contracts/service/server-communication.service";

/** Cостояниe сущности "Статья". */
export abstract class ArticleState {

    protected articleContext: ArticleDetailComponent;

    constructor(protected articleService: ArticleService,
                protected serverCommunicationService: ServerCommunicationService) { }

    setContext(articleContext: ArticleDetailComponent) {
        this.articleContext = articleContext;
    }

    /** Создание статьи. */
    abstract create(): void;

    /** Создание статьи. */
    abstract update(): void;

    /** Создание статьи. */
    abstract delete(): void;
}
