using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Day09;

internal class Program
{
    private static List<int> GetInput()
    {
        char[] input = File.ReadAllText("input")
                           .ToCharArray();

        return input.Select(character => (int)char.GetNumericValue(character)).ToList();
    }

    private static void Main(string[] args)
    {
        List<int> input = GetInput();
        DiskCalculator diskCalculator = new(input);

        SolvePartOne(diskCalculator);
        SolvePartTwo(diskCalculator);
    }

    private static void SolvePartOne(DiskCalculator calculator)
    {
        IReadOnlyCollection<int?> defragmentedDisk = calculator.GetDefragmentedDisk();
        Console.WriteLine($"PartOne answer: {CalculateChecksum(defragmentedDisk)}");
    }

    private static void SolvePartTwo(DiskCalculator calculator)
    {
        IReadOnlyCollection<int?> defragmentedDisk = calculator.GetDefragmentedDiskByBlocks();
        Console.WriteLine($"PartTwo answer: {CalculateChecksum(defragmentedDisk)}");
    }

    private static long CalculateChecksum(IReadOnlyCollection<int?> defragmentedDisk)
    {
        return defragmentedDisk.Select(CheckSumSelector).Sum();
    }

    private static long CheckSumSelector(int? number, int index)
    {
        if (number is null)
            return 0;

        return number.Value * index;
    }
}