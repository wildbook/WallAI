using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.World.Entities
{
    public interface IWorld2DEntity : IEntity
    {
        Point2D Location { get; }
        IWorld2D World { get; }
    }
}