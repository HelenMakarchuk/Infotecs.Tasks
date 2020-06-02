import { Injectable } from '@angular/core';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ServerCommunicationService } from '../../contracts/service/server-communication.service';
import { EntityServiceEvent } from '../../contracts/event/entity.service.event';

/** Сервис взаимодействия с сервером с использованием библиотеки SignalR. */
@Injectable({
    providedIn: 'root'
})
export class SignalrService extends ServerCommunicationService {

    private connection: HubConnection;
    private hubName = "сommunication";
    private hubServerMethodName = "send";
    private hubClientMethodName = "send";

    constructor() {
        super();
        this.buildConnection();
        this.registerOnServerEvents();
        this.connection.start();
    }

    private buildConnection() {
        this.connection = new HubConnectionBuilder()
            .withUrl(`${environment.hubUrl}/${this.hubName}`)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();
    }

    /** Подписывание на получение события от сервера. */
    private registerOnServerEvents(): void {
        this.connection.on(`${this.hubClientMethodName}`, (event: EntityServiceEvent) => this.onServerEvent.next(event));
    }

    /** Вызов события сервера. */
    send(event: EntityServiceEvent): void {
        this.connection.invoke(`${this.hubServerMethodName}`, event);
    }
}
