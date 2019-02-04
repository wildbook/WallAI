import { RpcResponseType } from "Rpc/RpcResponseTypes";
import { RpcRequestType } from "Rpc/RpcRequestTypes";
import { RpcRequest } from "Rpc/RpcRequest";
import { RpcResponse } from "Rpc/RpcResponse";

export class RpcMessage {
    type: RpcResponseType | RpcRequestType;
    id: string;

    constructor(id: string, type: RpcResponseType | RpcRequestType) {
        this.id = id;
        this.type = type;
    }

    static isRequest(message: RpcMessage) {
        return message.type in RpcRequestType;
    }

    static isResponse(message: RpcMessage) {
        return message.type in RpcResponseType;
    }

    static toRequest(message: RpcMessage) {
        if (RpcMessage.isRequest(message)) {
            return message as RpcRequest;
        } else {
            throw "RpcMessage is not a request.";
        }
    }

    static toResponse(message: RpcMessage) {
        if (RpcMessage.isResponse(message)) {
            return message as RpcResponse;
        } else {
            throw "RpcMessage is not a response.";
        }
    }

    isRequest() { return RpcMessage.isRequest(this); }
    isResponse() { return RpcMessage.isResponse(this); }
    
    toRequest() { return RpcMessage.toRequest(this); }
    toResponse() { return RpcMessage.toResponse(this); }
}