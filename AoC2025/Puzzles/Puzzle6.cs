using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 6 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 6 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle6 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 6: Trash Compactor";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/6";

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // math task
        // sum or multiply numbers in columns depending on the last line's operator
        // then sum the results and return them
        long total = 0;

        if (File.Exists(inputFile) == false) return -1;

        string[] lines = File.ReadAllLines(inputFile);
        int rows = lines.Length;
        int cols = lines.Max(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
        string[,] grid = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = j < parts.Length ? parts[j] : "";
            }
        }

        // process columns
        for (int col = 0; col < cols; col++)
        {
            string op = grid[rows - 1, col].Trim();
            long subtotal = (op == "*" ? 1 : 0);

            if (op == "*")
            {
                // multiply operation
                for (int x = 0; x < rows - 1; x++)
                {
                    subtotal *= int.Parse(grid[x, col]);
                }
            }

            else
            {
                // addition operation
                for (int x = 0; x < rows - 1; x++)
                {
                    subtotal += int.Parse(grid[x, col]);
                }
            }

            // update total
            total += subtotal;
        }

        return total;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        if (!File.Exists(inputFile)) return -1;

        string[] lines = File.ReadAllLines(inputFile);
        int rows = lines.Length;

        // normalize column widths
        int width = lines.Max(l => l.Length);
        for (int i = 0; i < rows; i++)
        {
            if (lines[i].Length < width)
                lines[i] = lines[i].PadRight(width, ' ');
        }

        // Helper function - Is the column empty?
        bool IsBlankColumn(int c)
        {
            for (int r = 0; r < rows; r++)
                if (lines[r][c] != ' ')
                    return false;
            return true;
        }

        // find the problems separated by empty columns
        var bands = new List<(int start, int end)>();
        int col = 0;
        while (col < width)
        {
            while (col < width && IsBlankColumn(col)) col++;
            if (col >= width) break;

            int start = col;
            while (col < width && !IsBlankColumn(col)) col++;
            int end = col;

            bands.Add((start, end));
        }

        long total = 0;

        // process these problemsright to left
        for (int b = bands.Count - 1; b >= 0; b--)
        {
            var (start, end) = bands[b];

            // the operator is on the last line
            string opStr = lines[rows - 1].Substring(start, end - start).Trim();
            if (string.IsNullOrEmpty(opStr)) continue;
            char op = opStr[0];

            // get the digits - top to bottom
            var numbers = new List<long>();
            for (int c = start; c < end; c++)
            {
                var digits = new List<char>();
                for (int r = 0; r < rows - 1; r++)
                {
                    char ch = lines[r][c];
                    if (char.IsDigit(ch))
                        digits.Add(ch);
                }
                if (digits.Count > 0)
                {
                    string numStr = new string(digits.ToArray());
                    numbers.Add(long.Parse(numStr));
                }
            }

            // apply the operator
            long subtotal = (op == '*') ? 1 : 0;
            foreach (var n in numbers)
            {
                if (op == '*') subtotal *= n;
                else subtotal += n;
            }

            total += subtotal;
        }

        return total;
    }
}
