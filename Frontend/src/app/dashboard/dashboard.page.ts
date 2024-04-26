import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToSignIn} from "../../models/clientWantsToSignIn";
import {ClientWantsToLogOut} from "../../models/clientWantsToLogOut";

@Component({
  selector: 'app-dashboard',
  templateUrl: 'dashboard.page.html',
  //styleUrls: ['home.page.scss'],
})
export class DashboardPage {
  constructor() {}
  ws = inject(WebSocketClientService);

  LogOut() {
    this.ws.socketConnection.sendDto(new ClientWantsToLogOut());
  }
}
