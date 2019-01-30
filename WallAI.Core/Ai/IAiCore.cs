using System;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;
using WallAI.Core.Math.Geometry;
using WallAI.Core.World;

namespace WallAI.Core.Ai
{
    public interface IAiCore
    {
        IReadOnlyStats Stats { get; }
        IReadOnlyStats MaxStats { get; }

        // TODO: Remove IAiCore.Vision
        // Currently allows the player to cheat by saving Vision.Origin and pooling AI instances.
        Circle2D Vision { get; }

        Random GetRandom();
        IReadOnlyWorld2D GetVisibleWorld();
        ActionStatus Move(Direction direction);
        void Kill();
    }
}