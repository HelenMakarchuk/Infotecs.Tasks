import { Article } from "../article/article";
import { Account } from "../account/account";

/** Сущность "Комментарий" */
export class Comment {
    id: number;
    body: string;
    articleid: number;
    accountid: number;
    article: Article;
    account: Account;
}
