using System;
using System.Collections.Generic;
using WallAI.Core.Ai.Vision;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World;
using WallAI.Core.World.Ai;
using WallAI.Core.World.Entities;

namespace WallAI.Core.Ai
{
    public class AiCore : IAiCore
    {
        private readonly AiWorld2D _aiWorld2D;
        private IWorld2DEntity Entity { get; }
        public IReadOnlyStats Stats => Entity.Stats;
        public IReadOnlyStats MaxStats => Entity.MaxStats;
        public Circle2D Vision => new Circle2D(Entity.Location, Stats.VisionRadius);
        public Point2D Location => Entity.Location;
        public int Tick { get; }

        public AiCore(AiWorld2D aiWorld2D, IWorld2DEntity entity, int tick)
        {
            _aiWorld2D = aiWorld2D;
            Entity = entity;
            Tick = tick;
        }

        public IWorld2D LockRect(Point2D center, Rectangle2D rect) => _aiWorld2D.LockRect(center, rect);

        public Random GetRandom() => new Random(_aiWorld2D.Seed + Tick);

        public void Move(Direction direction)
        {
            var destinationPoint = Location;

            if (direction == Direction.West)
                destinationPoint += new Point2D(-1, 0);
            else if (direction == Direction.East)
                destinationPoint += new Point2D(1, 0);
            else if (direction == Direction.North)
                destinationPoint += new Point2D(0, -1);
            else if (direction == Direction.South)
                destinationPoint += new Point2D(0, 1);

            var lockRect = new Rectangle2D(Location, destinationPoint);

            using (var map = LockRect(Point2D.Zero, lockRect))
            {
                if (map[destinationPoint].Entity != null)
                    throw new DestinationContainsObjectException();

                var obj = map[Location];
                map[Location] = new Tile2D();
                map[destinationPoint] = obj;

                DeltaEnergy(-1);
            }
        }

        public void Kill() => Entity.Stats.Alive = false;

        public IReadOnlyWorld2D GetVisibleWorld()
        {
            var visionRadius = Stats.VisionRadius;

            var lockRect = new Rectangle2D(Location - new Point2D(visionRadius, visionRadius), visionRadius * 2, visionRadius * 2);
            using (var map = LockRect(Point2D.Zero, lockRect))
            {
                var visiblePoints = new HashSet<Point2D>();

                ShadowCaster.ComputeFieldOfViewWithShadowCasting(
                    Entity.Location.X,
                    Entity.Location.Y,
                    visionRadius - 1,
                    (x, y) =>
                    {
                        var point = new Point2D(x, y);
                        if (point.Equals(Entity.Location))
                            return false;

                        var tile = map[point];

                        if (tile.Entity == null)
                            return false;

                        var stats = Entity.Stats;
                        var tileStats = tile.Entity.Stats;
                        if (tileStats.Height < stats.Height)
                            return false;

                        return tileStats.Opaque;
                    },
                    (x, y) => visiblePoints.Add(new Point2D(x, y)));

                return map.CreateDerivedWorld2D(Entity.Location, x => visiblePoints.Contains(x));
            }
        }

        private void DeltaEnergy(int energy)
        {
            if (Stats.Alive == false)
                throw new Exception("Entity can not spend energy while dead.");

            if (Stats.Energy + energy < 0)
                throw new Exception($"Spending {-energy} energy would kill the entity.");

            if (Stats.Energy + energy > MaxStats.Energy)
                throw new Exception($"Restoring {energy} energy would over-feed the entity.");

            Entity.Stats.Energy += (uint)energy;
        }
    }

    public class DestinationContainsObjectException : Exception { }
}
