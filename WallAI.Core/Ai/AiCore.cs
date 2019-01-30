using System;
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
        public Point2D Location => Entity.Location;
        public int Tick { get; }

        public AiCore(AiWorld2D aiWorld2D, IWorld2DEntity entity, int tick)
        {
            _aiWorld2D = aiWorld2D;
            Entity = entity;
            Tick = tick;
        }

        public IWorld2D LockRect(Rectangle2D rect) => _aiWorld2D.LockRect(rect);

        public Random GetRandom() => new Random(_aiWorld2D.Seed + Tick);

        public ActionStatus Move(Direction direction)
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

            using (var map = LockRect(lockRect))
            {
                if (map[destinationPoint].Entity != null)
                    return new ActionStatus(ActionStatus.Status.InvalidAction, "Target cell contains an entity.");
                
                var energyStatus = DeltaEnergy(-1);

                if (energyStatus.Success)
                {
                    var obj = map[Location];
                    map[Location] = new Tile2D();
                    map[destinationPoint] = obj;
                }

                return energyStatus;
            }
        }

        public void Kill() => Entity.Stats.Alive = false;

        public IReadOnlyWorld2D GetVisibleWorld()
        {
            var visionRadius = Stats.VisionRadius;

            var lockRect = new Rectangle2D(Location - new Point2D(visionRadius, visionRadius), visionRadius * 2, visionRadius * 2);
            using (var map = LockRect(Point2D.Zero, lockRect))
            {
                var fov = new RPAS(RPAS.Configuration.Default);
                var visiblePoints = fov.CalculateVisibleCells(new Circle2D(Entity.Location, visionRadius - 1), x => !IsOpaque(x.X, x.Y));

                return map.CreateDerivedWorld2D(Entity.Location, x => visiblePoints.Contains(x));

                bool IsOpaque(int x, int y)
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
                }
            }
        }

        private ActionStatus DeltaEnergy(int energy)
        {
            if (Stats.Alive == false)
                return new ActionStatus(ActionStatus.Status.InvalidAction, "Entity can not spend energy while dead.");

            if (Stats.Energy + energy < 0)
                return new ActionStatus(ActionStatus.Status.InvalidAction, $"Spending {-energy} energy would kill the entity.");

            if (Stats.Energy + energy > MaxStats.Energy)
                return new ActionStatus(ActionStatus.Status.InvalidAction, $"Restoring {energy} energy would over-feed the entity.");

            Entity.Stats.Energy += (uint)energy;

            return new ActionStatus(ActionStatus.Status.Success);
        }
    }

    public readonly struct ActionStatus
    {
        public ActionStatus(Status result, string description = null)
        {
            Result = result;
            Description = description;
        }

        public enum Status
        {
            Success,
            InsufficientEnergy,
            InvalidAction,
        }

        public readonly string Description;
        public readonly Status Result;
        public bool Success => Result == Status.Success;
    }
}
