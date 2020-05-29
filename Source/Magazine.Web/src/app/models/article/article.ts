import { AccountEntity } from "../account/account";

export class ArticleEntity {
    id: number;
    title: string;
    teaser: [];
    body: string;
    accountId: number;
    account: AccountEntity;
}
