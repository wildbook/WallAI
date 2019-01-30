using WallAI.Core.Ai;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.World.Entities
{
    internal class World2DEntity : IWorld2DEntity
    {
        public IAi Ai => _entity.Ai;

        IReadOnlyWorld2D IReadOnlyWorld2DEntity.World => _world;
        IReadOnlyStats IReadOnlyEntity.MaxStats => _entity.MaxStats;
        IReadOnlyStats IReadOnlyEntity.Stats => _entity.Stats;

        IWorld2D IWorld2DEntity.World => _world;

        IStats IEntity.MaxStats
        {
            get => _entity.Stats;
            set => _entity.Stats = new Stats(value);
        }
 
        IStats IEntity.Stats
        {
            get => _entity.Stats;
            set => _entity.Stats = new Stats(value);
        }

        private readonly IWorld2D _world;
        private readonly IEntity _entity;

        internal World2DEntity(IWorld2D world, IEntity entity, int x, int y)
        {
            Location = new Point2D(x, y);
            _entity = entity;
            _world = world;
        }

        public Point2D Location { get; }
    }
}