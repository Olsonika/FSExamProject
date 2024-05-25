import {BaseDto} from "./baseDto";
import {Project} from "./models";

export class ServerSendsProjects extends BaseDto<ServerSendsProjects> {
  ProjectsList: Project[] = [];
}
