using System;
using System.IO;
using System.Numerics;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 7 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 7 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle8 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 8: Playground";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/8";

    private static double EuclideanDistance(Vector3 v1, Vector3 v2)
    {
        return Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2) + Math.Pow(v1.Z - v2.Z, 2));
    }

    /// <summary>
    /// Helper function to read vectors from the given input <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Input vector value.</param>
    /// <returns>A new object of type <see cref="Vector3"/>.</returns>
    private static Vector3 ReadVector3(string value)
    {
        string[] parts = value.Split(',');
        Vector3 v = new Vector3
        {
            X = float.Parse(parts[0]),
            Y = float.Parse(parts[1]),
            Z = float.Parse(parts[2])
        };

        return v;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // the goal is to connect the closest junction boxes
        // the result is the multiplication of the length of the three largest circuits

        // check if the file exists
        if (File.Exists(inputFile) == false) return -1;

        // TODO implement
        long result = 0;
        return result;
    }


    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        // TODO implement
        long result = 0;
        return result;
    }
}
