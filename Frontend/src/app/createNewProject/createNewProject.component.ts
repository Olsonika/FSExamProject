import {Component, inject} from "@angular/core";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToLogOut} from "../../models/clientWantsToLogOut";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {ClientWantsToCreateProject} from "../../models/clientWantsToCreateProject";

@Component({
  selector: 'app-new-project',
  templateUrl: 'createNewProject.component.html',
  //styleUrls: ['home.page.scss'],
})

export class NewProjectComponent {
  constructor(public fb:FormBuilder) {
  }
  form = this.fb.group({
    name: new FormControl("", [Validators.required]),
    description: new FormControl("", [Validators.required]),
  })

  ws = inject(WebSocketClientService);


  createProject() {
    this.ws.socketConnection.sendDto(new ClientWantsToCreateProject({
      name: this.form.value.name!,
      description: this.form.value.description!
      })
    );
  }
}
