using System;
using System.Runtime.Serialization;
using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    [DataContract]
    public class Tile2D : ITile2D, IEquatable<Tile2D>
    {
        [DataMember] public Guid Id { get; }
        [DataMember] public IEntity Entity { get; set; }

        IEntity ITile2D.Entity { get => Entity; set => Entity = value; }
        IReadOnlyEntity IReadOnlyTile2D.Entity => Entity;

        public Tile2D(Guid id, IEntity entity = null)
        {
            Id = id;
            Entity = entity;
        }

        public bool Equals(Tile2D other) => Id == other.Id;
        public override int GetHashCode() => Id.GetHashCode();
    }
}