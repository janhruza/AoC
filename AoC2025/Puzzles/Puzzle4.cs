using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Pipelines;
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

    private long ProcessGrid(char[][] input)
    {
        char paper = '@';
        char empty = '.';
        long removedTotal = 0;

        while (true)
        {
            List<(int r, int c)> toRemove = new List<(int, int)>();

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == paper)
                    {
                        int count = 0;
                        
                        // directions
                        int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
                        int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };

                        for (int k = 0; k < 8; k++)
                        {
                            int nr = row + dRow[k];
                            int nc = col + dCol[k];

                            if (nr >= 0 && nr < input.Length &&
                                nc >= 0 && nc < input[nr].Length &&
                                input[nr][nc] == paper)
                            {
                                count++;
                                if (count >= 4) break; // early exit
                            }
                        }

                        if (count < 4)
                            toRemove.Add((row, col));
                    }
                }
            }

            if (toRemove.Count == 0) break; // nothing to remove

            foreach (var (r, c) in toRemove)
                input[r][c] = empty;

            removedTotal += toRemove.Count;
        }

        return removedTotal;
    }


    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        // same as part 1 but now we can remove those that match criteria,
        // so in the loop we count the number of removed blocks of paper until this number is 0 and then
        // we return the number of paper blocks
        long total = 0;

        char[][] inputGrid = File.ReadAllLines(inputFile)
            .Select(line => line.ToCharArray())
            .ToArray();

        long removed = -1;

        do
        {
            removed = ProcessGrid(inputGrid);
            total += removed;
        }
        while (removed > 0);

        return total;
    }
}
