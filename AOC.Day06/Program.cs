using System;
using System.Collections.Generic;

using AOC.Common;

namespace AOC.Day06;

internal class Program
{
    private static void Main(string[] args)
    {
        char[][] grid = Utility.GetGrid("input");
        GridNavigator gridNavigator = new(grid);

        HashSet<Point> guardPath = SolvePartOne(gridNavigator);
        SolvePartTwo(gridNavigator, guardPath);
    }

    private static HashSet<Point> SolvePartOne(GridNavigator gridNavigator)
    {
        HashSet<Point> visitedPositions = gridNavigator.GetVisitedPositions();

        Console.WriteLine($"PartOne answer {visitedPositions.Count}");

        return visitedPositions;
    }

    private static void SolvePartTwo(GridNavigator gridNavigator, HashSet<Point> guardPath)
    {
        List<Point> positionsToTrapGuard = gridNavigator.FindTrapPoints(guardPath);

        Console.WriteLine($"PartTwo answer {positionsToTrapGuard.Count}");
    }
}