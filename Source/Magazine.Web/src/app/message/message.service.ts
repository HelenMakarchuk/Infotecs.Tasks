import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  broadcastMessageReceived = new EventEmitter<string>();
  connectionEstablished = new EventEmitter<boolean>();

  private connectionIsEstablished = false;
  private connection: HubConnection;

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  sendMessage(message: string) {
    debugger;
    this.connection.invoke('send', message);
  }

  private createConnection() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/message')
      .build();
  }

  private registerOnServerEvents(): void {
    debugger;
    // после того как сообщение обработано на сервере
    this.connection.on('broadcastMessage', message => {
      debugger;
      console.log("Notify all clients");

      // уведомление клиентов
      this.broadcastMessageReceived.emit(message);
    });
  }

  private startConnection(): void {
    debugger;

    this.connection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(error => {
        console.log(error);
        console.log('Error while establishing connection, retrying...');
        setTimeout(function () { this.startConnection(); }, 5000);
      });
  }
}    
