import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToSignIn} from "../../models/clientWantsToSignIn";
import {ClientWantsToLogOut} from "../../models/clientWantsToLogOut";
import {ModalController} from "@ionic/angular";
import {NewProjectComponent} from "../createNewProject/createNewProject.component";

@Component({
  selector: 'app-dashboard',
  templateUrl: 'dashboard.page.html',
  //styleUrls: ['home.page.scss'],
})
export class DashboardPage {
  constructor(public modalController: ModalController) {}
  ws = inject(WebSocketClientService);

  LogOut() {
    this.ws.socketConnection.sendDto(new ClientWantsToLogOut());
  }

  async newProject() {
    const modal = await this.modalController.create({
      component: NewProjectComponent
      })
    modal.present();
  }
}
