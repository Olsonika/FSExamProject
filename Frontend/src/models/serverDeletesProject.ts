import {BaseDto} from "./baseDto";
import {Project} from "./models";

export class ServerDeletesProject extends BaseDto<ServerDeletesProject>
{
  projectId?: number;
}
