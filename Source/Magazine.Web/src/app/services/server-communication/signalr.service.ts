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

    subscriptions = new Map<string, Subject<any>>();
    methodName = 'сommunicateOnUpdate'; 

    constructor() {
        this.buildConnection();
        this.createSubscriptions();
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

    private createSubscriptions(): void {
        this.subscriptions.set(this.methodName, new Subject<string>());
    }

    private registerOnServerEvents(): void {
        this.connection.on(this.methodName, (...data: any[]) => this.subscriptions.get(this.methodName).next(data)); // Add ForEach
    }

    communicateOnUpdate(): void {
        this.connection.invoke(this.methodName);
    }
}
