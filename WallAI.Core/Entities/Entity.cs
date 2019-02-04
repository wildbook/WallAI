using System;
using System.Runtime.Serialization;
using WallAI.Core.Ai;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public class Entity<T> : IEntity, IEquatable<Entity<T>> where T : IAi, new()
    {
        [DataMember(Name = "ai")]
        public IAi Ai { get; }

        [DataMember(Name = "stats")]
        public IStats Stats { get; set; }

        [DataMember(Name = "maxStats")]
        public IStats MaxStats { get; set; }

        [DataMember(Name = "id")]
        public Guid Id { get; }

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

        public Entity(Guid id, T ai, IStats stats, IStats maxStats)
        {
            Ai = ai;
            Stats = stats;
            MaxStats = maxStats;
            Id = id;

            new Stats.Stats(Stats).EnsureNotGreaterThan(MaxStats);
        }

        public Entity(Guid id, IStats stats, IStats maxStats) : this(id, new T(), stats, maxStats) { }
        public Entity(Guid id, IStats stats) : this(id, new T(), stats, stats) { }

        public bool Equals(Entity<T> other) => other.Id == Id;
    }
}
