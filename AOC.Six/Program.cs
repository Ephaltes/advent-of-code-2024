using System;

using AOC.Common;

namespace AOC.Six;

internal class Program
{
    private static void Main(string[] args)
    {
        Utility utility = new();

        char[][] grid = utility.GetGrid("input");
        GridNavigator gridNavigator = new(grid);

        SolvePartOne(gridNavigator);
    }

    private static void SolvePartOne(GridNavigator gridNavigator)
    {
        int visitedNodeCount = gridNavigator.CountVisitedPositions();

        Console.WriteLine($"PartOne answer {visitedNodeCount}");
    }
}