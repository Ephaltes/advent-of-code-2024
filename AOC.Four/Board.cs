using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Four;

public class Board
{
    private readonly Dictionary<Direction, Position> _directionMapping;
    private readonly char[][] _grid;
    private int _horizontalBoundary;
    private int _verticalBoundary;

    public Board(char[][] grid)
    {
        _grid = grid;

        _directionMapping = new Dictionary<Direction, Position>
                            {
                                { Direction.North, new Position(-1, 0) },
                                { Direction.NorthEast, new Position(-1, 1) },
                                { Direction.East, new Position(0, 1) },
                                { Direction.SouthEast, new Position(1, 1) },
                                { Direction.South, new Position(1, 0) },
                                { Direction.SouthWest, new Position(1, -1) },
                                { Direction.West, new Position(0, -1) },
                                { Direction.NortWest, new Position(-1, -1) }
                            };
    }

    public int GetWordCountInBoard(char[] wordToFind)
    {
        char crossCharacter = wordToFind[0];

        List<Direction> directionsToSearch = Enum.GetValues<Direction>().ToList();

        List<Position> crossCharacterPositions = GetWordPositions(wordToFind, crossCharacter, directionsToSearch);

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

        List<Position> crossCharacterPositions = GetWordPositions(wordToFind, crossCharacter, directionsToSearch);

        return crossCharacterPositions
               .CountBy(position => (position.X, position.Y))
               .Count(group => group.Value / 2 > 0);
    }

    private List<Position> GetWordPositions(char[] wordToFind, char crossCharacter, List<Direction> directionsToSearch)
    {
        _verticalBoundary = _grid.Length;

        List<Position> crossCharacterPositions = [];
        for (int y = 0; y < _verticalBoundary; y++)
        {
            _horizontalBoundary = _grid[0].Length;
            for (int x = 0; x < _horizontalBoundary; x++)
            {
                if (_grid[y][x] != wordToFind[0])
                    continue;

                List<Position> crossCharacterLinePositions =
                    SearchDirections(new Position(y, x), wordToFind, crossCharacter, directionsToSearch);

                crossCharacterPositions.AddRange(crossCharacterLinePositions);
            }
        }

        return crossCharacterPositions;
    }

    private List<Position> SearchDirections(
        Position position, char[] wordToFind, char crossCharacter, List<Direction> directionsToSearch)
    {
        List<Position> crossCharacterPositions = [];

        foreach (Direction direction in directionsToSearch)
        {
            Position? foundPosition = SearchDirection(position, direction, wordToFind, crossCharacter);

            if (foundPosition is not null)
                crossCharacterPositions.Add(foundPosition.Value);
        }

        return crossCharacterPositions;
    }

    private Position? SearchDirection(Position position, Direction direction, char[] wordToFind, char crossCharacter)
    {
        Position directionPath = _directionMapping[direction];
        Position newPosition = position;
        Position? characterPosition = null;

        foreach (char currentCharacterToFind in wordToFind)
        {
            if (IsOutOfBound(newPosition))
                return null;

            char gridCharacter = _grid[newPosition.Y][newPosition.X];

            if (gridCharacter != currentCharacterToFind)
                return null;

            if (gridCharacter == crossCharacter)
                characterPosition = newPosition;

            newPosition += directionPath;
        }

        return characterPosition;
    }

    private bool IsOutOfBound(Position newPosition)
    {
        return newPosition.X >= _horizontalBoundary ||
               newPosition.Y >= _verticalBoundary ||
               newPosition.X < 0 ||
               newPosition.Y < 0;
    }
}