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

            foreach (var property in Stats.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                var prop = typeof(IStats).GetProperty(property.Name);
                var statsVal = prop.GetValue(Stats);
                var maxStatsVal = prop.GetValue(MaxStats);

                if (statsVal.GetType() != maxStatsVal.GetType())
                    throw new Exception($"Stat {prop.Name} does not match type of MaxStat.");

                switch (statsVal)
                {
                    case bool sv when maxStatsVal is bool msv:
                        if (sv && !msv) throw new StatOutOfRangeException<bool>(prop.Name, true, false);
                        break;

                    case int sv when maxStatsVal is int msv:
                        if (sv < msv) throw new StatOutOfRangeException<int>(prop.Name, sv, msv);
                        break;

                    case uint sv when maxStatsVal is uint msv:
                        if (sv > msv) throw new StatOutOfRangeException<uint>(prop.Name, sv, msv);
                        break;

                    default:
                        throw new Exception($"Invalid stat type ({statsVal.GetType()}) for stat {prop.Name}.");
                }
            }
        }
    }
}
