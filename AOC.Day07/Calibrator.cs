using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CSharpItertools;

namespace AOC.Day07;

public class Calibrator
{
    private readonly Itertools _itertools;
    private readonly List<CalibrationEquation> _calibrationEquations;

    public Calibrator(List<CalibrationEquation> calibrationEquations)
    {
        _calibrationEquations = calibrationEquations;
        _itertools = new();
    }

    public List<CalibrationEquation> GetValidEquations(List<Operator> operators)
    {
        ConcurrentBag<CalibrationEquation> validEquations = [];

        Parallel.ForEach(_calibrationEquations, equation =>
                                                {
                                                    IEnumerable<Operator[]> possibleCombinations = _itertools.Product(operators, equation.Values.Count - 1);

                                                    if (possibleCombinations.Any(combination => IsValidCombination(equation, combination)))
                                                        validEquations.Add(equation);
                                                });

        return validEquations.ToList();
    }

    private static bool IsValidCombination(CalibrationEquation equation, Operator[] combination)
    {
        List<long> equationValues = equation.Values;

        long result = ApplyOperation(combination, equationValues);

        return result == equation.Result;
    }

    private static long ApplyOperation(Operator[] combination, List<long> equationValues)
    {
        long result = equationValues[0];

        for (int i = 1; i < equationValues.Count; i++)
        {
            if (combination[i - 1] == Operator.Addition)
                result = Add(result, equationValues[i]);

            if (combination[i - 1] == Operator.Multiplication)
                result = Multiply(result, equationValues[i]);

            if (combination[i - 1] == Operator.Concat)
                result = Concat(result, equationValues[i]);
        }

        return result;
    }

    private static long Add(long addend1, long addend2)
    {
        return addend1 + addend2;
    }

    private static long Multiply(long addend1, long addend2)
    {
        return addend1 * addend2;
    }

    private static long Concat(long number1, long number2)
    {
        return long.Parse($"{number1}{number2}");
    }
}