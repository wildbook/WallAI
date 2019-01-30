using System;
using System.Reflection;
using WallAI.Core.Ai;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Exceptions;

namespace WallAI.Core.Entities
{
    public class Entity<T> : IEntity, IReadOnlyEntity where T : IAi, new()
    {
        public IAi Ai { get; }

        public Stats.Stats Stats { get; set; }
        public Stats.Stats MaxStats { get; set; }

        IReadOnlyStats IReadOnlyEntity.MaxStats => MaxStats;
        IStats IEntity.MaxStats
        {
            get => Stats;
            set => Stats = value as Stats.Stats ?? new Stats.Stats(value);
        }

        IReadOnlyStats IReadOnlyEntity.Stats => Stats;
        IStats IEntity.Stats
        {
            get => Stats;
            set => Stats = value as Stats.Stats ?? new Stats.Stats(value);
        }

        public Entity(IStats stats, IStats maxStats)
        {
            Ai = new T();
            Stats = new Stats.Stats(stats);
            MaxStats = new Stats.Stats(maxStats);

            Stats.EnsureNotGreaterThan(MaxStats);
        }
    }
}
