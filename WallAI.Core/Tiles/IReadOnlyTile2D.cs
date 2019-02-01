using System;
using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public interface IReadOnlyTile2D
    {
        Guid Id { get; }
        IReadOnlyEntity Entity { get; }
    }
}