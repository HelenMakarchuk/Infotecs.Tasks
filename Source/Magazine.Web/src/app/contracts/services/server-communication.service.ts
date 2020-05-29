import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

/** Сервис взаимодействия с сервером. */
@Injectable({
    providedIn: 'root'
})
export abstract class ServerCommunicationService {

    /** Событие обновления статьи. */
    abstract onUpdate: Subject<string>;

    /** Взаимодействие при обновлении статьи. */
    abstract communicateOnUpdate(): void;
}
