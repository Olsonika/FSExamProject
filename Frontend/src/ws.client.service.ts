import {Injectable} from "@angular/core";
import {WebsocketSuperclass} from "./models/WebSocketSuperclass";
import {environment} from "./environments/environment";
import {BaseDto} from "./models/baseDto";
import { ServerAuthenticatesUser } from "./models/serverAuthenticatesUser";
import { ServerSendsErrorMessageToClient } from "./models/serverSendErrorMessageToClient";
import {ServerLogsOutUser} from "./models/serverLogsOutUser";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {ServerInsertsProject} from "./models/serverInsertsProject";
import { ServerSendsProjects } from "./models/serverSendsProjects";
import {DataService} from "./app/data.service";
import {ServerDeletesProject} from "./models/serverDeletesProject";
import {ServerSendsSpecificProject} from "./models/serverSendSpecificProject";
import { ServerSendsAllTasksForProject } from "./models/serverSendsAllTasksForProject";

@Injectable({providedIn: 'root'})
export class WebSocketClientService {

  public socketConnection: WebsocketSuperclass;

  constructor(public router: Router, public toast: ToastController, public dataService: DataService) {
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
    this.socketConnection.onerror = async (err) => {
      const toast = await this.toast.create({
        message: err.message,
        color: "danger",
        duration: 2000,
        position: "top",
      });
      toast.present();
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

  async ServerInsertsProject(dto: ServerInsertsProject) {
      this.dataService.projects.push(dto.project!);

    const toast = await this.toast.create({
      message: "Project created!",
      color: "success",
      duration: 2000,
      position: "top",
    });
    toast.present();
  }

  async ServerSendsErrorMessageToClient(dto: ServerSendsErrorMessageToClient) {
    /*console.log(dto.errorMessage);
    const toast = await this.toast.create({
      message: dto.errorMessage,
      color: "danger",
      duration: 2000,
      position: "top",
    });
    toast.present();*/
  }

  ServerSendsProjects(dto: ServerSendsProjects) {
    this.dataService.projects = dto.ProjectsList || [];
  }

  ServerDeletesProject(dto: ServerDeletesProject) {
    this.dataService.projects.filter(a => a.ProjectId!=dto.projectId!)
  }

  ServerSendsSpecificProject(dto: ServerSendsSpecificProject) {
    this.dataService.currentProject = dto.Project!;
  }

  ServerSendsAllTasksForProject(dto: ServerSendsAllTasksForProject) {
    this.dataService.tasks = dto.TaskList || [];
    console.log(dto.TaskList);
  }
}
