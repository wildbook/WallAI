using System;
using System.Collections.Generic;
using System.Text;

namespace WallAI.Core.Entities.Stats
{
    public interface IReadOnlyStats
    {
        bool Alive { get; }
        uint Energy { get; }
    }
}
