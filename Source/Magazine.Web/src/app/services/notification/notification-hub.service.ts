import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationHubService {
  
  private connection: HubConnection;
  message = new Subject<string>();

  constructor() {
    this.createConnection();
    this.startConnection();
    this.registerOnServerEvents();
  }

  sendMessage() {
    this.connection.invoke('NotifyOnUpdate');
  }

  private createConnection() {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/notification`)
      .build();
  }

  private registerOnServerEvents(): void {
    this.connection.on('NotifyOnUpdate', message => {
      console.log('Received', message);
      this.message.next(message);
    });
  }

  private startConnection(): void {
    this.connection
      .start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(error => {
        console.log(error);
        console.log('Error while establishing connection, retrying...');
        setTimeout(function () { this.startConnection(); }, 5000);
      });
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }
}
