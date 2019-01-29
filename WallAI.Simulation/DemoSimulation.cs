using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World;

namespace WallAI.Simulation
{
    public class DemoSimulation : Simulation
    {
        public DemoSimulation(Point2D size, int seed = 0) : base(new World2DMethods(), seed, size.X, size.Y) { }
        
        internal class World2DMethods : IWorld2DMethods
        {
            public ITile2D GenerateTile(int seed, int x, int y) => new Tile2D();
        }
    }
}
