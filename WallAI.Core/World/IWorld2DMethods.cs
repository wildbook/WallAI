using WallAI.Core.Tiles;

namespace WallAI.Core.World
{
    public interface IWorld2DMethods
    {
        ITile2D GenerateTile(int seed, int x, int y);
    }
}