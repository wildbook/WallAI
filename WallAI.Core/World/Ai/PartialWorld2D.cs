using System;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Helpers.Disposable;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World.Ai
{
    public class PartialWorld2D : IWorld2D, IReadOnlyWorld2D
    {
        private readonly IWorld2D _world2D;
        private readonly Func<Point2D, bool> _isVisible;
        private readonly CountingDisposable _lifetime;

        internal PartialWorld2D(IWorld2D world2D, Func<Point2D, bool> isVisible, IDisposable lifetime = null)
        {
            _world2D = world2D;
            _isVisible = isVisible;
            _lifetime = new CountingDisposable(lifetime);
        }

        public PartialWorld2D CreateDerivedWorld2D(
            Func<Point2D, bool> isVisible) =>
            new PartialWorld2D(this, isVisible, _lifetime.CreateChild());

        public ITile2D this[Point2D point]
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                if (!_isVisible(point))
                    throw new IndexOutOfRangeException();
                return _world2D[point.X, point.Y];
            }
            set
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                if (!_isVisible(point))
                    throw new IndexOutOfRangeException();
                _world2D[point.X, point.Y] = value;
            }
        }

        public int Seed => _world2D.Seed;

        IReadOnlyTile2D IReadOnlyWorld2D.this[int x, int y] => this[x, y];
        public ITile2D this[int x, int y]
        {
            get => this[new Point2D(x, y)];
            set => this[new Point2D(x, y)] = value;
        }

        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities.Cast<IReadOnlyWorld2DEntity>();
        public IEnumerable<IWorld2DEntity> Entities
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                var entities = _world2D.Entities;
                var visibleEntities = entities.Where(x => _isVisible(x.Location));
                return visibleEntities.ToArray();
            }
        }

        public void Dispose() => _lifetime.Dispose();
    }
}