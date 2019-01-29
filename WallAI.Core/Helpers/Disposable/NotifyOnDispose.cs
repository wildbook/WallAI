using System;

namespace WallAI.Core.Helpers.Disposable
{
    public class NotifyOnDispose : ITrackingDisposable
    {
        public bool Disposed { get; private set; }
        private readonly Action _onDispose;

        public void ThrowIfDisposed(string name)
        {
            if (Disposed)
                throw new ObjectDisposedException(name);
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
