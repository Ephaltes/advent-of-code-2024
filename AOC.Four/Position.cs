namespace AOC.Four;

public struct Position
{
    public Position(int y, int x)
    {
        Y = y;
        X = x;
    }

    public static Position operator +(Position left, Position right)
    {
        left.X += right.X;
        left.Y += right.Y;

        return left;
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
}