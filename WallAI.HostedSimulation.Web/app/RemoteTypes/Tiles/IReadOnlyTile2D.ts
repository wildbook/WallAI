import { IReadOnlyEntity2D } from "../Entities/IReadOnlyEntity2D";
import { IHasReadOnlyId } from "RemoteTypes/Interfaces/IHasReadOnlyId";

export interface IReadOnlyTile2D extends IHasReadOnlyId {
    entity: IReadOnlyEntity2D;
}