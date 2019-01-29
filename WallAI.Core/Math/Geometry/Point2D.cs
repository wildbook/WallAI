namespace WallAI.Core.Math.Geometry
{
    public readonly struct Point2D
    {
        public readonly int X;
        public readonly int Y;

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Point2D operator +(Point2D point1, Point2D point2) => new Point2D(point1.X + point2.X, point1.Y + point2.Y);
        public static Point2D operator -(Point2D point1, Point2D point2) => new Point2D(point1.X - point2.X, point1.Y - point2.Y);
    }
}
