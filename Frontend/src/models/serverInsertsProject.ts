import {BaseDto} from "./baseDto";
import {Project} from "./models";

export class ServerInsertsProject extends BaseDto<ServerInsertsProject>
{
  project?: Project;
}
