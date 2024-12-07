using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Day01;

internal static class Program
{
    private static void Main(string[] args)
    {
        IReadOnlyCollection<string> inputList1 = File.ReadAllLines("inputList1");
        IReadOnlyCollection<string> inputList2 = File.ReadAllLines("inputList2");

        List<int> sortedInput1 = inputList1.Select(input => Convert.ToInt32(input)).Order().ToList();
        List<int> sortedInput2 = inputList2.Select(input => Convert.ToInt32(input)).Order().ToList();

        int result1 = 0;

        for (int i = 0; i < sortedInput1.Count; i++)
            result1 += Math.Abs(sortedInput1[i] - sortedInput2[i]);

        Console.WriteLine(result1);
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        List<KeyValuePair<int, int>> groupedInput2 = sortedInput2
                                                     .CountBy(number => number)
                                                     .ToList();

        int result2 = 0;
        foreach (int number in sortedInput1)
        {
            KeyValuePair<int, int> foundPair = groupedInput2.Find(element => element.Key == number);

            result2 += number * foundPair.Value;
        }

        Console.WriteLine(result2);
    }
}