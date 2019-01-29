using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Ai;
using WallAI.Core.World.Entities;
using WallAI.Core.World.Tiles;

namespace WallAI.Core.World
{
    public class World2D : IWorld2D
    {
        public int Seed { get; }

        private readonly IWorld2DMethods _methods;
        internal readonly ConcurrentDictionary<Point2D, ITile2D> Tiles;

        private IEnumerable<World2DEntity> Entities
            => Tiles.Where(x => x.Value.Entity != null)
                    .Select(x => new World2DEntity(this, x.Value.Entity, x.Key.X, x.Key.Y));

        private IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle)
        {
            for (var x = 0; x < circle.Radius * 2; x++)
            {
                for (var y = 0; y < circle.Radius * 2; y++)
                {
                    var point = new Point2D(x, y);
                    if (circle.ContainsPoint(point))
                        yield return new World2DTile2D(this[point], point, this);
                }
            }
        }

        IReadOnlyWorld2D IReadOnlyWorld2D.CreateDerivedWorld2D(Func<Point2D, bool> isVisible) => CreateDerivedWorld2D(isVisible);

        public IWorld2D CreateDerivedWorld2D(Func<Point2D, bool> isVisible) => new PartialWorld2D(this, isVisible);

        IEnumerable<IWorld2DEntity> IWorld2D.Entities => Entities;
        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities;

        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle).Cast<IReadOnlyWorld2DTile2D>();

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
                return Tiles.TryGetValue(coordinate, out var tile)
                    ? tile
                    : new TemporaryTile2D(_methods.GenerateTile(Seed, coordinate), z => this[coordinate] = z);
            }
            set
            {
                var defaultTile = _methods.GenerateTile(Seed, coordinate);

                if (Equals(defaultTile, value))
                {
                    if (!Tiles.TryRemove(coordinate, out _))
                        throw new Exception("Failed to remove non-modified tile.");
                }
                else
                {
                    if (value != null)
                        Tiles[coordinate] = value;
                    else if (!Tiles.TryRemove(coordinate, out _))
                        throw new Exception();
                }
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