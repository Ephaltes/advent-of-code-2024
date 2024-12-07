using System;

using AOC.Common;

namespace AOC.Four;

internal class Program
{
    private static void Main(string[] args)
    {
        Utility utility = new();
        char[][] grid = utility.GetGrid("input");
        SolvePartOne(grid);
        SolvePartTwo(grid);
    }

    private static void SolvePartOne(char[][] grid)
    {
        Board board = new(grid);
        int occurrences = board.GetWordCountInBoard("XMAS".ToCharArray());

        Console.WriteLine($"Solution for part one: {occurrences}");
    }

    private static void SolvePartTwo(char[][] grid)
    {
        Board board = new(grid);
        int occurrences = board.GetCrossedWordCountInBoard("MAS".ToCharArray(), 'A');

        Console.WriteLine($"Solution for part one: {occurrences}");
    }
}