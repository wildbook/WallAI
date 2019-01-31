using System;
using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public class Tile2D : ITile2D, IEquatable<Tile2D>
    {
        public Guid Id => Entity.Id;
        public IEntity Entity { get; set; }

        public Tile2D(IEntity entity = null) => Entity = entity;

        public bool Equals(Tile2D other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Tile2D) obj);
        }

        public override int GetHashCode() => (Entity != null ? Entity.GetHashCode() : 0);
    }
}