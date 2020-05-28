import { Injectable } from '@angular/core';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ServerCommunicationService } from '../../contracts/services/server-communication.service';

/** Сервис взаимодействия с сервером с использованием библиотеки SignalR. */
@Injectable({
    providedIn: 'root'
})
export class SignalrService implements ServerCommunicationService {

    private connection: HubConnection;
    message = new Subject<string>();

    constructor() {
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
        this.connection.on('сommunicateOnUpdate', message => this.message.next(message));
    }

    communicateOnUpdate(): void {
        this.connection.invoke('сommunicateOnUpdate');
    }
}
