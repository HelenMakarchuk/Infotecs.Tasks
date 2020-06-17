/** Аргумент события комментария. */
export class CommentEventArgument {
    id: number;
    articleId: number;

    constructor(id: number,
                articleId: number) {
        this.id = id;
        this.articleId = articleId;
    }
}
