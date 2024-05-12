import {Injectable} from "@angular/core";
import {Project} from "../models/models";

@Injectable({
  providedIn: 'root'
})

export class DataService {
  public projects?: Project [] = [];
}
