using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Common;

public class Utility
{
    public static char[][] GetGrid(string inputFile)
    {
        string[] lines = File.ReadAllLines(inputFile);

        char[][] grid = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
            grid[i] = lines[i].ToCharArray();

        return grid;
    }

    /// <summary>
    /// It increments like binary numbering meaning:
    /// 000 --> 001 --> 010 --> 011 --> 100 --> 101 --> 110 --> 111
    /// </summary>
    /// <param name="symbols"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    public static IEnumerable<T[]> GetAllCombinations<T>(List<T> symbols, int repeat)
    {
        // Initialize the indices array to track the current combination
        int[] indices = new int[repeat];

        while (true)
        {
            // Generate the current combination based on the indices
            yield return BuildCombination(symbols, indices);

            // Increment the indices to prepare for the next combination
            if (!IncrementIndices(indices, symbols.Count()))
                break; // Exit when all combinations are generated
        }
    }

    private static T[] BuildCombination<T>(List<T> operators, int[] indices)
    {
        // Map the indices to their corresponding operators
        T[] combination = new T[indices.Length];
        for (int i = 0; i < indices.Length; i++)
            combination[i] = operators[indices[i]];

        return combination;
    }

    private static bool IncrementIndices(int[] indices, int operatorCount)
    {
        // Increment indices from right to left (like a multi-digit counter)
        for (int position = indices.Length - 1; position >= 0; position--)
        {
            indices[position]++;

            if (indices[position] < operatorCount)
                return true; // Successfully incremented within bounds

            indices[position] = 0; // Reset the current index and continue
        }

        return false; // All combinations have been exhausted
    }
}