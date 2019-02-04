using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WallAI.Core.Exceptions;
using WallAI.Core.Helpers.Disposables;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds.Entities;
using WallAI.Core.Worlds.Tiles;

namespace WallAI.Core.Worlds
{
    [DataContract]
    public class PartialWorld2D : IPartialWorld2D
    {
        [DataMember(Name = "seed")]
        public int Seed => _world2D.Seed;

        [DataMember(Name = "maxArea")]
        public Rectangle2D MaxArea { get; }

        [DataMember(Name = "tiles")]
        public IEnumerable<IWorld2DTile2D> Tiles => _world2D.Tiles;

        private readonly IWorld2D _world2D;
        private readonly Point2D _center;
        private readonly Func<Point2D, bool> _isVisible;
        private readonly CountingDisposable _lifetime;

        /// <summary>
        /// Create a new PartialWorld2D.
        /// </summary>
        /// <param name="world2D">
        ///     The base world to which this <see cref="PartialWorld2D"/> will filter access.
        /// </param>
        /// <param name="center">
        ///     All coordinates will be offset by <paramref name="center"/>.
        ///     <remarks>Note: If you do not wish to fake/redefine center, pass <see cref="P:Point2D.Zero"/>.</remarks>
        /// </param>
        /// <param name="visionRect">
        ///     A <see cref="Rectangle2D"/> that will be used as a first pass for whether coordinates are allowed to be accessed.
        ///     <remarks>Note: Applied to local coordinates. Make sure to subtract <paramref name="center"/> if your <paramref name="visionRect"/> is in world-space.</remarks>
        /// </param>
        /// <param name="isVisible">
        ///     If coordinates inside of <paramref name="visionRect"/> are accessed, <paramref name="isVisible"/> will be called to allow more exact filtering.
        ///     <remarks>Note: If you do not want to filter further, pass <code>_ => true</code>.</remarks>
        /// </param>
        /// <param name="lifetime">
        ///     If a <see cref="IDisposable"/> is passed, it will be disposed when this world is disposed.
        ///     This allows the method creating the <see cref="PartialWorld2D"/> to track when it is no longer in use.
        /// </param>
        internal PartialWorld2D(IWorld2D world2D, Point2D center, Rectangle2D visionRect, Func<Point2D, bool> isVisible, IDisposable lifetime = null)
        {
            _world2D = world2D;
            _center = center;
            MaxArea = visionRect;
            _isVisible = isVisible;
            _lifetime = new CountingDisposable(lifetime);
        }

        private bool IsVisible(Point2D point) => MaxArea.ContainsPoint(point) && _isVisible(point + _center);

        public ITile2D this[Point2D point]
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));
                
                if (!IsVisible(point))
                    throw new TileNotVisibleException();
                
                return _world2D[point + _center];
            }
            set
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                if (!IsVisible(point))
                    throw new TileNotVisibleException();

                _world2D[point + _center] = value;
            }
        }

        public IPartialWorld2D CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible) 
            => new PartialWorld2D(this, center, worldRect, isVisible, _lifetime.CreateChild());

        public IEnumerable<IWorld2DTile2D> TilesInRange(Circle2D circle)
            => _world2D.TilesInRange(circle)
                       .Select(x => x.WithLocationOffset(-_center))
                       .Where(x => IsVisible(x.Location));

        public IEnumerable<IWorld2DTile2D> TilesInRect(Rectangle2D rect)
            => _world2D.TilesInRect(rect)
                       .Select(x => x.WithLocationOffset(-_center))
                       .Where(x => IsVisible(x.Location));

        public IEnumerable<IWorld2DEntity> Entities
        {
            get
            {
                _lifetime.ThrowIfDisposed(nameof(PartialWorld2D));

                var entities = _world2D.Entities;
                var visibleEntities = entities.Where(x => _isVisible(x.Location));
                return visibleEntities.ToArray();
            }
        }

        public void Dispose() => _lifetime.Dispose();

        #region Interface implementations
        int IWorld2D.Seed => Seed;

        IEnumerable<IWorld2DTile2D> IWorld2D.Tiles => Tiles;
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.Tiles => Tiles;

        IReadOnlyTile2D IReadOnlyWorld2D.this[Point2D location] => this[location];
        ITile2D IWorld2D.this[Point2D location] { get => this[location]; set => this[location] = value; }
        
        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);
        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRect(Rectangle2D rect) => TilesInRect(rect);

        IEnumerable<IReadOnlyWorld2DTile2D> IReadOnlyWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);
        IEnumerable<IWorld2DTile2D> IWorld2D.TilesInRange(Circle2D circle) => TilesInRange(circle);

        IReadOnlyPartialWorld2D IReadOnlyWorld2D.CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible) => CreateDerivedWorld2D(center, worldRect, isVisible);
        IPartialWorld2D IWorld2D.CreateDerivedWorld2D(Point2D center, Rectangle2D worldRect, Func<Point2D, bool> isVisible) => CreateDerivedWorld2D(center, worldRect, isVisible);

        IEnumerable<IReadOnlyWorld2DEntity> IReadOnlyWorld2D.Entities => Entities;
        IEnumerable<IWorld2DEntity> IWorld2D.Entities => Entities;
        #endregion
    }
}