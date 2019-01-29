using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.World.Tiles
{
    public interface IReadOnlyWorld2DTile2D : IReadOnlyTile2D
    {
        Point2D Location { get; }
        IReadOnlyWorld2D World { get; }
    }
}
