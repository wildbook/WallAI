using System.Diagnostics.Contracts;

namespace WallAI.Core.Math.Geometry
{
    public readonly struct Circle2D
    {
        public Point2D Origin { get; }

        public Circle2D(Point2D origin, int radius)
        {
            Origin = origin;
            Radius = radius;
            RadiusSquared = Radius * Radius;
        }

        public int Radius { get; }
        private int RadiusSquared { get; }

        public int X => Origin.X;
        public int Y => Origin.Y;

        [Pure]
        public bool ContainsPoint(Point2D point)
        {
            Point2D lp = point - Origin;
            return (lp.X * lp.X) + (lp.Y * lp.Y) < (Radius * Radius);
        }
    }
}
