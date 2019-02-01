using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds
{
    public interface IReadOnlyPartialWorld2D : IReadOnlyWorld2D
    {
        Rectangle2D MaxArea { get; }
    }
}
