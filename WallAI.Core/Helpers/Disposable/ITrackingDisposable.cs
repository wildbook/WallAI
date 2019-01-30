using System;

namespace WallAI.Core.Helpers.Disposable
{
    public interface ITrackingDisposable : IDisposable
    {
        bool Disposed { get; }
        void ThrowIfDisposed();
        void ThrowIfDisposed(string name);
    }
}
