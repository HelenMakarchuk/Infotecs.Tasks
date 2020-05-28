import { AccountEntity } from "../account/account";
import { CommentEntity } from "../comment/comment";

export class ArticleEntity {
    id: number;
    title: string;
    teaser: [];
    body: string;
    accountId: number;
    account: AccountEntity;
    comments: CommentEntity[];
}
