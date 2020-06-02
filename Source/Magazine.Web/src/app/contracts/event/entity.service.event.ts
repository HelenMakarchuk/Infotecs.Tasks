import { EntityService } from "../service/entity.service";

/** Событие сервиса сущности. */
export abstract class EntityServiceEvent {

    $type = "Magazine.API.Services.ClientCommunicationService.Events.ArticleServiceUpdateEvent, Infotecs.Magazine.API";

    /** Название класса сервиса сущности. */
    className: string;

    constructor(className: string) {
        this.className = className;
    }

    abstract visit(component: EntityService): void;
}
