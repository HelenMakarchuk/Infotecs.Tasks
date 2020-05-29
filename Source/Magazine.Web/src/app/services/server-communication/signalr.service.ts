import { Injectable } from '@angular/core';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ServerCommunicationService } from '../../contracts/service/server-communication.service';
import { ApplicationComponentEvent } from '../../contracts/event/application.component.event';

/** Сервис взаимодействия с сервером с использованием библиотеки SignalR. */
@Injectable({
    providedIn: 'root'
})
export class SignalrService extends ServerCommunicationService {

    private connection: HubConnection;

    constructor() {
        super();
        this.buildConnection();
        this.registerOnServerEvents();
        this.connection.start();
    }

    private buildConnection() {
        this.connection = new HubConnectionBuilder()
            .withUrl(`${environment.hubUrl}/сommunication`)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();
    }

    private registerOnServerEvents(): void {
        this.connection.on(EventType.Update, (event: ApplicationComponentEvent) => this.serverEvent.next(event));
    }

    communicate(eventType: EventType): void {
        this.connection.invoke(eventType);
    }
}
