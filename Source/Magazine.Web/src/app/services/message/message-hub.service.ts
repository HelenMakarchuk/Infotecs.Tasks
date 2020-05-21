import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageHubService {
  
  private connection: HubConnection;
  message = new Subject<string>();

  constructor() {
    this.createConnection();
    this.startConnection();
    this.registerOnServerEvents();
  }

  sendMessage(message: string) {
    this.connection.invoke('send', message);
  }

  private createConnection() {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/message`)
      .build();
  }

  private registerOnServerEvents(): void {
    this.connection.on('broadcastMessage', message => {
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
