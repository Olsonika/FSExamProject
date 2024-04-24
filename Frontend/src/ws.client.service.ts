import {Injectable} from "@angular/core";
import {WebsocketSuperclass} from "./models/WebSocketSuperclass";
import {MessageService} from "primeng/api";
import {environment} from "./environments/environment";
import {BaseDto} from "./models/baseDto";
import { ServerAuthenticatesUser } from "./models/serverAuthenticatesUser";
import { ServerSendsErrorMessageToClient } from "./models/serverSendErrorMessageToClient";

@Injectable({providedIn: 'root'})
export class WebSocketClientService {

  public socketConnection: WebsocketSuperclass;

  constructor(public messageService: MessageService) {
    this.socketConnection = new WebsocketSuperclass(environment.websocketBaseUrl);
    this.handleEventsEmittedByTheServer()
  }

  handleEventsEmittedByTheServer() {
    this.socketConnection.onmessage = (event) => {
      const data = JSON.parse(event.data) as BaseDto<any>;
      console.log("Received: " + JSON.stringify(data));
      //@ts-ignore
      this[data.eventType].call(this, data);
    }
    this.socketConnection.onerror = (err) => {
      this.messageService.add( {life: 5000, severity: 'error', summary: '⚠️', detail: 'The websocket API is currently not running (only the client app hosted on Firebase id running)'});
    }
  }

  ServerAuthenticatesUser(dto: ServerAuthenticatesUser) {
    this.messageService.add({life: 2000, detail: 'Authentication successful!'});
    localStorage.setItem("jwt", dto.jwt!);
  }

  ServerAuthenticatesUserFromJwt(dto: ServerAuthenticatesUser) {
    this.messageService.add({life: 2000, summary: 'success', detail: 'Authentication successful!'});
  }

  ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient) {
    this.messageService.add({life: 2000, severity: 'error', summary: '⚠️', detail: dto.errorMessage}); //todo implement with err handler
  }
}
