using System.Diagnostics.Contracts;

namespace WallAI.Core.Math.Geometry
{
    public readonly struct Rectangle2D
    {
        private readonly Point2D _origin;

        public Rectangle2D(Point2D first, Point2D second)
        {
            _origin = new Point2D(System.Math.Min(first.X, second.X), System.Math.Min(first.Y, second.Y));
            Width = System.Math.Abs(first.X - second.X) + 1;
            Height = System.Math.Abs(first.Y - second.Y) + 1;
        }

        public Rectangle2D(Point2D origin, int width, int height)
        {
            _origin = origin;
            Width = width;
            Height = height;
        }

        public int X => _origin.X;
        public int Y => _origin.Y;
        public int X2 => X + Width;
        public int Y2 => Y + Height;
        public int Width { get; }
        public int Height { get; }

        [Pure]
        public bool ContainsPoint(Point2D point)
            => point.X >= X &&
               point.Y >= Y &&
               point.X < X + Width && 
               point.Y < Y + Height;
    }
}
