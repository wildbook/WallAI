using System;
using System.Runtime.Serialization;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Ai
{
    //TODO: Rename AiWorld2D - A better name would be SafeWorld2D, ConcurrentWorld2D, or something else that clarifies what it does.
    [DataContract]
    public class AiWorld2D
    {
        //TODO: Prevent multiple entities locking the same tile.
        // In theory this is simple, in practice I want it to be deterministic, meaning:
        //   Order of map requests being granted has to always be the same.
        //   Order of map requests being granted can not be random.
        //   Order of map requests being granted can not be decided by call order.

        [DataMember(Name = "seed")]
        public int Seed => _w2D.Seed;

        private readonly IWorld2D _w2D;
        public AiWorld2D(IWorld2D w2D)
        {
            _w2D = w2D;
        }

        public IPartialWorld2D RequestRect(Guid entityId, Point2D center, Rectangle2D area) => new PartialWorld2D(_w2D, center, area, x => true);
    }
}
