using WallAI.Core.Ai;
using WallAI.Core.Entities.Stats;

namespace WallAI.Core.Entities
{
    public interface IEntity
    {
        IAi Ai { get; }
        IStats Stats { get; set; }
        IStats MaxStats { get; set; }
    }
}