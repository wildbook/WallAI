using System;
using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public class Tile2D : ITile2D, IReadOnlyTile2D, IEquatable<Tile2D>
    {
        public IEntity Entity { get; set; }

        public Tile2D(IEntity entity = null)
        {

        }

        public bool Equals(Tile2D other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Entity, other.Entity);
        }

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