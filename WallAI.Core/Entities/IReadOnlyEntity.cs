using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IReadOnlyEntity
    {
        IReadOnlyStats Stats { get; }
        IReadOnlyStats MaxStats { get; }
    }
}