using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.World.Tiles
{
    public interface IWorld2DTile2D : IReadOnlyWorld2DTile2D, ITile2D
    {
        new Point2D Location { get; set; }
        new IWorld2D World { get; set; }
    }
}
