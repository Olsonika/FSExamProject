import {BaseDto} from "./baseDto";

export class ClientWantsToCreateProject extends BaseDto<ClientWantsToCreateProject>{
  projectname?: string;
  description?: string;
}
