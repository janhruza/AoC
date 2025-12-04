using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 4 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 4 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle4 : IPuzzleSolution
{
    const int MAX = 100;

    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 4";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/4";

    private char GetCharFromArray(char[][] array, int x, int y)
    {
        if (x < 0 || x >= array.Length) return '\0';
        if (y < 0 || y >= array[x].Length) return '\0';
        return array[x][y];
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // read input file
        // go cell after cell anc chek if there's a roll of paper
        // if there is, check the eight directions for other rolls of paper
        // if there are at least 4, this roll cannot be used
        // count all usable rolls of paper and return that number

        // the input is a grid of characters where '@' represents a roll of paper and '.' represents empty space
        char paper = '@';
        char empty = '.';
        long total = 0;

        char[][] inputGrid = File.ReadAllLines(inputFile)
            .Select(line => line.ToCharArray())
            .ToArray();

        for (int row = 0; row < inputGrid.Length; row++)
        {
            for (int col = 0; col < inputGrid[row].Length; col++)
            {
                // process each cell
                if (inputGrid[row][col] == paper)
                {
                    // check all eight directions from here (UP, DOWN, LEFT, RIGHT, NW, NE, SW, SE)
                    char[] chars = new char[8];

                    chars[0] = GetCharFromArray(inputGrid, row - 1, col);
                    chars[1] = GetCharFromArray(inputGrid, row + 1, col);
                    chars[2] = GetCharFromArray(inputGrid, row, col - 1);
                    chars[3] = GetCharFromArray(inputGrid, row, col + 1);
                    chars[4] = GetCharFromArray(inputGrid, row - 1, col - 1);
                    chars[5] = GetCharFromArray(inputGrid, row - 1, col + 1);
                    chars[6] = GetCharFromArray(inputGrid, row + 1, col - 1);
                    chars[7] = GetCharFromArray(inputGrid, row + 1, col + 1);

                    //if (chars.Where(x => x == paper).Count() < 4)
                    //{
                    //    total++;
                    //}

                    int count = 0;
                    foreach (char c in chars)
                    {
                        if (c == paper)
                        {
                            count++;
                        }
                    }

                    if (count < 4) total++;
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
        long total = 0;

        return total;
    }
}
