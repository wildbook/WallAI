using System;

namespace WallAI.Core.Helpers.Disposable
{
    interface ITrackingDisposable : IDisposable
    {
        bool Disposed { get; }
        void ThrowIfDisposed();
        void ThrowIfDisposed(string name);
    }
}
