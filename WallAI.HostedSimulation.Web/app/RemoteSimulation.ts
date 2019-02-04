import { Logger } from "./Logger";
import { TypedEvent } from "./TypedEvent";
import { Rpc } from "./Rpc/Rpc";
import { IReadOnlyWorld2D } from "RemoteTypes/Worlds/IReadOnlyWorld2D";
import { RpcResponseEvent } from "Rpc/RpcResponseEvent";

export class RemoteSimulation {
    private rpc: Rpc;
    private logger: Logger = Logger.create(this);

    tick = new TypedEvent<RpcResponseEvent<IReadOnlyWorld2D>>();

    constructor(url: string) {
        this.rpc = new Rpc(new WebSocket(url));

        this.rpc.onopen.on(() => {
            this.rpc.subscribe(Events.Tick, x => this.onTick(x) );
        });
    }

    onTick(tick: any) { this.tick.emit(tick); }
}

enum Events {
    Tick = "tick",
}