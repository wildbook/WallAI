using System;
using System.Runtime.Serialization;
using WallAI.Core.Ai;
using WallAI.Core.Entities;
using WallAI.Core.Entities.Stats;
using WallAI.Core.Interfaces;
using WallAI.Core.Math.Geometry;

namespace WallAI.Core.Worlds.Entities
{
    [DataContract]
    public class World2DEntity : IWorld2DEntity
    {
        [IgnoreDataMember]
        private readonly IEntity _wrappedEntity;
        
        [DataMember(Name = "id")]
        public Guid Id => _wrappedEntity.Id;

        [DataMember(Name = "ai")]
        public IAi Ai => _wrappedEntity.Ai;

        [DataMember(Name = "location")]
        public Point2D Location { get; }

        [DataMember(Name = "maxStats")]
        public IStats MaxStats { get => _wrappedEntity.MaxStats; set => _wrappedEntity.MaxStats = value; }

        [DataMember(Name = "stats")]
        public IStats Stats { get => _wrappedEntity.Stats; set => _wrappedEntity.Stats = value; }

        public IWorld2DEntity WithLocationOffset(Point2D offset) => new World2DEntity(_wrappedEntity, Location + offset);

        internal World2DEntity(IEntity entity, Point2D location)
        {
            Location = location;
            _wrappedEntity = entity;
        }

        IStats IEntity.MaxStats { get => MaxStats; set => MaxStats = value; }
        IReadOnlyStats IReadOnlyEntity.MaxStats => MaxStats;

        IStats IEntity.Stats { get => Stats; set => Stats = value; }
        IReadOnlyStats IReadOnlyEntity.Stats => Stats;

        IHasReadOnlyLocation IHasReadOnlyLocation.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IWorld2DEntity IWorld2DEntity.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
        IReadOnlyWorld2DEntity IReadOnlyWorld2DEntity.WithLocationOffset(Point2D offset) => WithLocationOffset(offset);
    }
}