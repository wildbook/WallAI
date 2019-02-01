using System.Runtime.Serialization;
using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    public interface IReadOnlyWorld2DEntity : IReadOnlyEntity
    {
        [DataMember] Point2D Location { get; }
        [IgnoreDataMember] IReadOnlyWorld2D World { get; }

        IReadOnlyWorld2DEntity WithLocationOffset(Point2D offset);
    }
}