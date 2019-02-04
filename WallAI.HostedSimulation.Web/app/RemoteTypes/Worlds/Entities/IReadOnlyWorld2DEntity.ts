import { IReadOnlyWorld2D } from "../IReadOnlyWorld2D";
import { IReadOnlyEntity2D } from "RemoteTypes/Entities/IReadOnlyEntity2D";
import { IHasReadOnlyLocation } from "RemoteTypes/Interfaces/IHasReadOnlyLocation";

export interface IReadOnlyWorld2DEntity extends IReadOnlyEntity2D, IHasReadOnlyLocation {
    
}