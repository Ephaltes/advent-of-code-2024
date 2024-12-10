using System;

namespace AOC.Day09;

public class Block : IEquatable<Block>
{
    public Block(int? value, int startIndex, int length)
    {
        Value = value;
        StartIndex = startIndex;
        Length = length;
    }

    public bool Equals(Block? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Value == other.Value && StartIndex == other.StartIndex && Length == other.Length;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        return Equals((Block)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, StartIndex, Length);
    }

    public int? Value
    {
        get;
        init;
    }

    public int StartIndex
    {
        get;
        init;
    }

    public int Length
    {
        get;
        init;
    }
}