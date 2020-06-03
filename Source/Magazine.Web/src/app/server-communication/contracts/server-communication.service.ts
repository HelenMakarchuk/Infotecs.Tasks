import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { EntityServiceEvent } from "./entity.service.event";
import { map } from "rxjs/operators";
import { EntityService } from "../../contracts/service/entity.service";
import { ArticleServiceAddEvent } from "../events/article/article.service.add.event";
import { ArticleServiceUpdateEvent } from "../events/article/article.service.update.event";
import { ArticleServiceDeleteEvent } from "../events/article/article.service.delete.event";
import { CommentServiceAddEvent } from "../events/comment/comment.service.add.event";

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
        'CommentServiceAddEvent': CommentServiceAddEvent,
    };

    /** Подписывание на получение событий сервера. */
    subscribe(service: EntityService): void {
        this.onServerEvent
            .pipe(map(event => {
                let concreteEvent = new this.entityServiceEventClassMapping[event.className]();
                Object.assign(concreteEvent, event);
                
                return concreteEvent;
            }))
            .subscribe(event => service.accept(event));
    }

    /** Взаимодействие с сервером при наступлении события. */
    abstract send(event: EntityServiceEvent): void;
}
