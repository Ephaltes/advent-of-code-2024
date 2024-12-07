using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Day07;

internal class Program
{
    private static void Main(string[] args)
    {
        List<CalibrationEquation> calibrationEquations = GetEquations();
        Calibrator calibrator = new(calibrationEquations);

        SolvePartOne(calibrator);
        SolvePartTwo(calibrator);
    }

    private static void SolvePartOne(Calibrator calibrator)
    {
        List<CalibrationEquation> validEquations = calibrator.GetValidEquations([Operator.Addition, Operator.Multiplication]);

        long sum = validEquations.Select(equation => equation.Result).Sum();

        Console.WriteLine($"Solution PartOne: {sum}");
    }

    private static void SolvePartTwo(Calibrator calibrator)
    {
        List<CalibrationEquation> validEquations = calibrator.GetValidEquations([Operator.Addition, Operator.Multiplication, Operator.Concat]);

        long sum = validEquations.Select(equation => equation.Result).Sum();

        Console.WriteLine($"Solution PartTwo: {sum}");
    }

    private static List<CalibrationEquation> GetEquations()
    {
        string[] lines = File.ReadAllLines("input");

        List<CalibrationEquation> equations = [];
        foreach (string line in lines)
        {
            string[] equationSplit = line.Split(": ");

            long equationResult = long.Parse(equationSplit[0]);
            long[] equationValues = Array.ConvertAll(equationSplit[1].Split(' '), long.Parse);

            equations.Add(new CalibrationEquation(equationResult, equationValues.ToList()));
        }

        return equations;
    }
}