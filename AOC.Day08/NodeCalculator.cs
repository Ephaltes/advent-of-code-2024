using System.Collections.Generic;
using System.Linq;

using AOC.Common;

namespace AOC.Day08;

public class NodeCalculator
{
    private const char EmptyCellSymbol = '.';
    private readonly char[][] _grid;
    private readonly int _gridBoundary;

    public NodeCalculator(char[][] grid)
    {
        _grid = grid;
        _gridBoundary = _grid.GetUpperBound(0);
    }

    public HashSet<Point> CalculateAntiNodes(bool partTwo = false)
    {
        List<Antenna> antennaPositions = GetAntennaPositions();

        List<IGrouping<char, Antenna>> groupedAntennas = antennaPositions.GroupBy(antenna => antenna.Symbol).ToList();

        HashSet<Point> antiNodePositions = [];
        foreach (IGrouping<char, Antenna> groupedAntenna in groupedAntennas.Where(group => group.Count() > 1))
        {
            HashSet<Point> antiNodesForGroup = GetAntiNodesForGroup(groupedAntenna, partTwo);

            antiNodePositions.UnionWith(antiNodesForGroup);
        }

        return antiNodePositions;
    }

    private HashSet<Point> GetAntiNodesForGroup(IGrouping<char, Antenna> groupedAntenna, bool partTwo = false)
    {
        HashSet<Point> antiNodePositions = [];
        foreach (Antenna currentAntenna in groupedAntenna)
        {
            if (partTwo)
                antiNodePositions.Add(currentAntenna.Position);

            IEnumerable<Antenna> otherAntennas = groupedAntenna.Where(group => group != currentAntenna);

            foreach (Antenna otherAntenna in otherAntennas)
            {
                Point distance = currentAntenna.GetDistance(otherAntenna);
                Point invertedDistance = distance.Invert();

                Point antiNodePosition = currentAntenna.Position + invertedDistance;

                if (!IsOutOfBounds(antiNodePosition))
                    antiNodePositions.Add(antiNodePosition);

                if (!partTwo)
                    continue;

                while (true)
                {
                    antiNodePosition += invertedDistance;

                    if (IsOutOfBounds(antiNodePosition))
                        break;

                    antiNodePositions.Add(antiNodePosition);
                }
            }
        }

        return antiNodePositions;
    }

    private List<Antenna> GetAntennaPositions()
    {
        List<Antenna> antennaPositions = [];
        for (int y = 0; y <= _gridBoundary; y++)
        {
            for (int x = 0; x <= _gridBoundary; x++)
            {
                char cellContent = _grid[y][x];
                if (cellContent != EmptyCellSymbol)
                    antennaPositions.Add(new(cellContent, new Point(y, x)));
            }
        }

        return antennaPositions;
    }

    private bool IsOutOfBounds(Point position)
    {
        return position.X < 0 || position.Y < 0 || position.X > _gridBoundary || position.Y > _gridBoundary;
    }
}