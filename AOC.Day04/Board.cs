using System;
using System.Collections.Generic;
using System.Linq;

using AOC.Common;

namespace AOC.Day04;

public class Board
{
    private readonly Dictionary<Direction, Point> _directionMapping;
    private readonly char[][] _grid;
    private int _horizontalBoundary;
    private int _verticalBoundary;

    public Board(char[][] grid)
    {
        _grid = grid;

        _directionMapping = new Dictionary<Direction, Point>
                            {
                                { Direction.North, new Point(-1, 0) },
                                { Direction.NorthEast, new Point(-1, 1) },
                                { Direction.East, new Point(0, 1) },
                                { Direction.SouthEast, new Point(1, 1) },
                                { Direction.South, new Point(1, 0) },
                                { Direction.SouthWest, new Point(1, -1) },
                                { Direction.West, new Point(0, -1) },
                                { Direction.NortWest, new Point(-1, -1) }
                            };
    }

    public int GetWordCountInBoard(char[] wordToFind)
    {
        char crossCharacter = wordToFind[0];

        List<Direction> directionsToSearch = Enum.GetValues<Direction>().ToList();

        List<Point> crossCharacterPositions = GetWordPositions(wordToFind, crossCharacter, directionsToSearch);

        return crossCharacterPositions.Count;
    }

    public int GetCrossedWordCountInBoard(char[] wordToFind, char crossCharacter)
    {
        List<Direction> directionsToSearch =
        [
            Direction.NortWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        ];

        List<Point> crossCharacterPositions = GetWordPositions(wordToFind, crossCharacter, directionsToSearch);

        return crossCharacterPositions
               .CountBy(position => (position.X, position.Y))
               .Count(group => group.Value / 2 > 0);
    }

    private List<Point> GetWordPositions(char[] wordToFind, char crossCharacter, List<Direction> directionsToSearch)
    {
        _verticalBoundary = _grid.Length;

        List<Point> crossCharacterPositions = [];
        for (int y = 0; y < _verticalBoundary; y++)
        {
            _horizontalBoundary = _grid[0].Length;
            for (int x = 0; x < _horizontalBoundary; x++)
            {
                if (_grid[y][x] != wordToFind[0])
                    continue;

                List<Point> crossCharacterLinePositions =
                    SearchDirections(new Point(y, x), wordToFind, crossCharacter, directionsToSearch);

                crossCharacterPositions.AddRange(crossCharacterLinePositions);
            }
        }

        return crossCharacterPositions;
    }

    private List<Point> SearchDirections(
        Point point, char[] wordToFind, char crossCharacter, List<Direction> directionsToSearch)
    {
        List<Point> crossCharacterPositions = [];

        foreach (Direction direction in directionsToSearch)
        {
            Point? foundPosition = SearchDirection(point, direction, wordToFind, crossCharacter);

            if (foundPosition is not null)
                crossCharacterPositions.Add(foundPosition.Value);
        }

        return crossCharacterPositions;
    }

    private Point? SearchDirection(Point point, Direction direction, char[] wordToFind, char crossCharacter)
    {
        Point directionPath = _directionMapping[direction];
        Point newPoint = point;
        Point? characterPosition = null;

        foreach (char currentCharacterToFind in wordToFind)
        {
            if (IsOutOfBound(newPoint))
                return null;

            char gridCharacter = _grid[newPoint.Y][newPoint.X];

            if (gridCharacter != currentCharacterToFind)
                return null;

            if (gridCharacter == crossCharacter)
                characterPosition = newPoint;

            newPoint += directionPath;
        }

        return characterPosition;
    }

    private bool IsOutOfBound(Point newPoint)
    {
        return newPoint.X >= _horizontalBoundary ||
               newPoint.Y >= _verticalBoundary ||
               newPoint.X < 0 ||
               newPoint.Y < 0;
    }
}