using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Entities;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    public interface IWorld2D : IReadOnlyWorld2D
    {
        [DataMember] int Seed { get; }
        new ITile2D this[Point2D location] { get; set; }
        new IEnumerable<IWorld2DEntity> Entities { get; }
        new IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle);
        new IEnumerable<IWorld2DTile2D> TilesInRect(Rectangle2D circle);
        new IPartialWorld2D CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible);
    }
}