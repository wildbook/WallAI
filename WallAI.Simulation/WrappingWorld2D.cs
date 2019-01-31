using WallAI.Core.Math.Geometry;
using WallAI.Core.Tiles;
using WallAI.Core.Worlds;

namespace WallAI.Simulation
{
    internal class WrappingWorld2D : World2D
    {
        private readonly Point2D _size;

        private static int WrapInt(int i, int max)
        {
            while (i < 0)
                i += max;

            while (i >= max)
                i -= max;

            return i;
        }

        protected override ITile2D this[Point2D location]
        {
            get => base[new Point2D(WrapInt(location.X, _size.X), WrapInt(location.Y, _size.Y))];
            set => base[new Point2D(WrapInt(location.X, _size.X), WrapInt(location.Y, _size.Y))] = value;
        }

        public static IWorld2D Create(IWorld2DMethods methods, Point2D size, int seed) => new WrappingWorld2D(methods, size, seed);
        protected WrappingWorld2D(IWorld2DMethods methods, Point2D size, int seed) : base(methods, seed) => _size = size;
    }
}
