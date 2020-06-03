import { Account } from "../../account/models/account";

/** Сущность "Статья" */
export class Article {
    id: number;
    title: string;
    teaser: [];
    body: string;
    accountId: number;
    account: Account;
}
