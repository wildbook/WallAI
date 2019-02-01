using WallAI.Core.Entities;
using WallAI.Core.Interfaces;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    public interface IReadOnlyWorld2DEntity : IReadOnlyEntity, IHasReadOnlyLocation
    {
        new IReadOnlyWorld2DEntity WithLocationOffset(Point2D offset);
    }
}