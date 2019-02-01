using System;
using System.Runtime.Serialization;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IReadOnlyEntity
    {
        [DataMember] Guid Id { get; }
        [DataMember] IReadOnlyStats Stats { get; }
        [DataMember] IReadOnlyStats MaxStats { get; }
    }
}