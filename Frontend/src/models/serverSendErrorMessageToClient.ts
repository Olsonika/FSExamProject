import {BaseDto} from "./baseDto";

export class ServerSendsErrorMessageToClient extends BaseDto<ServerSendsErrorMessageToClient> {
    ErrorMessage?: string;
    ReceivedMessage?: string;
}
