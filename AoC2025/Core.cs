using AoC2025.Puzzles;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2025;

/// <summary>
/// Represeting common core functionalities.
/// </summary>
public static class Core
{
    /// <summary>
    /// Represents the URL for the Advent of Code 2025 event.
    /// </summary>
    public const string LINK_AOC_2025 = "https://adventofcode.com/2025";

    /// <summary>
    /// Creates a new StringReader for the contents of the specified file.
    /// </summary>
    /// <remarks>The caller is responsible for disposing the returned StringReader when it is no longer
    /// needed. This method reads the entire file into memory; for large files, consider using a streaming approach
    /// instead.</remarks>
    /// <param name="fileName">The path to the file to read. Must refer to an existing file accessible for reading.</param>
    /// <returns>A StringReader containing the entire contents of the specified file, or null if the file is empty.</returns>
    public static StringReader? GetStringReader(string fileName)
    {
        using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                string content = sr.ReadToEnd();
                return new StringReader(content);
            }
        }
    }

    /// <summary>
    /// Runs the specified puzzle by executing both parts and displaying the results to the console.
    /// </summary>
    /// <remarks>This method writes the puzzle title and the results of Part 1 and Part 2 to the standard
    /// output. It is intended for use in console applications where immediate feedback is desired.</remarks>
    /// <param name="puzzle">An object that implements the puzzle logic for both parts. Must not be null.</param>
    /// <param name="inputFile">The path to the input file containing data for the puzzle. Must not be null or empty.</param>
    public static void SolvePuzzle(IPuzzleSolution puzzle, string inputFile)
    {
        Console.WriteLine(puzzle.Title);
        Console.WriteLine(puzzle.Source);
        Console.WriteLine();

        Stopwatch sw = Stopwatch.StartNew();
        long resultPart1 = puzzle.Part1(inputFile);
        long resultPart2 = puzzle.Part2(inputFile);
        sw.Stop();

        // get results formatted in a table
        List<string> lines = [];
        lines.Add($"Part 1: {resultPart1,8}");
        lines.Add($"Part 2: {resultPart2,8}");

        // get the longest line
        int longest = lines.Max(x => x.Length);

        // print results
        Console.WriteLine($"Part    Solution");
        Console.WriteLine(new string('-', longest));

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine(new string('-', longest));
        Console.WriteLine($"Operation completed in {sw.ElapsedMilliseconds} ms");
        return;
    }

    /// <summary>
    /// Gets a read-only mapping of day numbers to their corresponding puzzle solutions.
    /// </summary>
    /// <remarks>
    /// This dictionary is being expanded as the AoC 2025 puzzles are implemented. Each entry maps a day number (int).
    /// </remarks>
    internal static IReadOnlyDictionary<int, IPuzzleSolution> PuzzleByDay => new Dictionary<int, IPuzzleSolution>
    {
        { 0, new Puzzle1() },
        { 1, new Puzzle2() },
    };
}
