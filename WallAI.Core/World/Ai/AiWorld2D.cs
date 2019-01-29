using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WallAI.Core.Helpers;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World.Ai
{
    public class AiWorld2D
    {
        private readonly IWorld2D _w2D;
        private readonly ConcurrentDictionary<Guid, PartialWorld2D> _locked;

        public AiWorld2D(IWorld2D w2D)
        {
            _w2D = w2D;
            _locked = new ConcurrentDictionary<Guid, PartialWorld2D>();
        }

        public int Seed => _w2D.Seed;

        public PartialWorld2D LockRect(Rectangle2D rect)
        {
            var lockId = Guid.NewGuid();
            var lifetime = new NotifyOnDispose(Unlock);
            var partial = new PartialWorld2D(_w2D, rect, lifetime);

            if (_locked.TryAdd(lockId, partial))
                return partial;

            throw new Exception();

            void Unlock()
            {
                if (!_locked.TryRemove(lockId, out _))
                    throw new Exception();
            }
        }
    }

    public class PartialWorld2D : IWorld2D, IDisposable
    {
        private readonly IWorld2D _world2D;
        private readonly Rectangle2D _rect;
        private readonly NotifyOnDispose _lifetime;

        public int Width => _rect.Width;
        public int Height => _rect.Height;

        internal PartialWorld2D(IWorld2D world2D, Rectangle2D rect, NotifyOnDispose lifetime)
        {
            _world2D = world2D;
            _rect = rect;

            _lifetime = lifetime;
        }

        public ITile2D this[Point2D point]
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                if (!_rect.ContainsPoint(point))
                    throw new IndexOutOfRangeException();
                return _world2D[point.X, point.Y];
            }
            set
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                if (!_rect.ContainsPoint(point))
                    throw new IndexOutOfRangeException();
                _world2D[point.X, point.Y] = value;
            }
        }

        public int Seed => _world2D.Seed;

        public ITile2D this[int x, int y]
        {
            get => this[new Point2D(x, y)];
            set => this[new Point2D(x, y)] = value;
        }

        public IEnumerable<IWorld2DEntity> Entities
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                var entities = _world2D.Entities;
                var visibleEntities = entities.Where(x => _rect.ContainsPoint(x.Location));
                return visibleEntities.ToArray();
            }
        }

        public void Dispose() => _lifetime.Dispose();
    }
}
