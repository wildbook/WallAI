using WallAI.Core.Tiles;
using WallAI.Core.World;

namespace WallAI.Simulation
{
    public class DemoSimulation : Simulation
    {
        public DemoSimulation() : base(new World2DMethods(), 0, 25, 25) { }
        
        internal class World2DMethods : IWorld2DMethods
        {
            public ITile2D GenerateTile(int seed, int x, int y) => new Tile2D();
        }
    }
}
