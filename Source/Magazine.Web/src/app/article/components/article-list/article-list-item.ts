/** Элемент списка статей. */
export class ArticleListItem {
    id: number;
    title: string;

    constructor(id: number,
                title: string) {
        this.id = id;
        this.title = title;
    }
}
