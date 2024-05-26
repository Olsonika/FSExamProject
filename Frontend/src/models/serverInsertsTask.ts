import {BaseDto} from "./baseDto";
import {Task} from "./models";

export class ServerInsertsTask extends BaseDto<ServerInsertsTask>
{
  task?: Task;
}
