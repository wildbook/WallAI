using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;

namespace WallAI.Core.World
{
    public interface IWorld2DMethods
    {
        ITile2D GenerateTile(int seed, Point2D location);
    }
}