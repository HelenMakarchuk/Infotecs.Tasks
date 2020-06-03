import { Article } from "../../article/models/article";
import { Account } from "../../account/models/account";

/** Сущность "Комментарий" */
export class Comment {
    id: number;
    body: string;
    articleId: number;
    accountId: number;
    article: Article;
    account: Account;
}
