import { IReadOnlyStats } from "./Stats/IReadOnlyStats";
import { IHasReadOnlyLocation } from "RemoteTypes/Interfaces/IHasReadOnlyLocation";
import { IHasReadOnlyId } from "RemoteTypes/Interfaces/IHasReadOnlyId";

export interface IReadOnlyEntity2D extends IHasReadOnlyLocation, IHasReadOnlyId{
    ai: string;
    stats: IReadOnlyStats;
    maxStats: IReadOnlyStats;
}