import { Injectable } from '@angular/core';
import { HubConnection, LogLevel, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationHubService {
  
  private connection: HubConnection;
  message = new Subject<string>();

  constructor() {
    this.buildConnection();
    this.registerOnServerEvents();
    this.connection.start();
  }

  sendMessage() {
    this.connection.invoke('NotifyOnUpdate');
  }

  private buildConnection() {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/notification`)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();
  }

  private registerOnServerEvents(): void {
    this.connection.on('NotifyOnUpdate', message => this.message.next(message));
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }
}
