import {BaseDto} from "./baseDto";
import {Project} from "./models";

export class ServerSendsSpecificProject extends BaseDto<ServerSendsSpecificProject> {
  Project?: Project;
}
