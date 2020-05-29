import { Injectable } from "@angular/core";
import { ApplicationComponentEvent } from "../event/application.component.event";

/** Компонент приложения. */
@Injectable({
    providedIn: 'root'
})
export abstract class ApplicationComponent {
    accept(event: ApplicationComponentEvent): void {
        event.visit(this);
    }
}
