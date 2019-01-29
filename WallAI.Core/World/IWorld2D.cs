using System;
using System.Collections.Generic;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;
using WallAI.Core.World.Tiles;

namespace WallAI.Core.World
{
    public interface IWorld2D : IReadOnlyWorld2D
    {
        int Seed { get; }
        new ITile2D this[Point2D location] { get; set; }
        new IEnumerable<IWorld2DEntity> Entities { get; }
        new IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle);
        new IWorld2D CreateDerivedWorld2D(Func<Point2D, bool> isVisible);
    }
}