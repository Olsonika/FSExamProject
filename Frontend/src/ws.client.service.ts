import {Injectable} from "@angular/core";
import {WebsocketSuperclass} from "./models/WebSocketSuperclass";
import {environment} from "./environments/environment";
import {BaseDto} from "./models/baseDto";
import { ServerAuthenticatesUser } from "./models/serverAuthenticatesUser";
import { ServerSendsErrorMessageToClient } from "./models/serverSendErrorMessageToClient";
import {ServerLogsOutUser} from "./models/serverLogsOutUser";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";

@Injectable({providedIn: 'root'})
export class WebSocketClientService {

  public socketConnection: WebsocketSuperclass;

  constructor(public router: Router, public toast: ToastController) {
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
      //this.messageService.add( {life: 5000, severity: 'error', summary: '⚠', detail: 'The websocket API is currently not running (only the client app hosted on Firebase id running)'});
    }
  }

  ServerAuthenticatesUser(dto: ServerAuthenticatesUser) {
    this.router.navigate(['/dashboard']);
    localStorage.setItem("jwt", dto.jwt!);
  }

  ServerLogsOutUser(dto: ServerLogsOutUser) {
    this.router.navigate(['/home']);
    localStorage.clear();
  }

  ServerAuthenticatesUserFromJwt(dto: ServerAuthenticatesUser) {
    //this.messageService.add({life: 2000, summary: 'success', detail: 'Authentication successful!'});
  }

  async ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient) {




    console.log(dto.errorMessage);
    const toast = await this.toast.create({
      message: dto.errorMessage,
      color: "danger",
      duration: 2000,
      position: "top",
    });
    toast.present();
  }
}
