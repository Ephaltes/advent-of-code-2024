using System;
using System.Collections.Generic;

using AOC.Common;

namespace AOC.Day06;

public class GridNavigator
{
    private const char GuardSymbol = '^';
    private const char ObstacleSymbol = '#';

    private readonly char[][] _grid;

    private readonly Point[] _directions =
    [
        new(-1, 0), // North
        new(0, 1), // East
        new(1, 0), // South
        new(0, -1) // West
    ];

    private readonly HashSet<Point> _visitedPositions;

    private readonly int _gridBoundary;

    private Point _currentPositionOnGrid;
    private int _currentDirectionIndex;

    public GridNavigator(char[][] grid)
    {
        _grid = grid;
        _gridBoundary = _grid.GetUpperBound(0);

        _currentPositionOnGrid = FindInitialPosition();

        _visitedPositions =
        [
            _currentPositionOnGrid
        ];
    }

    public int CountVisitedPositions()
    {
        while (true)
        {
            MovementState movementState = EvaluateMovementState();

            if (movementState == MovementState.OutOfBounds)
                break;

            if (movementState == MovementState.Obstacle)
                ChangeDirectionClockwise();
            else
                AdvancePosition();
        }

        return _visitedPositions.Count;
    }

    private MovementState EvaluateMovementState()
    {
        Point nextPosition = _currentPositionOnGrid + _directions[_currentDirectionIndex];

        if (IsOutOfBounds(nextPosition))
            return MovementState.OutOfBounds;

        char cellContent = _grid[nextPosition.Y][nextPosition.X];

        return cellContent == ObstacleSymbol ? MovementState.Obstacle : MovementState.Movable;
    }

    private void AdvancePosition()
    {
        _currentPositionOnGrid += _directions[_currentDirectionIndex];

        _visitedPositions.Add(_currentPositionOnGrid);
    }

    private void ChangeDirectionClockwise()
    {
        _currentDirectionIndex = (_currentDirectionIndex + 1) % _directions.Length;
    }

    private Point FindInitialPosition()
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            int column = Array.IndexOf(_grid[row], GuardSymbol);

            if (column != -1)
                return new Point(row, column);
        }

        return Point.Empty;
    }

    private bool IsOutOfBounds(Point position)
    {
        return position.X < 0 || position.Y < 0 || position.X > _gridBoundary || position.Y > _gridBoundary;
    }
}