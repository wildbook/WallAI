import { RpcRequest } from "./RpcRequest";
import { RpcRequestType } from "./RpcRequestTypes";

export class RpcRequestCall extends RpcRequest {
    method: string;
    params: string[];

    constructor(id: string, method: string, params: string[]) {
        super(id, RpcRequestType.Call);
        this.method = method;
        this.params = params;
    }
}