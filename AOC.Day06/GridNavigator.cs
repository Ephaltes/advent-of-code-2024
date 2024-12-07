using System;
using System.Collections.Generic;
using System.Linq;

using AOC.Common;

namespace AOC.Day06;

public class GridNavigator
{
    private const char GuardSymbol = '^';
    private const char ObstacleSymbol = '#';

    private readonly char[][] _grid;

    private readonly int _gridBoundary;

    public GridNavigator(char[][] grid)
    {
        _grid = grid;
        _gridBoundary = _grid.GetUpperBound(0);
    }

    public HashSet<Point> GetVisitedPositions()
    {
        (HashSet<NavigationState> navigationStates, MovementState _) = TravelGrid(FindInitialPosition());

        return navigationStates.Select(navigationState => navigationState.Position)
                               .ToHashSet();
    }

    public List<Point> FindTrapPoints(HashSet<Point> guardPath)
    {
        Point initialPosition = FindInitialPosition();

        List<Point> trapPoints = [];
        foreach (Point point in guardPath)
        {
            (HashSet<NavigationState> _, MovementState movementState) = TravelGrid(initialPosition, point);

            if (movementState is not MovementState.OutOfBounds)
                trapPoints.Add(point);
        }

        return trapPoints;
    }

    private (HashSet<NavigationState>, MovementState) TravelGrid(Point initialPoint, Point? additionalObstacle = null)
    {
        NavigationState currentNavigationState = new(initialPoint, NavigationState.DirectionNorth);
        HashSet<NavigationState> navigationStates = [new(currentNavigationState)];
        MovementState movementState;
        while (true)
        {
            movementState = EvaluateMovementState(currentNavigationState, additionalObstacle);

            if (movementState == MovementState.OutOfBounds)
                break;

            if (movementState == MovementState.Obstacle)
            {
                currentNavigationState.ChangeDirectionClockwise();
            }
            else
            {
                currentNavigationState.Advance();

                if (!navigationStates.Add(new(currentNavigationState)))
                    break;
            }
        }

        return (navigationStates, movementState);
    }

    private MovementState EvaluateMovementState(NavigationState navigationState, Point? additionalObstacle = null)
    {
        Point nextPosition = navigationState.GetNextPosition();

        if (nextPosition == additionalObstacle)
            return MovementState.Obstacle;

        if (IsOutOfBounds(nextPosition))
            return MovementState.OutOfBounds;

        char cellContent = _grid[nextPosition.Y][nextPosition.X];

        return cellContent == ObstacleSymbol ? MovementState.Obstacle : MovementState.Movable;
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