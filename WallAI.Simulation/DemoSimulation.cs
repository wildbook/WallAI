using System;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Helpers;
using WallAI.Core.Helpers.Extensions;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds;
using WallAI.Simulation.Ai;

namespace WallAI.Simulation
{
    public class DemoSimulation : Simulation
    {
        public DemoSimulation(Point2D size, int seed = 0) : base(new World2DMethods(), seed, size.X, size.Y) { }

        public class World2DMethods : IWorld2DMethods
        {
            private static readonly (int Weight, Func<Guid, IStats, IStats, IEntity> Factory)[] AiFactories =
            {
                (100, (id, s, m) => null),
                //(1,  (id, s, m) => new Entity<ObedientAi>(id, s, m)),
                (5,  (id, s, m) => new Entity<Testing>(id, s, m)),
            };

            public ITile2D GenerateTile(int seed, Point2D location)
            {
                var random = new LoggingRandom(HashCode.Combine(seed, location));
                
                var stats = new Stats
                {
                    Alive = true,
                    Energy = (uint)random.Next(0, 100),
                    Opaque = true,
                    VisionRadius = (byte)random.Next(1, 7),
                };

                var entityId = random.NextGuid();
                var tileId = random.NextGuid();
                var ai = random.NextByWeight(AiFactories, x => x.Weight).Factory(entityId, stats, stats);
                return new Tile2D(tileId, ai);
            }
        }
    }
}
