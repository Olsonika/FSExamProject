import {Component, inject} from '@angular/core';
import {FormControl} from "@angular/forms";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToSignIn} from "../../models/clientWantsToSignIn";
import {ClientWantsToRegister} from "../../models/clientWantsToRegister";

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  constructor() {}
  ws = inject(WebSocketClientService);

  email = new FormControl("");
  password = new FormControl("");

  SignIn() {
    this.ws.socketConnection.sendDto(new ClientWantsToSignIn({password: this.password.value!, email: this.email.value!}))
  }

  Register() {
    this.ws.socketConnection.sendDto(new ClientWantsToRegister({password: this.password.value!, email: this.email.value!}))
  }
}
