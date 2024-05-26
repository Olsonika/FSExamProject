import {Component, inject} from "@angular/core";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToCreateTask} from "../../models/clientWantsToCreateTask";

@Component({
  selector: 'app-new-task',
  templateUrl: 'createNewTask.component.html',
  //styleUrls: ['home.page.scss'],
})
export class NewTaskComponent {
  constructor(public fb: FormBuilder) {
  }

  form = this.fb.group({
    name: new FormControl("", [Validators.required]),
    description: new FormControl("", [Validators.required]),
    date: new FormControl("", [Validators.required]),
  })

  ws = inject(WebSocketClientService);

  createTask() {
    this.ws.socketConnection.sendDto(new ClientWantsToCreateTask({
      taskName: this.form.value.name!,
      description: this.form.value.description!,
      dueDate: new Date(this.form.value.date!)
    }))
  }
}
