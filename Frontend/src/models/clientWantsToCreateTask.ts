import {BaseDto} from "./baseDto";

export class ClientWantsToCreateTask extends BaseDto<ClientWantsToCreateTask> {
taskName?: string;
description?: string;
dueDate?: Date;
projectId?: string;
}
