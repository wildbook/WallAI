import { Renderer } from "Renderer";
import { RemoteSimulation } from "RemoteSimulation";
import { Logger } from "Logger";
import { IReadOnlyWorld2D } from "RemoteTypes/Worlds/IReadOnlyWorld2D";

export class RemoteSimulationRenderer {
    renderer: Renderer;
    simulation: RemoteSimulation;
    logger: Logger = Logger.create(this);
    
    constructor(renderer: Renderer, simulation: RemoteSimulation) {
        this.renderer = renderer;
        this.simulation = simulation;

        this.hookEvents();
    }
    
    hookEvents(): any {
        this.simulation.tick.on(x => this.tick(x.data));
    }

    tick(world: IReadOnlyWorld2D): any {
        var ctx = this.renderer.Context2D;

        ctx.fillStyle = "#333";
        world.tiles.forEach(tile => {
            ctx.fillRect(tile.location.x * 10, tile.location.y * 10, 10, 10);
        });

        
        ctx.fillStyle = "#0F0";
        world.entities.forEach(entity => {
            if (entity.stats.alive == true) {
                ctx.fillRect(entity.location.x * 10, entity.location.y * 10, 10, 10);
            }
        });
        
        ctx.fillStyle = "#F00";
        world.entities.forEach(entity => {
            if (entity.stats.alive === false) {
                ctx.fillRect(entity.location.x * 10, entity.location.y * 10, 10, 10);
            }
        });
    }
}