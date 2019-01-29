﻿using System;
using System.Collections.Generic;
using WallAI.Core.Tiles;
using WallAI.Core.World.Entities;

namespace WallAI.Core.World
{
    public interface IWorld2D : IDisposable
    {
        int Seed { get; }
        ITile2D this[int x, int y] { get; set; }
        IEnumerable<IWorld2DEntity> Entities { get; }
    }
}