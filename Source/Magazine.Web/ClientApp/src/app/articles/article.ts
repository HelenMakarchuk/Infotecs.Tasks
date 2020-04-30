import { AccountEntity } from "../accounts/account";
import { CommentEntity } from "../comments/comment";

export interface ArticleEntity {
  id: number;
  title: string;
  teaser: [];
  body: string;
  accountId: number;
  //account: AccountEntity;
  comments: CommentEntity[];
}
