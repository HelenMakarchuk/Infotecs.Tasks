import { Injectable } from "@angular/core";
import { EntityServiceEvent } from "../../server-communication/contracts/entity.service.event";

/** Сервис сущности. */
@Injectable({
    providedIn: 'root'
})
export abstract class EntityService {
    accept(event: EntityServiceEvent): void {
        event.visit(this);
    }
}
