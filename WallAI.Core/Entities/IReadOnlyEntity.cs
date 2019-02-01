using WallAI.Core.Entities.Stats;
using WallAI.Core.Interfaces;

namespace WallAI.Core.Entities
{
    public interface IReadOnlyEntity : IHasReadOnlyId
    {
        IReadOnlyStats Stats { get; }
        IReadOnlyStats MaxStats { get; }
    }
}