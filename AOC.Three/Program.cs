using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC.Three;

internal partial class Program
{
    private static void Main(string[] args)
    {
        string input = File.ReadAllText("input");
        PartOne(input);
        PartTwo(input);
    }

    private static void PartOne(string input)
    {
        Regex findMulRegex = FindMulRegex();

        MatchCollection matchesCollection = findMulRegex.Matches(input);

        int result = SumMulValues(matchesCollection);

        Console.WriteLine($"Solution for Part One: {result}");
    }

    private static int SumMulValues(MatchCollection matchesCollection)
    {
        int result = 0;

        foreach (Match match in matchesCollection)
            result += Convert.ToInt32(match.Groups[1].Value) * Convert.ToInt32(match.Groups[2].Value);

        return result;
    }

    private static void PartTwo(string input)
    {
        Regex enabledTextPartRegex = GetEnabledTextPartRegex();

        MatchCollection enabledTextPartCollection = enabledTextPartRegex.Matches(input);

        StringBuilder enabledTextStringBuilder = new();

        foreach (Match enabledTextPart in enabledTextPartCollection)
            enabledTextStringBuilder.Append(enabledTextPart.Value);

        Regex findMulRegex = FindMulRegex();

        MatchCollection matchesCollection = findMulRegex.Matches(enabledTextStringBuilder.ToString());

        int result = SumMulValues(matchesCollection);

        Console.WriteLine($"Solution for Part One: {result}");
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex FindMulRegex();

    [GeneratedRegex(@"^.*?(?=don't\(\))|(?<=do\(\)).*?(?=don't\(\))", RegexOptions.Singleline)]
    private static partial Regex GetEnabledTextPartRegex();
}