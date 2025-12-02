using System;
using System.IO;

namespace AoC2025.Puzzles;


/// <summary>
/// Provides solutions for Day 2 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 2 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle2 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 2";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/2";

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // problem breakdown:
        // split input into intervals
        // iterate through every number in every interval
        // verify if the number contains a repeating pattern
        // Yes -> count it
        // No -> move on
        // result: sum of counted numbers

        long count = 0;
        long sum = 0;

        using (StringReader? reader = Core.GetStringReader(inputFile))
        {
            if (reader == null)
            {
                return -1;
            }

            // input is a long line of text separated by commas
            string line = reader.ReadLine() ?? string.Empty;
            string[] intervals = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // iterate through every interval
            foreach (string interval in intervals)
            {
                // get the interval bounds
                string[] bounds = interval.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                long start = long.Parse(bounds[0]);
                long end = long.Parse(bounds[1]);

                // iterate through every number in the interval
                for (long number = start; number <= end; number++)
                {
                    // check for repeating pattern
                    string sNumber = number.ToString();
                    if (sNumber.Length % 2 != 0)
                    {
                        // odd length numbers cannot have repeating patterns
                        continue;
                    }

                    int halfLength = sNumber.Length / 2;
                    string half1 = sNumber.Substring(0, halfLength);
                    string half2 = sNumber.Substring(halfLength, halfLength);

                    if (half1 == half2)
                    {
                        // found a repeating pattern
                        count++;
                        sum += number;
                    }
                }
            }
        }

        return sum;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        return 0;
    }
}
