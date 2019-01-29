using System;

namespace WallAI.Core.Internal
{
    public struct Coordinate2D : IEquatable<Coordinate2D>
    {
        public readonly int X;
        public readonly int Y;

        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Coordinate2D other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object obj) => obj is Coordinate2D other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}