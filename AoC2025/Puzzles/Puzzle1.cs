using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 1 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 1 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle1 : IPuzzleSolution
{
    const int MAX = 100;

    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 1: Secret Entrance";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/1";

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        if (File.Exists(inputFile) == false)
        {
            return -1;
        }

        var reader = Core.GetStringReader(inputFile);
        if (reader == null)
        {
            return -1;
        }

        long count = 0;
        long dial = 50;
        while (reader.Peek() >= 0)
        {
            string? line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            int num = int.Parse(line.Substring(1));

            // gets the direction and distance
            if (line[0] == 'L')
            {
                dial -= num;
            }

            else
            {
                dial += num;
            }

            if (dial % MAX == 0)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        if (File.Exists(inputFile) == false)
        {
            return -1;
        }

        var reader = Core.GetStringReader(inputFile);
        if (reader == null)
        {
            return -1;
        }

        long count = 0;
        long dial = 50;

        while (reader.Peek() >= 0)
        {
            string? line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            int num = int.Parse(line.Substring(1));
            count += num / MAX;

            int remainder = num % MAX;

            if (remainder > 0)
            {
                if (line[0] == 'L')
                {
                    if (dial > 0 && (dial - remainder <= 0))
                    {
                        count++;
                    }

                    dial = (dial - remainder) % MAX;
                    if (dial < 0) dial += MAX;
                }
                else
                {
                    if (dial + remainder >= MAX)
                    {
                        count++;
                    }

                    dial = (dial + remainder) % MAX;
                }
            }
        }

        return count;
    }

}
