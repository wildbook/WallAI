using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World
{
    public class World2D : IWorld2D, IReadOnlyWorld2D
    {
        public int Seed { get; }

        private readonly IWorld2DMethods _methods;
        internal readonly ConcurrentDictionary<Point2D, ITile2D> Tiles;
        
        private IEnumerable<World2DEntity> Entities
            => Tiles.Where(x => x.Value.Entity != null)
                    .Select(x => new World2DEntity(this, x.Value.Entity, x.Key.X, x.Key.Y));

        IEnumerable<IWorld2DEntity> IWorld2D.Entities => Entities;
        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities;

        IReadOnlyTile2D IReadOnlyWorld2D.this[int x, int y] => this[x, y];
        ITile2D IWorld2D.this[int x, int y]
        {
            get => this[x, y];
            set => this[x, y] = value;
        }

        protected internal virtual ITile2D this[int x, int y]
        {
            get
            {
                var coordinate = new Point2D(x, y);
                return Tiles.TryGetValue(coordinate, out var tile)
                    ? tile
                    : new TemporaryTile2D(_methods.GenerateTile(Seed, x, y), z => this[x, y] = z);
            }
            set
            {
                var coordinate = new Point2D(x, y);
                var defaultTile = _methods.GenerateTile(Seed, x, y);

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
    }
}