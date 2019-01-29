using System;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Enums;

namespace WallAI.Core.Ai
{
    public interface IAiCore
    {
        IReadOnlyStats Stats { get; }
        IReadOnlyStats MaxStats { get; }

        Random GetRandom();
        void Move(Direction direction);
        void Kill();
    }
}