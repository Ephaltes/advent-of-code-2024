using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC.Day02;

internal class Program
{
    private static void Main(string[] args)
    {
        IReadOnlyCollection<Row> rows = GetRows();
        PartOne(rows);
        PartTwo(rows);
    }

    private static void PartOne(IReadOnlyCollection<Row> rows)
    {
        int safeLines = rows.Count(row => IsSafe(row.RowItems));

        Console.WriteLine($"Answer for Part One: {safeLines}");
    }

    private static void PartTwo(IReadOnlyCollection<Row> rows)
    {
        int safeLines = rows.Count(row => IsSafeWithDampener(row.RowItems));

        Console.WriteLine($"Answer for Part Two: {safeLines}");
    }

    private static bool IsSafeWithDampener(LinkedList<int> rowItems)
    {
        LinkedListNode<int> firstRowItem = rowItems.First!;

        bool isSafe = IsRowSafe(firstRowItem);

        if (isSafe)
            return true;

        for (int i = 0; i < rowItems.Count; i++)
        {
            List<int> copiedRowItemsList = rowItems.ToList();
            copiedRowItemsList.RemoveAt(i);

            LinkedList<int> copiedRowItems = new(copiedRowItemsList);

            if (IsRowSafe(copiedRowItems.First!))
                return true;
        }

        return false;
    }

    private static bool IsSafe(LinkedList<int> rowItems)
    {
        LinkedListNode<int> currentColumn = rowItems.First!;

        return IsRowSafe(currentColumn);
    }

    private static bool IsRowSafe(LinkedListNode<int> rowItem)
    {
        bool? wasPreviousPositive = null;
        while (rowItem.Next is not null)
        {
            int difference = rowItem.Value - rowItem.Next.Value;

            if (wasPreviousPositive is not null && wasPreviousPositive != IsPositive(difference))
                return false;

            if (Math.Abs(difference) is > 3 or 0)
                return false;

            wasPreviousPositive ??= IsPositive(difference);
            rowItem = rowItem.Next;
        }

        return true;
    }

    private static bool IsPositive(int number)
    {
        return number >= 0;
    }

    private static IReadOnlyCollection<Row> GetRows()
    {
        List<Row> rows = [];
        foreach (string line in File.ReadLines("input"))
        {
            string[] columnValues = line.Split(' ');

            LinkedList<int> linkedList = new();

            foreach (string columnValue in columnValues)
                linkedList.AddLast(Convert.ToInt32(columnValue));

            rows.Add(new Row(linkedList));
        }

        return rows;
    }
}