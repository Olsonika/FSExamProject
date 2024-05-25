import {BaseDto} from "./baseDto";
import {Task} from "./models";

export class ServerSendsAllTasksForProject extends BaseDto<ServerSendsAllTasksForProject> {
  TaskList: Task[] = [];
}
