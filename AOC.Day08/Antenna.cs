using System;

using AOC.Common;

namespace AOC.Day08;

public struct Antenna : IEquatable<Antenna>
{
    public Antenna(char symbol, Point position)
    {
        Symbol = symbol;
        Position = position;
    }

    public char Symbol
    {
        get;
        set;
    }

    public Point Position
    {
        get;
        set;
    }

    public Point GetDistance(Antenna otherAntenna)
    {
        return otherAntenna.Position - Position;
    }

    public static bool operator ==(Antenna left, Antenna right)
    {
        return left.Symbol == right.Symbol && left.Position == right.Position;
    }

    public static bool operator !=(Antenna left, Antenna right)
    {
        return !(left == right);
    }

    public bool Equals(Antenna other)
    {
        return Symbol == other.Symbol && Position == other.Position;
    }

    public override bool Equals(object? obj)
    {
        return obj is Antenna other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Symbol, Position.GetHashCode());
    }

    public static readonly Antenna Empty;
}