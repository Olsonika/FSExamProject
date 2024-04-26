import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {ClientWantsToSignIn} from "../../models/clientWantsToSignIn";
import {ClientWantsToRegister} from "../../models/clientWantsToRegister";

@Component({
  selector: 'app-login-register',
  templateUrl: 'login-register.page.html',
  styleUrls: ['login-register.page.scss'],
})
export class LoginRegisterPage {

  constructor(private readonly fb: FormBuilder) {
  }

  ws = inject(WebSocketClientService);

  form = this.fb.group({
    email: new FormControl("", [Validators.required, Validators.email]),
    password:  new FormControl("", [Validators.required, Validators.minLength(6)])
  })

  get email() { return this.form.controls.email; }
  get password() { return this.form.controls.password; }


  SignIn() {
    this.ws.socketConnection.sendDto(new ClientWantsToSignIn({
      password: this.form.controls.password.value!,
      email: this.form.controls.email.value!
    }))
  }

  Register() {
    this.ws.socketConnection.sendDto(new ClientWantsToRegister({
      password: this.form.controls.password.value!,
      email: this.form.controls.email.value!
    }))
  }
}
