using System.Runtime.Serialization;
using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    public interface IReadOnlyWorld2DEntity : IReadOnlyEntity
    {
        [DataMember] Point2D Location { get; }

        IReadOnlyWorld2DEntity WithLocationOffset(Point2D offset);
    }
}