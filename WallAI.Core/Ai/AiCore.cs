using System;
using WallAI.Core.Ai.Vision;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;
using WallAI.Core.Helpers;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds;
using WallAI.Core.Worlds.Ai;
using WallAI.Core.Worlds.Entities;

namespace WallAI.Core.Ai
{
    public class AiCore : IAiCore, IDisposable
    {
        private readonly AiWorld2D _aiWorld2D;
        private readonly IPartialWorld2D _world2D;
        private IWorld2DEntity Entity { get; }
        public IReadOnlyStats Stats => Entity.Stats;
        public IReadOnlyStats MaxStats => Entity.MaxStats;
        public Circle2D Vision => new Circle2D(Entity.Location, Stats.VisionRadius);
        public Point2D Location => Entity.Location;
        public int Tick { get; }

        public AiCore(AiWorld2D aiWorld2D, IWorld2DEntity entity, int tick)
        {
            _aiWorld2D = aiWorld2D;
            Entity = entity.WithLocationOffset(Point2D.Zero - entity.Location);
            Tick = tick;

            var visionRange = entity.Stats.VisionRadius;
            var visionOffset = new Point2D(visionRange, visionRange);
            var visionRect = new Rectangle2D(
                Location - visionOffset,
                Location + visionOffset);

            _world2D = aiWorld2D.RequestRect(entity.Id, entity.Location, visionRect);
        }

        public Random GetRandom() => new LoggingRandom(_aiWorld2D.Seed ^ Location.X ^ Location.Y ^ Tick);

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

            if (_world2D[destinationPoint].Entity != null)
                return new ActionStatus(ActionStatus.Status.InvalidAction, "Target cell contains an entity.");

            var energyStatus = DeltaEnergy(-1);

            if (energyStatus.Success)
            {
                var obj = _world2D[Location];
                _world2D[Location] = new Tile2D();
                _world2D[destinationPoint] = obj;
            }

            return energyStatus;
        }

        public void Kill() => Entity.Stats.Alive = false;

        public IReadOnlyWorld2D GetVisibleWorld()
        {
            var visionRadius = Stats.VisionRadius;
            
            var fov = new RPAS(RPAS.Configuration.Default);
            var visiblePoints = fov.CalculateVisibleCells(new Circle2D(Entity.Location, visionRadius - 1), x => !IsOpaque(x.X, x.Y));

            var visionOffset = new Point2D(visionRadius, visionRadius);
            var visionMaxRect = new Rectangle2D(Location - visionOffset, Location + visionOffset);

            return _world2D.CreateDerivedWorld2D(Entity.Location, visionMaxRect, x => visiblePoints.Contains(x));

            bool IsOpaque(int x, int y)
            {
                var point = new Point2D(x, y);
                if (point.Equals(Entity.Location))
                    return false;

                var tile = _world2D[point];
                if (tile.Entity == null)
                    return false;

                if (!tile.Entity.Stats.Opaque)
                    return false;

                return true;
            }
        }

        private ActionStatus DeltaEnergy(int energy)
        {
            if (Stats.Alive == false)
                return new ActionStatus(ActionStatus.Status.InvalidAction, "Entity can not spend energy while dead.");

            if (Stats.Energy + energy < 0)
                return new ActionStatus(ActionStatus.Status.InsufficientEnergy, $"Spending {-energy} energy would kill the entity.");

            if (Stats.Energy + energy > MaxStats.Energy)
                return new ActionStatus(ActionStatus.Status.InvalidAction, $"Restoring {energy} energy would over-feed the entity.");

            Entity.Stats.Energy += (uint)energy;

            return new ActionStatus(ActionStatus.Status.Success);
        }

        public void Dispose() => _world2D.Dispose();
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
