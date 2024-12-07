using System.IO;

namespace AOC.Common;

public class Utility
{
    public char[][] GetGrid(string inputFile)
    {
        string[] lines = File.ReadAllLines(inputFile);

        char[][] grid = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
            grid[i] = lines[i].ToCharArray();

        return grid;
    }
}