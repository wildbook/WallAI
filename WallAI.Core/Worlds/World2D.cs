using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Ai;
using WallAI.Core.Worlds.Entities;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    public class World2D : IWorld2D
    {
        public int Seed { get; }

        private readonly IWorld2DMethods _methods;
        internal readonly ConcurrentDictionary<Point2D, ITile2D> Tiles;

        private IEnumerable<World2DEntity> Entities
            => Tiles.Where(x => x.Value.Entity != null)
                    .Select(x => new World2DEntity(this, x.Value.Entity, x.Key));

        private IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle)
        {
            for (var x = -circle.Radius; x < circle.Radius; x++)
            for (var y = -circle.Radius; y < circle.Radius; y++)
            {
                var point = circle.Origin + new Point2D(x, y);
                if (circle.ContainsPoint(point))
                    yield return new World2DTile2D(this[point], point, this);
            }
        }

        private IEnumerable<IWorld2DTile2D> TilesInRect(Rectangle2D rect)
        {
            for (var x = rect.X1; x < rect.X2; x++)
            for (var y = rect.Y1; y < rect.Y2; y++)
            {
                var point = new Point2D(x, y);
                if (rect.ContainsPoint(point))
                    yield return new World2DTile2D(this[point], point, this);
            }
        }

        IReadOnlyPartialWorld2D IReadOnlyWorld2D.CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible)
            => CreateDerivedWorld2D(center, worldRect, isVisible);

        public IPartialWorld2D CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible)
            => new PartialWorld2D(this, center, worldRect, isVisible);

        IEnumerable<IWorld2DEntity> IWorld2D.Entities => Entities;
        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities;

        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);

        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);

        IReadOnlyTile2D IReadOnlyWorld2D.this[Point2D location] => this[location];
        ITile2D IWorld2D.this[Point2D location]
        {
            get => this[location];
            set => this[location] = value;
        }

        protected internal virtual ITile2D this[Point2D coordinate]
        {
            get
            {
                if (Tiles.TryGetValue(coordinate, out var tile))
                    return tile;

                var generatedTile = _methods.GenerateTile(Seed, coordinate);

                //if (generatedTile.Entity == null)
                //    return new TemporaryTile2D(generatedTile, z => this[coordinate] = z);

                return this[coordinate] = generatedTile;
            }
            set
            {
                //var defaultTile = _methods.GenerateTile(Seed, coordinate);
                //
                //if (defaultTile.Entity == null && Equals(defaultTile, value))
                //{
                //    if (!Tiles.TryRemove(coordinate, out _))
                //        throw new Exception("Failed to remove non-modified tile.");
                //}
                //else
                //{
                if (value != null)
                    Tiles[coordinate] = value;
                else if (!Tiles.TryRemove(coordinate, out _))
                    throw new Exception();
                //}
            }
        }

        public static IWorld2D Create(IWorld2DMethods methods, int seed) => new World2D(methods, seed);

        protected World2D(IWorld2DMethods methods, int seed)
        {
            Seed = seed;
            _methods = methods;
            Tiles = new ConcurrentDictionary<Point2D, ITile2D>();
        }

        public void Dispose() { }
    }
}