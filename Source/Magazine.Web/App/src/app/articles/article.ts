export interface Article {
  id: number;
  title: string;
  teaser: [];
  body: string;
  accountId: number;
  account: Account;
  comments: Comment[];
}
