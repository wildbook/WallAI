using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.World.Tiles
{
    public class World2DTile2D : IWorld2DTile2D
    {
        public IEntity Entity { get; set; }
        public Point2D Location { get; set; }
        public IWorld2D World { get; set; }

        public World2DTile2D(ITile2D tile, Point2D location, IWorld2D world)
        {
            Entity = tile.Entity;
            Location = location;
            World = world;
        }

        IReadOnlyWorld2D IReadOnlyWorld2DTile2D.World => World;
    }
}
