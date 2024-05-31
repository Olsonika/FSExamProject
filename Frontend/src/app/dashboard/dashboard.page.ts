import { Component, OnInit } from "@angular/core";
import { WebSocketClientService } from "../../ws.client.service";
import { ClientWantsToLogOut } from "../../models/clientWantsToLogOut";
import { ModalController } from "@ionic/angular";
import { NewProjectComponent } from "../createNewProject/createNewProject.component";
import { ClientWantsToGetProjects } from "../../models/clientWantsToGetProjects";
import { DataService } from "../data.service";
import { DeleteProjectComponent } from "../deleteProject/deleteProject.component";
import { Router } from "@angular/router";

@Component({
  selector: 'app-dashboard',
  templateUrl: 'dashboard.page.html',
  styleUrls: ['dashboard.page.scss'],
})
export class DashboardPage implements OnInit {
  isAuthenticated: boolean = false;

  constructor(
    public router: Router,
    public modalController: ModalController,
    public dataService: DataService,
    private ws: WebSocketClientService
  ) {
    this.getProjects();
  }

  ngOnInit() {
    this.ws.authenticationStatus$.subscribe(status => {
      this.isAuthenticated = status;
    });

    const jwt = localStorage.getItem('jwt');
    if (jwt) {
      this.ws.authenticateWithJwt(jwt);
    }
  }

  LogOut() {
    this.ws.logout();
  }

  async newProject() {
    const modal = await this.modalController.create({
      component: NewProjectComponent
    });
    modal.present();
  }

  getProjects() {
    this.ws.socketConnection.sendDto(new ClientWantsToGetProjects());
  }

  async deleteProjectConf(projectId: number | undefined) {
    const modal = await this.modalController.create({
      component: DeleteProjectComponent,
      componentProps: {
        projectToDelete: projectId
      },
    });
    modal.present();
  }

  seeDetails(projectId: number | undefined) {
    this.router.navigate(['project', projectId]);
  }
}
