import { RpcResponseType } from "./RpcResponseTypes";

export class RpcResponse {
    id: string;
    type: RpcResponseType;

    constructor(id: string, type: RpcResponseType){
        this.id = id;
        this.type = type;
    }
}
