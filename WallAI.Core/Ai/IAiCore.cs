using System;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Worlds;

namespace WallAI.Core.Ai
{
    public interface IAiCore
    {
        IReadOnlyStats MaxStats { get; }
        IReadOnlyStats Stats { get; }
        Circle2D Vision { get; }
        Random Random { get; }

        IReadOnlyWorld2D GetVisibleWorld();
        ActionStatus Move(Direction direction);
        void Kill();
    }
}
