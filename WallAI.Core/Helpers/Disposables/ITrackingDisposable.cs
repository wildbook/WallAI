using System;

namespace WallAI.Core.Helpers.Disposables
{
    public interface ITrackingDisposable : IDisposable
    {
        bool Disposed { get; }
        void ThrowIfDisposed();
        void ThrowIfDisposed(string name);
    }
}
