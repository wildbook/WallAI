using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Interfaces
{
    public interface IHasLocation : IHasReadOnlyLocation
    {
        new Point2D Location { get; set; }
        new IHasLocation WithLocationOffset(Point2D offset);
    }
}
