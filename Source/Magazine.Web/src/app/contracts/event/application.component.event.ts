import { ApplicationComponent } from "../component/application.component";

/** Событие компонента приложения. */
export abstract class ApplicationComponentEvent {

    /** Название класса. */
    className: string;

    abstract visit(component: ApplicationComponent): void;
}
