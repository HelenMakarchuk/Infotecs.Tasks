import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

/** Сервис взаимодействия с сервером. */
@Injectable({
    providedIn: 'root'
})
export abstract class ServerCommunicationService {

    methodName: string; // Add Enum

    /** Словарь подписок на события. */
    subscriptions: Map<string, Subject<any>>;

    /** Взаимодействие при обновлении статьи. */
    abstract communicateOnUpdate(): void;
}
