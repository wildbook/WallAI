import { RpcResponse } from "./RpcResponse";
import { RpcResponseType } from "./RpcResponseTypes";

export class RpcResponseEvent<T> extends RpcResponse {
    data: T;

    constructor(id: string, data: T) {
        super(id, RpcResponseType.Event)
        this.data = data;
    }
}