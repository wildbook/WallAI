using System;
using WallAI.Core.Entities;

namespace WallAI.Core.Tiles
{
    public class TemporaryTile2D : ITile2D
    {
        public Guid Id => _tile2D.Id;
        private readonly ITile2D _tile2D;
        private readonly Action<ITile2D> _onSet;

        public IEntity Entity
        {
            get => _tile2D.Entity;
            set
            {
                _tile2D.Entity = value;
                _onSet(_tile2D);
            }
        }

        IEntity ITile2D.Entity { get => Entity; set => Entity = value; }
        IReadOnlyEntity IReadOnlyTile2D.Entity => Entity;

        public TemporaryTile2D(ITile2D tile2D, Action<ITile2D> onSet)
        {
            _tile2D = tile2D;
            _onSet = onSet;
        }
    }
}
