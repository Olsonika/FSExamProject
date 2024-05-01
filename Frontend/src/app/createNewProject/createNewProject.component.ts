import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";

@Component({
  selector: 'app-new-project',
  templateUrl: 'createNewProject.component.html',
  //styleUrls: ['home.page.scss'],
})

export class NewProjectComponent {
  constructor() {
  }
  ws = inject(WebSocketClientService);


}
