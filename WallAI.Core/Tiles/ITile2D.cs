using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public interface ITile2D : IReadOnlyTile2D
    {
        new IEntity Entity { get; set; }
    }
}