import { ArticleEntity } from "../article/article";
import { AccountEntity } from "../account/account";

export class CommentEntity {
    id: number;
    body: string;
    articleid: number;
    accountid: number;
    article: ArticleEntity;
    account: AccountEntity;
}
