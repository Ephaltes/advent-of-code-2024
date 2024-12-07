using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Day05;

internal class Program
{
    private const string FileName = "input";

    public static void Main(string[] args)
    {
        (IReadOnlyCollection<Update> updates, IReadOnlyCollection<Rule> rules) = GetInput();
        UpdateProcessor updateProcessor = new(rules);

        List<Update> complaintUpdates = SolvePartOne(updateProcessor, updates);
        SolvePartTwo(updateProcessor, updates, complaintUpdates);
    }

    private static List<Update> SolvePartOne(UpdateProcessor updateProcessor, IReadOnlyCollection<Update> updates)
    {
        List<Update> validUpdates = updateProcessor.GetValidUpdates(updates);

        int sumOfMiddlePages = GetSumOfMiddlePageNumbers(validUpdates);

        Console.WriteLine($"PartOne answer: {sumOfMiddlePages}");

        return validUpdates;
    }

    private static void SolvePartTwo(UpdateProcessor updateProcessor, IReadOnlyCollection<Update> updates, IReadOnlyCollection<Update> validUpdates)
    {
        List<Update> invalidUpdates = updates.Except(validUpdates).ToList();

        List<Update> correctedUpdates = updateProcessor.CorrectInvalidUpdates(invalidUpdates);

        int sumOfMiddlePages = GetSumOfMiddlePageNumbers(correctedUpdates);

        Console.WriteLine($"PartTwo answer: {sumOfMiddlePages}");
    }

    private static int GetSumOfMiddlePageNumbers(IReadOnlyCollection<Update> complaintUpdates)
    {
        return complaintUpdates.Sum(update => update.Pages[update.Pages.Count / 2].Number);
    }

    private static (IReadOnlyCollection<Update>, IReadOnlyCollection<Rule>) GetInput()
    {
        string[] lines = File.ReadAllLines(FileName);

        List<Rule> rules = [];
        List<Update> updates = [];

        bool isParsingRules = true;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isParsingRules = false;

                continue;
            }

            ParseLine(line, isParsingRules, rules, updates);
        }

        return (updates, rules);
    }

    private static void ParseLine(string line, bool isParsingRules, List<Rule> rules, List<Update> updates)
    {
        int[] numbers = line.Split(isParsingRules ? '|' : ',')
                            .Select(int.Parse)
                            .ToArray();

        if (isParsingRules)
        {
            rules.Add(new Rule(numbers[0], numbers[1]));
        }
        else
        {
            List<Page> pages = numbers.Select(num => new Page(num)).ToList();
            updates.Add(new Update(pages));
        }
    }
}