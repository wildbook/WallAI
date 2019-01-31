using System;
using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.Worlds.Tiles
{
    public class World2DTile2D : IWorld2DTile2D
    {
        private readonly ITile2D _tile;
        public Guid Id => _tile.Id;

        public IEntity Entity { get => _tile.Entity; set => _tile.Entity = value; }
        public Point2D Location { get; set; }
        public IWorld2D World { get; set; }

        public IWorld2DTile2D WithLocationOffset(Point2D offset) => new World2DTile2D(this, Location + offset, World);

        Guid IReadOnlyTile2D.Id => Id;
        IReadOnlyWorld2D IReadOnlyWorld2DTile2D.World => World;
        IWorld2DTile2D IWorld2DTile2D.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IReadOnlyWorld2DTile2D IReadOnlyWorld2DTile2D.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);

        public World2DTile2D(ITile2D tile, Point2D location, IWorld2D world)
        {
            _tile = tile;
            Location = location;
            World = world;
        }
    }
}
