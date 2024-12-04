using System;
using System.IO;

namespace AOC.Four;

internal class Program
{
    private static void Main(string[] args)
    {
        char[][] grid = GetGrid();
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
        int occurrences = board.GetCrossedWordCountInBoard("MAS".ToCharArray(),'A');

        Console.WriteLine($"Solution for part one: {occurrences}");
    }

    private static char[][] GetGrid()
    {
        string[] lines = File.ReadAllLines("input");

        char[][] grid = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
            grid[i] = lines[i].ToCharArray();

        return grid;
    }
}