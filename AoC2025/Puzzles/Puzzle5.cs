using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 5 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 5 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle5 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 5";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/5";

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // read the input file
        // ranges and ingredients are separated by an empty line

        // this example shows how using HashSet and storing ranges instead of each number separately is much faster

        // number of fresh ingredients found in the ranges
        long total = 0;

        using (StringReader? reader = Core.GetStringReader(inputFile))
        {
            if (reader == null) return -1;

            string? line;
            HashSet<(long, long)> freshIds = [];

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                string [] parts = line.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);

                freshIds.Add((start, end));
            }

            while ((line = reader.ReadLine()) != null)
            {
                long id = long.Parse(line);
                foreach (var (s, e) in freshIds)
                {
                    if (id >= s && id <= e)
                    {
                        total++;
                        break;
                    }
                }
            }
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
        // now we count only the valid ingredients - we don't need the second part of the input (the individual ids)
        // we only want the number of unique ids among the ranges

        long total = 0;
        using (StringReader? reader = Core.GetStringReader(inputFile))
        {
            if (reader == null) return -1;

            List<(long, long)> ranges = [];
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) break;

                string[] parts = line.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);

                ranges.Add((start, end));
            }

            // get only the unique numbers
            ranges.Sort();
            long currentStart = ranges[0].Item1;
            long currentEnd = ranges[0].Item2;

            foreach (var (s, e) in ranges)
            {
                if (s <= currentEnd + 1)
                {
                    currentEnd = Math.Max(currentEnd, e);
                }

                else
                {
                    total += currentEnd - currentStart + 1;
                    currentStart = s;
                    currentEnd = e;
                }
            }

            total += currentEnd - currentStart + 1;
        }

        return total;
    }
}
