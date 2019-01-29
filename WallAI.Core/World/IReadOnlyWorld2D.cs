using System;
using System.Collections.Generic;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World
{
    public interface IReadOnlyWorld2D : IDisposable
    {
        IReadOnlyTile2D this[int x, int y] { get; }
        IEnumerable<IReadOnlyWorld2DEntity> Entities { get; }
    }
}