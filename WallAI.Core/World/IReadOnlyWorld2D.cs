using System;
using System.Collections.Generic;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;
using WallAI.Core.World.Tiles;

namespace WallAI.Core.World
{
    public interface IReadOnlyWorld2D : IDisposable
    {
        IReadOnlyTile2D this[Point2D location] { get; }
        IEnumerable<IReadOnlyWorld2DEntity> Entities { get; }
        IEnumerable<IReadOnlyWorld2DTile2D> TilesInRange(Circle2D circle);
        IReadOnlyWorld2D CreateDerivedWorld2D(Func<Point2D, bool> isVisible);
    }
}