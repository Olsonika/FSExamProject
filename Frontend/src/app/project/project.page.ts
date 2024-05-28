import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToGetProjects} from "../../models/clientWantsToGetProjects";
import {ClientWantsToGetProjectById} from "../../models/clientWantsToGetProjectById";
import {ClientWantsToCreateProject} from "../../models/clientWantsToCreateProject";
import {ActivatedRoute} from "@angular/router";
import {DataService} from "../data.service";
import {ClientWantsAllTasksForProject} from "../../models/clientWantsAllTasksForProject";
import {DeleteProjectComponent} from "../deleteProject/deleteProject.component";
import {ModalController} from "@ionic/angular";
import {NewTaskComponent} from "../createNewTask/createNewTask.component";

@Component({
  selector: 'app-project',
  templateUrl: 'project.page.html'
})

export class ProjectPage {
  constructor(private activatedRoute: ActivatedRoute, public dataService: DataService, public modalController: ModalController) {
    this.getProject();
    this.getTasks();
  }

  ws = inject(WebSocketClientService);

  getProject() {
    this.activatedRoute.params.subscribe(async (params) => {
      const projectId = +params['projectId'];
      if (projectId) {
        this.ws.socketConnection.sendDto(new ClientWantsToGetProjectById({
          projectId: projectId
        }));
      } else {

      }
    });
  }

  getTasks() {
    this.activatedRoute.params.subscribe(async (params) => {
      const projectId = +params['projectId'];
      if (projectId) {
        this.ws.socketConnection.sendDto(new ClientWantsAllTasksForProject({
          projectId: projectId,
        }))
      }
    })
  }

  async newTask() {
    this.activatedRoute.params.subscribe(async (params) => {
      const projectId = +params['projectId'];
      if (projectId) {
        const modal = await this.modalController.create({
          component: NewTaskComponent,
          componentProps: {
            projectId: projectId
          },
        });
        modal.present();
      }
    })
  }
}

