import { Injectable } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ApplicationComponent } from "../component/application.component";
import { ApplicationComponentEvent } from "../event/application.component.event";
import { map } from "rxjs/operators";
import { ArticleComponentEvent } from "../../events/article/article.component.event";

/** Сервис взаимодействия с сервером. */
@Injectable({
    providedIn: 'root'
})
export abstract class ServerCommunicationService {

    protected serverEvent = new Subject<ApplicationComponentEvent>();

    private classMapping = {
        'ArticleComponentEvent': ArticleComponentEvent,
    };

    /** Подписывание на получение событий сервера. */
    subscribe(component: ApplicationComponent): void {
        this.serverEvent
            .pipe(map(event => Object.assign(new this.classMapping[event.className](), event)))
            .subscribe(event => component.accept(event));
    }

    /** Взаимодействие с сервером при наступлении события. */
    abstract communicate(eventType: EventType): void;
}
