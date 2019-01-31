using System.Collections.Generic;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    public interface IReadOnlyPartialWorld2D : IReadOnlyWorld2D
    {
        Rectangle2D MaxArea { get; }
        IEnumerable<IReadOnlyWorld2DTile2D> Tiles { get; }
    }
}
