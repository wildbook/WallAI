using System;
using System.Runtime.Serialization;
using WallAI.Core.Entities;
using WallAI.Core.Interfaces;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.Worlds.Tiles
{
    [DataContract]
    public class World2DTile2D : IWorld2DTile2D
    {
        private readonly ITile2D _tile;

        [DataMember(Name = "id")]
        public Guid Id => _tile.Id;

        [DataMember(Name = "entity")]
        public IEntity Entity { get => _tile.Entity; set => _tile.Entity = value; }

        [DataMember(Name = "location")]
        public Point2D Location { get; set; }

        public IWorld2DTile2D WithLocationOffset(Point2D offset) => new World2DTile2D(this, Location + offset);

        IEntity ITile2D.Entity { get => Entity; set => Entity = value; }
        IReadOnlyEntity IReadOnlyTile2D.Entity => Entity;

        Guid IHasReadOnlyId.Id => Id;
        IHasReadOnlyLocation IHasReadOnlyLocation.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IWorld2DTile2D IWorld2DTile2D.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IReadOnlyWorld2DTile2D IReadOnlyWorld2DTile2D.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);

        public World2DTile2D(ITile2D tile, Point2D location)
        {
            _tile = tile;
            Location = location;
        }
    }
}
