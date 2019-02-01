using System.Runtime.Serialization;
using WallAI.Core.Ai;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IEntity : IReadOnlyEntity
    {
        [DataMember] IAi Ai { get; }
        [DataMember] new IStats Stats { get; set; }
        [DataMember] new IStats MaxStats { get; set; }
    }
}