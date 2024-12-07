using System;
using System.Collections.Generic;

using AOC.Common;

namespace AOC.Day06;

public class NavigationState : IEquatable<NavigationState>
{
    public static Point DirectionNorth = new(-1, 0);
    public static Point DirectionEast = new(0, 1);
    public static Point DirectionSouth = new(1, 0);
    public static Point DirectionWest = new(0, -1);

    private readonly List<Point> _directions =
    [
        DirectionNorth,
        DirectionEast,
        DirectionSouth,
        DirectionWest
    ];

    private int _currentDirectionIndex;

    public NavigationState(NavigationState navigationState)
    {
        Position = navigationState.Position;
        _currentDirectionIndex = _directions.IndexOf(navigationState.Direction);
    }

    public NavigationState(Point position, Point direction)
    {
        Position = position;
        _currentDirectionIndex = _directions.IndexOf(direction);
    }

    public bool Equals(NavigationState? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Position.Equals(other.Position) &&
               Direction.Equals(other.Direction);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj is NavigationState state && Equals(state);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position.GetHashCode(), Direction.GetHashCode());
    }

    public void ChangeDirectionClockwise()
    {
        _currentDirectionIndex = (_currentDirectionIndex + 1) % _directions.Count;
    }

    public Point Advance()
    {
        return Position += Direction;
    }

    public Point GetNextPosition()
    {
        return Position + Direction;
    }

    public Point Position
    {
        get;
        set;
    }

    public Point Direction => _directions[_currentDirectionIndex];
}