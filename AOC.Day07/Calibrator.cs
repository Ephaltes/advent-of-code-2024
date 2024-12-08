using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using AOC.Common;

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

        foreach (CalibrationEquation equation in _calibrationEquations)
        {
            //IEnumerable<Operator[]> possibleCombinations = _itertools.Product(operators, equation.Values.Count - 1);

            IEnumerable<Operator[]> possibleCombinations = Utility.GetAllCombinations(operators, equation.Values.Count - 1);

            if (possibleCombinations.Any(combination => IsValidCombination(equation, combination)))
                validEquations.Add(equation);
        }

        return validEquations.ToList();
    }

    /// <summary>
    /// Optimized Solution by https://github.com/sundman/AdventOfCode-24/blob/main/ConsoleApp/Day7.cs
    /// 200ms vs 15s using itertools vs 5s
    /// </summary>
    /// <param name="allowConcat"></param>
    /// <returns></returns>
    public List<CalibrationEquation> GetValidEquationsOptimizedWithFixedOperators(bool allowConcat)
    {
        List<CalibrationEquation> validEquations = [];

        foreach (CalibrationEquation equation in _calibrationEquations)
        {
            List<long> reversedValues = equation.Values.ToList();
            reversedValues.Reverse();

            if (IsValidEquationOptimized(equation.Result, reversedValues, 0, allowConcat))
                validEquations.Add(equation);
        }

        return validEquations;
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

    /// <summary>
    /// This only works because operators will always be evaluated left ro right, not according to precedence rules
    /// and numbers can not be rearranged
    /// </summary>
    /// <param name="currentResult"></param>
    /// <param name="reversedValues"></param>
    /// <param name="index"></param>
    /// <param name="allowConcat"></param>
    /// <returns></returns>
    private static bool IsValidEquationOptimized(
        long currentResult,
        List<long> reversedValues,
        int index,
        bool allowConcat)
    {
        if (index == reversedValues.Count - 1)
            return currentResult == reversedValues[index];

        long currentValue = reversedValues[index];

        if (currentResult > currentValue &&
            IsValidEquationOptimized(currentResult - currentValue, reversedValues, index + 1, allowConcat))
            return true;

        if (currentResult % currentValue == 0 &&
            IsValidEquationOptimized(currentResult / currentValue, reversedValues, index + 1, allowConcat))
            return true;

        if (!allowConcat)
            return false;

        //Check if the remainder is the value we want to concat
        //e.g. currentResult = 12345 and currentValue is 45 --> divideBy = 100 --> remainder of modulo will be 45
        long divideBy = (long)Math.Pow(10, Math.Floor(Math.Log10(currentValue)) + 1);

        return currentResult % divideBy == currentValue &&
               IsValidEquationOptimized(currentResult / divideBy, reversedValues, index + 1, allowConcat);
    }
}