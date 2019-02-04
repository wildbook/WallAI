import { RpcRequest } from "./RpcRequest";
import { RpcResponse } from "./RpcResponse";
import { TypedEvent } from "../TypedEvent";
import { RpcRequestEvent } from "./RpcRequestEvent";
import { RpcResponseType } from "./RpcResponseTypes";
import { RpcRequestType } from "./RpcRequestTypes";
import { RpcRequestCall } from "./RpcRequestCall";
import { RpcResponseError } from "./RpcResponseError";
import { RpcResponseEvent } from "./RpcResponseEvent";
import { RpcMessage } from "./RpcMessage";
import { Logger } from "../Logger";

export class Rpc {
    private ws: WebSocket;
    private logger: Logger = Logger.create(this);
    
    private commands: {[command: string]: (params: string[]) => RpcResponse};
    private waitingForResponse: {[id: string]: (response: RpcResponse) => void};
    private subscribedTo: {[id: string]: (response: RpcResponseEvent<any>) => void};

    private uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    onopen: TypedEvent<undefined> = new TypedEvent();
    onclose: TypedEvent<undefined> = new TypedEvent();

    async subscribe(event: string, handler: (response: RpcResponseEvent<any>) => void) : Promise<boolean> {
        var id = this.uuidv4();
        this.ws.send(JSON.stringify(new RpcRequestEvent(id, event)));

        return new Promise<boolean>(resolve => {
            this.waitingForResponse[id] = response => {
                if (response.type == RpcResponseType.Subscribed) {
                    this.subscribedTo[id] = handler;
                    resolve(true);
                } else {
                    resolve(false);
                }
            };
        });
    }

    registerCommand(command: string, handler: (params: string[]) => RpcResponse) {
        this.commands[command] = handler;
    }

    constructor (ws: WebSocket) {
        var _this = this;
        this.ws = ws;
        this.commands = { };
        this.waitingForResponse = { };
        this.subscribedTo = { };

        this.ws.onopen    = function (ev: Event)  { _this.onOpen(this, ev); };
        this.ws.onclose   = function (ev: CloseEvent)  { _this.onClose(this, ev); };
        this.ws.onerror   = function (ev: Event)  { _this.onError(this, ev); };
        this.ws.onmessage = function (ev: MessageEvent)  { _this.onMessage(this, ev); };
    }

    private onClose(ws: WebSocket, ev: CloseEvent) {
        this.onclose.emit(null);
    }
    
    private onOpen(ws: WebSocket, ev: Event) {
        this.onopen.emit(null);
    }

    private onError(ws: WebSocket, ev: Event) { 
        this.logger.error("Error", ev); 
    }

    private onMessage(ws: WebSocket, ev: MessageEvent) {
        var raw: RpcMessage | RpcMessage[] = JSON.parse(ev.data);

        var requests: RpcRequest[] = [];
        var responses: RpcResponse[] = [];

        if (!Array.isArray(raw)) {
            raw = [raw];
        }

        raw.forEach(element => {
            if (RpcMessage.isRequest(element)) {
                requests.push(RpcMessage.toRequest(element));
            }
            else if (RpcMessage.isResponse(element)) {
                responses.push(RpcMessage.toResponse(element));
            } else {
                this.logger.error("Failed to identify message type", element);
            }
        });

        requests.forEach(element => {
            ws.send(JSON.stringify(this.handleRpcRequest(element)));
        });

        responses.forEach(element => {
            this.handleRpcResponse(element);
        });
    }

    private handleRpcResponse(res: RpcResponse) {
        if (res.id in this.waitingForResponse) {
            var handler = this.waitingForResponse[res.id];
            delete this.waitingForResponse[res.id];
            handler(res);
        } else if (res.id in this.subscribedTo) {
            this.subscribedTo[res.id](res as RpcResponseEvent<any>);
        }
    }

    private handleRpcRequest(req: RpcRequest): RpcResponse {
        var isNotification: boolean = req.id == null;
        var result: RpcResponse;

        switch (req.type) {
            case RpcRequestType.Call:
                result = this.handleRpcRequestCall(req as RpcRequestCall);
                break;
            case RpcRequestType.Subscribe:
                result = this.handleRpcRequestSubscribe(req as RpcRequestEvent);
                break;
            case RpcRequestType.Unsubscribe:
                result = this.handleRpcRequestUnsubscribe(req as RpcRequestEvent);
                break;
        }

        return isNotification ? null : result;
    }

    private handleRpcRequestCall(req: RpcRequestCall) : RpcResponse {
        if (this.commands.hasOwnProperty(req.method)) {
            var handler = this.commands[req.method];
            var response = handler(req.params);
            return response;
        } else {
            return new RpcResponseError(req.id, "Method does not exist.");
        }
    }

    private handleRpcRequestSubscribe(req: RpcRequestEvent) : RpcResponse {
        return new RpcResponseError(req.id, "Event does not exist.");
    }
    
    private handleRpcRequestUnsubscribe(req: RpcRequestEvent) : RpcResponse {
        return new RpcResponseError(req.id, "Event does not exist.");
    }
}
