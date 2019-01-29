using System;

namespace WallAI.Core.Helpers.Disposable
{
    public class CountingDisposable : ITrackingDisposable
    {
        public bool Disposed => LivingReferences == 0;
        public uint LivingReferences { get; private set; }
        private IDisposable Wrapped { get; }

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

        public CountingDisposable() => LivingReferences++;
        public CountingDisposable(IDisposable wrapped) : this() => Wrapped = wrapped;

        public CountingDisposable CreateChild()
        {
            LivingReferences++;
            return this;
        }

        public void Dispose()
        {
            if (!Disposed)
                LivingReferences--;

            if (Disposed)
                Wrapped?.Dispose();
        }

        public void DisposeInstantly()
        {
            LivingReferences = 0;
            Wrapped?.Dispose();
        }
    }
}
