using System;
using System.IO;

namespace AoC2025;

/// <summary>
/// Representing the main program for Advent of Code 2025.
/// </summary>
internal class Program
{
    /// <summary>
    /// Serves as the entry point for the application.
    /// </summary>
    /// <param name="args">An array of command-line arguments supplied to the application. Each element represents a single argument as a
    /// string.</param>
    /// <returns>An integer value that is returned to the operating system upon application exit. Typically, a return value of 0
    /// indicates successful execution.</returns>
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            // select puzzle interactively
            Console.WriteLine("Advent of Code 2025 Solver");
            Console.WriteLine(Core.LINK_AOC_2025);
            Console.WriteLine();

            Console.Write("Enter input file path: ");
            string inputFile = Console.ReadLine() ?? string.Empty;

            // validate input file
            if (File.Exists(inputFile) == false)
            {
                Console.Error.WriteLine("Error: input file does not exist.");
                return -1;
            }

            // select day
            Console.Write("Enter day number: ");
            string dayInput = Console.ReadLine() ?? string.Empty;

            // validate day
            if (int.TryParse(dayInput, out int day) == false)
            {
                Console.Error.WriteLine("Error: day must be an integer.");
                return -1;
            }

            Console.WriteLine();

            // run puzzle for the specified day with the specified input file
            IPuzzleSolution puzzle = Core.PuzzleByDay[day - 1];
            Core.SolvePuzzle(puzzle, inputFile);
            return 0;
        }

        else if (args.Length != 2)
        {
            Console.Error.WriteLine("AoC 2025 solver\nUsage: AoC2025 [inputFile: string] [day: int]");
            return -1;
        }

        else
        {
            string inputFile = args[0];

            if (int.TryParse(args[1], out int day) == false)
            {
                Console.Error.WriteLine("Error: day must be an integer.");
                return -1;
            }

            int idx = day - 1;

            if (Core.PuzzleByDay.ContainsKey(idx) == false)
            {
                Console.Error.WriteLine($"Error: no puzzle found for day {day}.");
                return -1;
            }

            // get the puzzle for the specified day
            IPuzzleSolution puzzle = Core.PuzzleByDay[idx];

            // run the specified puzzle
            Core.SolvePuzzle(puzzle, inputFile);
            return 0;
        }
    }
}
