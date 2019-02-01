using WallAI.Core.Interfaces;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.Worlds.Tiles
{
    public interface IReadOnlyWorld2DTile2D : IReadOnlyTile2D, IHasReadOnlyLocation
    {
        new IReadOnlyWorld2DTile2D WithLocationOffset(Point2D offset);
    }
}
