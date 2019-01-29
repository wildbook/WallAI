using System;
using System.Collections.Concurrent;

namespace WallAI.Core.Helpers
{
    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        }

        public T GetObject() => _objects.TryTake(out var item) ? item : _objectGenerator();
        public void PutObject(T item) => _objects.Add(item);
    }
}
