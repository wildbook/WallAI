using WallAI.Core.Entities;
using WallAI.Core.Interfaces;

namespace WallAI.Core.Tiles
{
    public interface IReadOnlyTile2D : IHasReadOnlyId
    {
        IReadOnlyEntity Entity { get; }
    }
}