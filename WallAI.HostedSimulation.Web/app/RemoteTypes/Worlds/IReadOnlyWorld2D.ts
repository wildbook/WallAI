import { IReadOnlyWorld2DEntity } from "./Entities/IReadOnlyWorld2DEntity";
import { IReadOnlyWorld2DTile2D } from "./Tiles/IReadOnlyWorld2DTile";

export interface IReadOnlyWorld2D
{
    tiles: IReadOnlyWorld2DTile2D[];
    entities: IReadOnlyWorld2DEntity[];
}