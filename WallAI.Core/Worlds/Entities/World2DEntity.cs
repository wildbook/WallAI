using System;
using WallAI.Core.Ai;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    internal class World2DEntity : IWorld2DEntity
    {
        public Point2D Location { get; }

        private readonly IWorld2D _world;
        private readonly IEntity _entity;

        public Guid Id => _entity.Id;
        public IAi Ai => _entity.Ai;
        public IWorld2DEntity WithLocationOffset(Point2D offset) => new World2DEntity(_world, _entity, Location + offset);
        public IStats MaxStats { get => _entity.MaxStats; set => _entity.MaxStats = new Stats(value); }
        public IStats Stats { get => _entity.Stats; set => _entity.Stats = new Stats(value); }

        internal World2DEntity(IWorld2D world, IEntity entity, Point2D location)
        {
            Location = location;
            _entity = entity;
            _world = world;
        }

        #region Interface implementations
        IWorld2DEntity IWorld2DEntity.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IReadOnlyWorld2DEntity IReadOnlyWorld2DEntity.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);

        IStats IEntity.MaxStats { get => MaxStats; set => MaxStats = value; }
        IReadOnlyStats IReadOnlyEntity.MaxStats => _entity.MaxStats;

        IWorld2D IWorld2DEntity.World => _world;
        IReadOnlyWorld2D IReadOnlyWorld2DEntity.World => _world;

        IStats IEntity.Stats { get => Stats; set => Stats = value; }
        IReadOnlyStats IReadOnlyEntity.Stats => _entity.Stats;
        #endregion
    }
}