import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { EntityServiceEvent } from "../event/entity.service.event";
import { map } from "rxjs/operators";
import { ArticleServiceAddEvent } from "../../events/article/article.service.add.event";
import { EntityService } from "./entity.service";
import { ArticleServiceUpdateEvent } from "src/app/events/article/article.service.update.event";
import { ArticleServiceDeleteEvent } from "src/app/events/article/article.service.delete.event";

/** Сервис взаимодействия с сервером. */
@Injectable({
    providedIn: 'root'
})
export abstract class ServerCommunicationService {

    protected onServerEvent = new Subject<EntityServiceEvent>();

    private entityServiceEventClassMapping = {
        'ArticleServiceAddEvent': ArticleServiceAddEvent,
        'ArticleServiceUpdateEvent': ArticleServiceUpdateEvent,
        'ArticleServiceDeleteEvent': ArticleServiceDeleteEvent,
    };

    /** Подписывание на получение событий сервера. */
    subscribe(service: EntityService): void {
        this.onServerEvent
            .pipe(map(event => Object.assign(new this.entityServiceEventClassMapping[event.className](), event)))
            .subscribe(event => service.accept(event));
    }

    /** Взаимодействие с сервером при наступлении события. */
    abstract send(event: EntityServiceEvent): void;
}
