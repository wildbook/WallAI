using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace WallAI.Core.Math.Geometry
{
    [DataContract]
    public readonly struct Point2D : IEquatable<Point2D>
    {
        [DataMember(Name = "x")]
        public readonly int X;

        [DataMember(Name = "y")]
        public readonly int Y;

        public Point2D(int x, int y) { X = x; Y = y; }

        [Pure] public static Point2D operator  +(Point2D point1, Point2D point2) => new Point2D(point1.X + point2.X, point1.Y + point2.Y);
        [Pure] public static Point2D operator  -(Point2D point1, Point2D point2) => new Point2D(point1.X - point2.X, point1.Y - point2.Y);
        [Pure] public static bool    operator ==(Point2D point1, Point2D point2) =>  point1.Equals(point2);
        [Pure] public static bool    operator !=(Point2D point1, Point2D point2) => !point1.Equals(point2);
        [Pure] public static Point2D operator  /(Point2D point, int divisor)     => new Point2D(point.X / divisor, point.Y / divisor);
        [Pure] public static Point2D operator *(Point2D point, int multiplier) => new Point2D(point.X * multiplier, point.Y * multiplier);
        [Pure] public static Point2D operator -(Point2D point) => new Point2D(-point.X, -point.Y);

        /// <summary>
        /// <remarks>Rounds to nearest whole <see cref="int"/>.</remarks>
        /// </summary>
        /// <param name="point">The point to return a modified copy of.</param>
        /// <param name="divisor">Divisor.</param>
        /// <returns></returns>
        [Pure] public static Point2D operator /(Point2D point, float divisor) => new Point2D((int)((point.X / divisor) + 0.5f), (int)((point.Y / divisor) + 0.5f));

        /// <summary>
        /// <remarks>Rounds to nearest whole <see cref="int"/>.</remarks>
        /// </summary>
        /// <param name="point">The point to return a modified copy of.</param>
        /// <param name="multiplier">Multiplier.</param>
        /// <returns></returns>
        [Pure] public static Point2D operator *(Point2D point, float multiplier) => new Point2D((int)((point.X * multiplier) + 0.5f), (int)((point.Y * multiplier) + 0.5f));

        /// <summary>
        /// A static <see cref="Point2D"/> with <see cref="P:X"/> and <see cref="P:Y"/> set to 0.
        /// </summary>
        [IgnoreDataMember]
        public static Point2D Zero { get; } = new Point2D(0, 0);

        [Pure] public bool Equals(Point2D other) => (X, Y) == (other.X, other.Y);
        [Pure] public override bool Equals(object obj) => obj is Point2D other && Equals(other);
        [Pure] public override int GetHashCode() { unchecked { return (X * 397) ^ Y; } }
        [Pure] public override string ToString() => $"({X}, {Y})";
    }
}
