using WallAI.Core.Entities;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.World.Entities
{
    public interface IReadOnlyWorld2DEntity : IReadOnlyEntity
    {
        Point2D Location { get; }
        IReadOnlyEntity Entity { get; }
        IReadOnlyWorld2D World { get; }
    }
}