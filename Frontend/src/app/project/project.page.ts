import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToGetProjects} from "../../models/clientWantsToGetProjects";
import {ClientWantsToGetProjectById} from "../../models/clientWantsToGetProjectById";
import {ClientWantsToCreateProject} from "../../models/clientWantsToCreateProject";
import {ActivatedRoute} from "@angular/router";
import {DataService} from "../data.service";

@Component({
  selector: 'app-project',
  templateUrl: 'project.page.html',
  //styleUrls: ['login-register.page.scss'],
})

export class ProjectPage {
  constructor(private activatedRoute: ActivatedRoute, public dataService: DataService) {
    this.getProject();
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
}

