import { Point2D } from "./Point2D";

export class Circle2D {
    origin: Point2D;
    radius: number;

    get X(): number { return this.origin.x; }
    get Y(): number { return this.origin.y; }

    constructor(origin: Point2D, radius: number) {
        this.origin = origin;
        this.radius = radius;
    }
}