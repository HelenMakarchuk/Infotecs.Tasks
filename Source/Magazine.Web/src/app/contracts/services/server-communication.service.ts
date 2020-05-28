import { Injectable } from "@angular/core";

/** Сервис взаимодействия с сервером. */
@Injectable({
    providedIn: 'root'
})
export abstract class ServerCommunicationService {
    /**
     * Взаимодействие при обновлении статьи.
     */
    abstract communicateOnUpdate(): void;
}
