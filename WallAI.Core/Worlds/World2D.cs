using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Ai;
using WallAI.Core.Worlds.Entities;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    [DataContract]
    public class World2D : IWorld2D
    {
        private readonly IWorld2DMethods _methods;
        private readonly ConcurrentDictionary<Point2D, ITile2D> _tiles;

        [DataMember]
        public IEnumerable<IWorld2DTile2D> Tiles
            => _tiles.Select(x => new World2DTile2D(x.Value, x.Key));

        [DataMember]
        public IEnumerable<IWorld2DEntity> Entities
            => _tiles.Where(x => x.Value.Entity != null)
                     .Select(x => new World2DEntity(x.Value.Entity, x.Key));

        [DataMember]
        public int Seed { get; }

        public static IWorld2D Create(IWorld2DMethods methods, int seed)
            => new World2D(methods, seed);

        public IPartialWorld2D CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible)
            => new PartialWorld2D(this, center, worldRect, isVisible);

        public void Dispose() { }

        public IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle)
        {
            for (var x = -circle.Radius; x < circle.Radius; x++)
                for (var y = -circle.Radius; y < circle.Radius; y++)
                {
                    var point = circle.Origin + new Point2D(x, y);
                    if (circle.ContainsPoint(point))
                        yield return new World2DTile2D(this[point], point);
                }
        }

        public IEnumerable<IWorld2DTile2D> TilesInRect(Rectangle2D rect)
        {
            for (var x = rect.X1; x < rect.X2; x++)
                for (var y = rect.Y1; y < rect.Y2; y++)
                {
                    var point = new Point2D(x, y);
                    if (rect.ContainsPoint(point))
                        yield return new World2DTile2D(this[point], point);
                }
        }

        protected internal virtual ITile2D this[Point2D coordinate]
        {
            get
            {
                if (_tiles.TryGetValue(coordinate, out var tile))
                    return tile;

                var generatedTile = _methods.GenerateTile(Seed, coordinate);
                return this[coordinate] = generatedTile;
            }
            set
            {
                if (value != null)
                    _tiles[coordinate] = value;
                else if (!_tiles.TryRemove(coordinate, out _))
                    throw new Exception();
            }
        }

        protected World2D(IWorld2DMethods methods, int seed)
        {
            Seed = seed;
            _methods = methods;
            _tiles = new ConcurrentDictionary<Point2D, ITile2D>();
        }

        int IWorld2D.Seed => Seed;

        IEnumerable<IWorld2DEntity> IWorld2D.Entities => Entities;
        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities;

        IEnumerable<IWorld2DTile2D> IWorld2D.Tiles => Tiles;
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.Tiles => Tiles;

        ITile2D IWorld2D.this[Point2D location] { get => this[location]; set => this[location] = value; }
        IReadOnlyTile2D IReadOnlyWorld2D.this[Point2D location] => this[location];

        IPartialWorld2D IWorld2D.CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible) => CreateDerivedWorld2D(center, worldRect, isVisible);
        IReadOnlyPartialWorld2D IReadOnlyWorld2D.CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible) => CreateDerivedWorld2D(center, worldRect, isVisible);

        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);

        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);
    }
}