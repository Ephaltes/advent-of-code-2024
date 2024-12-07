using System;

namespace AOC.Common;

public struct Point : IEquatable<Point>
{
    public Point(int y, int x)
    {
        Y = y;
        X = x;
    }

    public static Point operator +(Point left, Point right)
    {
        left.X += right.X;
        left.Y += right.Y;

        return left;
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }

    public int X
    {
        get;
        private set;
    }

    public int Y
    {
        get;
        private set;
    }

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static readonly Point Empty;
}