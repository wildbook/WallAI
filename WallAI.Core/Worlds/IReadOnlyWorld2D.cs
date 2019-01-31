using System;
using System.Collections.Generic;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Entities;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    public interface IReadOnlyWorld2D : IDisposable
    {
        IReadOnlyTile2D this[Point2D location] { get; }
        IEnumerable<IReadOnlyWorld2DEntity> Entities { get; }
        IEnumerable<IReadOnlyWorld2DTile2D> TilesInRange(Circle2D circle);
        IEnumerable<IReadOnlyWorld2DTile2D> TilesInRect(Rectangle2D circle);
        IReadOnlyPartialWorld2D CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible);
    }
}