using WallAI.Core.Ai;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IEntity : IReadOnlyEntity
    {
        IAi Ai { get; }
        new IStats Stats { get; set; }
        new IStats MaxStats { get; set; }
    }
}