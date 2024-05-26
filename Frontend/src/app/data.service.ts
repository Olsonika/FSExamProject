import { Injectable } from "@angular/core";
import {Project, Task} from "../models/models";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public projects: Project[] = [];
  public currentProject: Project = {};
  public tasks: Task[] = [];

  constructor() {}
}
