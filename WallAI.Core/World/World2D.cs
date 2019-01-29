using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Internal;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World
{
    public class World2D : IWorld2D, IReadOnlyWorld2D
    {
        protected readonly int Seed;

        private readonly IWorld2DMethods _methods;
        internal readonly ConcurrentDictionary<Coordinate2D, ITile2D> Tiles;
        
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
                var coordinate = new Coordinate2D(x, y);
                return Tiles.GetOrAdd(coordinate, d => _methods.GenerateTile(Seed, d.X, d.Y));
            }
            set
            {
                var coordinate = new Coordinate2D(x, y);

                if (value != null)
                    Tiles[coordinate] = value;
                else if (!Tiles.TryRemove(coordinate, out _))
                    throw new Exception();
            }
        }

        public static IWorld2D Create(IWorld2DMethods methods, int seed) => new World2D(methods, seed);

        protected World2D(IWorld2DMethods methods, int seed)
        {
            Seed = seed;
            _methods = methods;
            Tiles = new ConcurrentDictionary<Coordinate2D, ITile2D>();
        }
    }
}