import { Injectable } from "@angular/core";
import { ApplicationComponentEvent } from "../../contracts/event/application.component.event";
import { ArticleDetailComponent } from "../../components/article/article-detail/article-detail.component";

/** Событие компонента "Статья". */
@Injectable({
    providedIn: 'root'
})
export class ArticleComponentEvent extends ApplicationComponentEvent {

    message: string;

    visit(component: ArticleDetailComponent): void {
        alert(this.message);
    }
}
