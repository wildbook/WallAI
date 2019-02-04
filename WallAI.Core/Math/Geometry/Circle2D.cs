using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace WallAI.Core.Math.Geometry
{
    public readonly struct Circle2D
    {
        [DataMember(Name = "origin")]
        public Point2D Origin { get; }

        public Circle2D(Point2D origin, int radius)
        {
            Origin = origin;
            Radius = radius;
            RadiusSquared = Radius * Radius;
        }

        [DataMember(Name = "radius")]
        public int Radius { get; }

        [DataMember(Name = "x")]
        public int X => Origin.X;

        [DataMember(Name = "y")]
        public int Y => Origin.Y;

        private int RadiusSquared { get; }

        [Pure]
        public bool ContainsPoint(Point2D point)
        {
            var lp = point - Origin;
            return (lp.X * lp.X) + (lp.Y * lp.Y) < (RadiusSquared);
        }

        public override string ToString() => $"({Origin}, Ø{RadiusSquared})";
    }
}
