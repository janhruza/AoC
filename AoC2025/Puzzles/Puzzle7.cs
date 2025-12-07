using System.Collections.Generic;
using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 7 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 7 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle7 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 7: Laboratories";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/7";

    static long RunBeam(char[,] grid, int startRow, int startCol)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        long splitCount = 0;

        // Používáme HashSet, aby se pozice automaticky deduplikovaly
        // (int r, int c) je C# ValueTuple, funguje v HashSetu správně defaultně
        var currentBeams = new HashSet<(int r, int c)>();

        // Přidáme startovní pozici
        currentBeams.Add((startRow, startCol));

        // Dokud máme nějaké aktivní paprsky
        while (currentBeams.Count > 0)
        {
            var nextBeams = new HashSet<(int r, int c)>();

            foreach (var (r, c) in currentBeams)
            {
                // Pokud jsme vypadli z mapy, ignorujeme
                if (r >= rows || r < 0 || c >= cols || c < 0)
                    continue;

                char currentTile = grid[r, c];

                if (currentTile == '^')
                {
                    // count the split
                    splitCount++;

                    // split happens - two new beams are created and the original disappears
                    nextBeams.Add((r + 1, c - 1));
                    nextBeams.Add((r + 1, c + 1));
                }
                else
                {
                    // 'S' nebo '.' nebo cokoliv jiného -> padá rovně dolů
                    nextBeams.Add((r + 1, c));
                }
            }

            // Přepneme na novou sadu paprsků pro další krok
            currentBeams = nextBeams;
        }

        return splitCount;
    }


    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // beam starts with an 'S' on the first line
        // each beam travels donwards only
        // when the beam lands on the '^' character, it increments the counter and two new beans are spawned (the original beam is discarded) - recursion
        // run until the end of file
        // return the number of splits

        string[] lines = File.ReadAllLines(inputFile);
        int rows = lines.Length;
        int cols = lines[0].Length;
        char[,] grid = new char[rows, cols];

        int startRow = 0;
        int startCol = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                grid[r, c] = lines[r][c];
                if (grid[r, c] == 'S')
                {
                    startRow = r;
                    startCol = c;
                }
            }
        }

        return RunBeam(grid, startRow, startCol);
    }

    /// <summary>
    /// Helper method to merge values.
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <param name="col">Column value.</param>
    /// <param name="count">New state value.</param>
    static void AddTimelines(Dictionary<int, long> dict, int col, long count)
    {
        if (!dict.ContainsKey(col))
        {
            dict[col] = 0;
        }
        dict[col] += count;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        string[] lines = File.ReadAllLines(inputFile);
        int rows = lines.Length;
        int cols = lines[0].Length;

        // Najdeme startovní pozici 'S'
        int startCol = 0;
        for (int c = 0; c < cols; c++)
        {
            if (lines[0][c] == 'S') startCol = c;
        }

        // dictionary: Key = column, Value = number of active timelines at this point
        var activeTimelines = new Dictionary<int, long>();

        // we start at 1 - one timeline/particle
        activeTimelines[startCol] = 1;

        // line by line through the grid (downwards)
        for (int r = 0; r < rows; r++)
        {
            // new dictionary for the row (line)
            var nextTimelines = new Dictionary<int, long>();
            string currentRow = lines[r];

            foreach (var kvp in activeTimelines)
            {
                int c = kvp.Key;        // current column
                long count = kvp.Value; // number of active timelines here

                // bounds check
                if (c < 0 || c >= cols) continue;

                char tile = currentRow[c];

                // logic of the spliting universes
                if (tile == '^')
                {
                    // Quantum splits: all timelines are splitting here
                    // particles continue to the next row left (c-1) and right (c+1)

                    // Add to the left side
                    AddTimelines(nextTimelines, c - 1, count);

                    // Add to the right side
                    AddTimelines(nextTimelines, c + 1, count);
                }
                else
                {
                    // 'S' or '.' -> timelines continue unchanged
                    AddTimelines(nextTimelines, c, count);
                }
            }

            // move in time (overwrite the current state)
            activeTimelines = nextTimelines;
        }

        // at the end we count all the timelines
        long totalTimelines = 0;
        foreach (var count in activeTimelines.Values)
        {
            totalTimelines += count;
        }

        return totalTimelines;
    }
}
