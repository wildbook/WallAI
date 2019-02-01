using System.Runtime.Serialization;
using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    public interface IWorld2DEntity : IReadOnlyWorld2DEntity, IEntity
    {
        [DataMember] new Point2D Location { get; }
        [IgnoreDataMember] new IWorld2D World { get; }

        new IWorld2DEntity WithLocationOffset(Point2D offset);
    }
}