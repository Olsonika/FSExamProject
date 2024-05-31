import {Component, inject, OnInit} from "@angular/core";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {WebSocketClientService} from "../../ws.client.service";
import {ClientWantsToCreateTask} from "../../models/clientWantsToCreateTask";
import {ModalController, NavParams} from "@ionic/angular";

@Component({
  selector: 'app-new-task',
  templateUrl: 'createNewTask.component.html',
  styleUrls: ['createNewTask.component.scss'],
})
export class NewTaskComponent  implements OnInit{
  dateTime: any;
  constructor(public fb: FormBuilder, public navParams: NavParams, public modalController: ModalController) {
  }

  ngOnInit() {
    setTimeout(() => {
      this.dateTime = new Date().toISOString();
    })
  }

  projectId: number = this.navParams.get('projectId');

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
      dueDate: new Date(this.form.value.date!),
      projectId: this.projectId!
    }))
    this.modalController.dismiss();
  }
}
