import {BaseDto} from "./baseDto";

export class ClientWantsToCreateProject extends BaseDto<ClientWantsToCreateProject>{
  name?: string;
  description?: string;
}
