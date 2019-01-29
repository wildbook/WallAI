using System;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Ai;
using WallAI.Core.World.Entities;

namespace WallAI.Core.Ai
{
    public class AiCore : IAiCore
    {
        private readonly AiWorld2D _aiWorld2D;
        private IWorld2DEntity Entity { get; }
        public IReadOnlyStats Stats => (IReadOnlyStats)Entity.Stats;
        public IReadOnlyStats MaxStats => (IReadOnlyStats)Entity.MaxStats;
        public Point2D Location => Entity.Location;
        public int Tick { get; }

        public AiCore(AiWorld2D aiWorld2D, IWorld2DEntity entity, int tick)
        {
            _aiWorld2D = aiWorld2D;
            Entity = entity;
            Tick = tick;
        }

        public PartialWorld2D LockRect(Rectangle2D rect) => _aiWorld2D.LockRect(rect);

        public Random GetRandom() => new Random(Tick);

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

            using (var map = LockRect(lockRect))
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
