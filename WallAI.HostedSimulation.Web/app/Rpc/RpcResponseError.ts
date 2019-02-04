import { RpcResponse } from "./RpcResponse";
import { RpcResponseType } from "./RpcResponseTypes";

export class RpcResponseError extends RpcResponse {
    message: string;

    constructor(id: string, message: string) {
        super(id, RpcResponseType.Error);
        this.message = message;
    }
}