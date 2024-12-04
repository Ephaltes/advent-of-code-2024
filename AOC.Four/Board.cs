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
        _verticalBoundary = _grid.Length;

        int occurrences = 0;
        for (int y = 0; y < _verticalBoundary; y++)
        {
            _horizontalBoundary = _grid[0].Length;
            for (int x = 0; x < _horizontalBoundary; x++)
                occurrences += GetWordCount(new Position(y, x), wordToFind);
        }

        return occurrences;
    }

    public int GetCrossedWordCountInBoard(char[] wordToFind, char crossCharacter)
    {
        _verticalBoundary = _grid.Length;

        List<Position> crossCharacterPositions = [];
        for (int y = 0; y < _verticalBoundary; y++)
        {
            _horizontalBoundary = _grid[0].Length;
            for (int x = 0; x < _horizontalBoundary; x++)
            {
                List<Position>? crossCharacterLinePositions = GetCrossWordCount(new Position(y, x), wordToFind, crossCharacter);

                if (crossCharacterLinePositions is not null)
                    crossCharacterPositions.AddRange(crossCharacterLinePositions);
            }
        }

        return crossCharacterPositions
               .CountBy(position => (position.X, position.Y))
               .Count(group => group.Value / 2 > 0);
    }

    private List<Position>? GetCrossWordCount(Position position, char[] wordToFind, char crossCharacter)
    {
        if (_grid[position.Y][position.X] != wordToFind[0])
            return null;

        List<Direction> directionsToSearch =
        [
            Direction.NortWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        ];
        return SearchCrossDirections(position, wordToFind, crossCharacter, directionsToSearch);
    }

    private List<Position> SearchCrossDirections(Position position, char[] wordToFind, char crossCharacter, List<Direction> directionsToSearch)
    {
        List<Position> crossCharacterPositions = [];

        foreach (Direction direction in directionsToSearch)
        {
            Position? foundPosition = SearchInCross(position, direction, wordToFind, crossCharacter);

            if (foundPosition is not null)
                crossCharacterPositions.Add(foundPosition.Value);
        }

        return crossCharacterPositions;
    }

    private Position? SearchInCross(Position position, Direction direction, char[] wordToFind, char crossCharacter)
    {
        Position directionPath = _directionMapping[direction];
        Position newPosition = position + directionPath;
        Position? characterPosition = null;

        for (int i = 1; i < wordToFind.Length; i++)
        {
            if (newPosition.X >= _horizontalBoundary ||
                newPosition.Y >= _verticalBoundary ||
                newPosition.X < 0 ||
                newPosition.Y < 0)
                return null;

            char character = _grid[newPosition.Y][newPosition.X];

            if (character != wordToFind[i])
                return null;

            if (character == crossCharacter)
                characterPosition = newPosition;

            newPosition += directionPath;
        }

        return characterPosition;
    }

    private int GetWordCount(Position position, char[] wordToFind)
    {
        if (_grid[position.Y][position.X] != wordToFind[0])
            return 0;

        return SearchInAllDirections(position, wordToFind);
    }

    private int SearchInAllDirections(Position position, char[] wordToFind)
    {
        return Enum.GetValues<Direction>()
                   .Sum(direction => SearchInDirection(position, direction, wordToFind));
    }

    private int SearchInDirection(Position position, Direction direction, char[] wordToFind)
    {
        Position directionPath = _directionMapping[direction];
        Position newPosition = position + directionPath;

        for (int i = 1; i < wordToFind.Length; i++)
        {
            if (newPosition.X >= _horizontalBoundary ||
                newPosition.Y >= _verticalBoundary ||
                newPosition.X < 0 ||
                newPosition.Y < 0)
                return 0;

            if (_grid[newPosition.Y][newPosition.X] != wordToFind[i])
                return 0;

            newPosition += directionPath;
        }

        return 1;
    }
}