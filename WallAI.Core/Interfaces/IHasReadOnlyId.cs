using System;

namespace WallAI.Core.Interfaces
{
    public interface IHasReadOnlyId
    {
        Guid Id { get; }
    }
}
