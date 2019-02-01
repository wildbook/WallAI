using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Interfaces
{
    public interface IHasReadOnlyLocation
    {
        Point2D Location { get; }
        IHasReadOnlyLocation WithLocationOffset(Point2D offset);
    }
}
