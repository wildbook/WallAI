import { RpcRequest } from "./RpcRequest";
import { RpcRequestType } from "./RpcRequestTypes";

export class RpcRequestEvent extends RpcRequest {
    event: string;

    constructor(id: string, event: string) {
        super(id, RpcRequestType.Subscribe);
        this.event = event;
    }
}