using System;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IReadOnlyEntity
    {
        Guid Id { get; }
        IReadOnlyStats Stats { get; }
        IReadOnlyStats MaxStats { get; }
    }
}