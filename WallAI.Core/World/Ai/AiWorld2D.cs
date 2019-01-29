using System;
using System.Collections.Concurrent;
using WallAI.Core.Helpers;
using WallAI.Core.Helpers.Disposable;
using WallAI.Core.Math.Geometry;

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
            var partial = new PartialWorld2D(_w2D, rect.ContainsPoint, lifetime);

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
}
