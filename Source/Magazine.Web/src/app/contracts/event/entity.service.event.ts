import { EntityService } from "../service/entity.service";

/** Событие сервиса сущности. */
export abstract class EntityServiceEvent {

    /** Название класса сервиса сущности. */
    className: string;

    constructor(className: string) {
        this.className = className;
    }

    abstract visit(component: EntityService): void;
}
