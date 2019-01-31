using System.Collections.Generic;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    public interface IPartialWorld2D : IWorld2D, IReadOnlyPartialWorld2D
    {
        new IEnumerable<IWorld2DTile2D> Tiles { get; }
    }
}
