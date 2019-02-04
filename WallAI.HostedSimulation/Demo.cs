using System;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Helpers;
using WallAI.Core.Helpers.Extensions;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds;
using WallAI.HostedSimulation.Ai;

namespace WallAI.HostedSimulation
{
    public static class Demo
    {
        public class World2DMethods : IWorld2DMethods
        {
            public static World2DMethods Instance { get; } = new World2DMethods();

            private static readonly (int Weight, Func<Guid, IStats, IStats, IEntity> Factory)[] AiFactories =
            {
                (100, (id, s, m) => null),
                (5, (id, s, m) => new Entity<Testing>(id, s, m)),
            };

            public ITile2D GenerateTile(int seed, Point2D location)
            {
                // We're allowed to use GetHashCode() solely because we implemented it on our own and we're 100% sure it's deterministic.
                var random = new LoggingRandom(seed ^ location.GetHashCode());

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
