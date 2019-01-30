using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.World.Entities
{
    public interface IWorld2DEntity : IReadOnlyWorld2DEntity, IEntity
    {
        new Point2D Location { get; }
        new IWorld2D World { get; }
    }
}