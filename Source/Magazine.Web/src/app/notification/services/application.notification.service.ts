import { Injectable } from "@angular/core";

/** Сервис уведомления пользователя приложения. */
@Injectable({
    providedIn: 'root'
})
export class ApplicationNotificationService {

    /**
     * Уведомление пользователя приложения.
     * @param message Текст уведомления.
     */
    notify(message: string) {
        alert(message);
    }
}