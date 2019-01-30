using System;
using System.Threading;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Helpers.Extensions;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World;
using WallAI.Simulation.Ai;

namespace WallAI.Simulation
{
    public class DemoSimulation : Simulation
    {
        public DemoSimulation(Point2D size, int seed = 0) : base(new World2DMethods(), seed, size.X, size.Y) { }

        internal class World2DMethods : IWorld2DMethods
        {
            private static readonly (int Weight, Func<IStats, IStats, IEntity> Factory)[] AiFactories =
            {
                (100, (s, m) => null),
                (1,  (s, m) => new Entity<ObedientAi>(s, m)),
                (5,  (s, m) => new Entity<Testing>(s, m)),
            };

            public ITile2D GenerateTile(int seed, Point2D location)
            {
                var random = new Random(HashCode.Combine(seed, location));
                
                var stats = new Stats
                {
                    Alive = true,
                    Energy = (uint)random.Next(0, 100),
                    Height = 1,
                    Opaque = true,
                    VisionRadius = (byte)random.Next(0, 7),
                };

                var ai = random.NextByWeight(AiFactories, x => x.Weight).Factory(stats, stats);
                return new Tile2D(ai);
            }
        }
    }
}
