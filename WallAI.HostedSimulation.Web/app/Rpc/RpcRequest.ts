import { RpcRequestType } from "./RpcRequestTypes";

export class RpcRequest {
    id: string;
    type: RpcRequestType;

    constructor (id: string, type: RpcRequestType) {
        this.id = id;
        this.type = type;
    }
}
