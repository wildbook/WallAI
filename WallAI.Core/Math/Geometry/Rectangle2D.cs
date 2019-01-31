using System;
using System.Diagnostics.Contracts;

namespace WallAI.Core.Math.Geometry
{
    public readonly struct Rectangle2D : IEquatable<Rectangle2D>
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

        public int X1 => _origin.X;
        public int Y1 => _origin.Y;
        public int X2 => X1 + Width;
        public int Y2 => Y1 + Height;
        public int Width { get; }
        public int Height { get; }

        [Pure] public bool ContainsPoint(Point2D point) => !(point.X < X1) && !(point.Y < Y1) && point.X < X1 + Width && point.Y < Y1 + Height;
        [Pure] public bool IntersectsWith(Rectangle2D rect) => rect.X2 >= X1 && rect.X1 <= X2 && rect.Y2 >= Y1 && rect.Y1 <= Y2;

        [Pure] public override string ToString() => $"({_origin}, {new Point2D(X2, Y2)})";

        [Pure] public static Rectangle2D operator +(Rectangle2D rect, Point2D point) => new Rectangle2D(rect._origin + point, rect._origin + point + new Point2D(rect.Height, rect.Width));
        [Pure] public static Rectangle2D operator -(Rectangle2D rect, Point2D point) => new Rectangle2D(rect._origin - point, rect._origin - point + new Point2D(rect.Height, rect.Width));
        [Pure] public static bool        operator ==(Rectangle2D rect1, Rectangle2D rect2) => rect1.Equals(rect2);
        [Pure] public static bool        operator !=(Rectangle2D rect1, Rectangle2D rect2) => !rect1.Equals(rect2);

        [Pure] public override bool Equals(object obj) => obj is Rectangle2D other && Equals(other);
        [Pure] public          bool Equals(Rectangle2D other) => _origin.Equals(other._origin) && Width == other.Width && Height == other.Height;

        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _origin.GetHashCode();
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                return hashCode;
            }
        }
    }
}
