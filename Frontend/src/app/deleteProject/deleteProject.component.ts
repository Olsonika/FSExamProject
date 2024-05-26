import {Component, inject} from "@angular/core";
import {ModalController, NavParams} from "@ionic/angular";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToDeleteProject} from "../../models/clientWantsToDeleteProject";



@Component({
  selector: 'app-delete-project',
  templateUrl: 'deleteProject.component.html',
})

export class DeleteProjectComponent {
  constructor(private navParams: NavParams, public modalController: ModalController) {
  }

  ws = inject(WebSocketClientService);
  projectToDelete: number = this.navParams.get('projectToDelete');

  deleteProject() {
    this.ws.socketConnection.sendDto(new ClientWantsToDeleteProject({
      projectId: this.projectToDelete
    }));
    this.modalController.dismiss();
  }

  dismiss() {
    this.modalController.dismiss();
  }
}
