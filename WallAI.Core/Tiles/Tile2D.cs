using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public class Tile2D : ITile2D, IReadOnlyTile2D
    {
        public IEntity Entity { get; set; }

        public Tile2D(IEntity entity = null)
        {

        }
    }
}