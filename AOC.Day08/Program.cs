using System;
using System.Collections.Generic;

using AOC.Common;

namespace AOC.Day08;

internal class Program
{
    private static void Main(string[] args)
    {
        char[][] grid = Utility.GetGrid("input");
        NodeCalculator nodeCalculator = new(grid);

        SolvePartOne(nodeCalculator);
        SolvePartTwo(nodeCalculator);
    }

    private static void SolvePartOne(NodeCalculator nodeCalculator)
    {
        HashSet<Point> antiNodePositions = nodeCalculator.CalculateAntiNodes();

        Console.WriteLine($"PartOne answer: {antiNodePositions.Count}");
    }

    private static void SolvePartTwo(NodeCalculator nodeCalculator)
    {
        HashSet<Point> antiNodePositions = nodeCalculator.CalculateAntiNodes(true);

        Console.WriteLine($"PartTwo answer: {antiNodePositions.Count}");
    }
}