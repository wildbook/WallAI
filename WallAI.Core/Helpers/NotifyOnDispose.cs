using System;

namespace WallAI.Core.Helpers
{
    public class NotifyOnDispose : IDisposable
    {
        public bool Disposed;
        private readonly Action _onDispose;

        public bool ThrowIfDisposed(string name)
        {
            if (Disposed)
                throw new ObjectDisposedException(name);

            return Disposed;
        }

        public void ThrowIfDisposed()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(NotifyOnDispose));
        }

        public NotifyOnDispose(Action onDispose)
        {
            Disposed = false;
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (!Disposed && (Disposed = true))
                _onDispose?.Invoke();
        }
    }

    public class NotifyOnDispose<T> : NotifyOnDispose
    {
        public T Data { get; }
        public NotifyOnDispose(T data, Action onDispose) : base(onDispose) => Data = data;
    }
}
