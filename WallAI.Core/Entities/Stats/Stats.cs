using System;
using System.Reflection;
using System.Runtime.Serialization;
using WallAI.Core.Exceptions;

namespace WallAI.Core.Entities.Stats
{
    [DataContract]
    public class Stats : IStats
    {
        [DataMember(Name = "alive")]
        public bool Alive { get; set; }

        [DataMember(Name = "energy")]
        public uint Energy { get; set; }

        [DataMember(Name = "opaque")]
        public bool Opaque { get; set; }

        [DataMember(Name = "visionRadius")]
        public byte VisionRadius { get; set; }

        public Stats() { }
        public Stats(IStats stats)
        {
            foreach (var property in typeof(IStats).GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                var prop = typeof(IStats).GetProperty(property.Name);
                prop.SetValue(this, prop.GetValue(stats));
            }
        }

        public void EnsureNotGreaterThan(IStats maxStats)
        {
            foreach (var property in typeof(IStats).GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
            {
                var prop = typeof(IStats).GetProperty(property.Name);
                var statsVal = prop.GetValue(this);
                var maxStatsVal = prop.GetValue(maxStats);

                if (statsVal.GetType() != maxStatsVal.GetType())
                    throw new Exception($"Stat {prop.Name} does not match type of MaxStat.");

                switch (statsVal)
                {
                    case bool sv when maxStatsVal is bool msv:
                        if (sv && !msv) throw new StatOutOfRangeException<bool>(prop.Name, true, false);
                        break;

                    case int sv when maxStatsVal is int msv:
                        if (sv > msv) throw new StatOutOfRangeException<int>(prop.Name, sv, msv);
                        break;

                    case uint sv when maxStatsVal is uint msv:
                        if (sv > msv) throw new StatOutOfRangeException<uint>(prop.Name, sv, msv);
                        break;

                    case byte sv when maxStatsVal is byte msv:
                        if (sv > msv) throw new StatOutOfRangeException<byte>(prop.Name, sv, msv);
                        break;

                    default:
                        throw new Exception($"Invalid stat type ({statsVal.GetType()}) for stat {prop.Name}.");
                }
            }
        }
    }
}
