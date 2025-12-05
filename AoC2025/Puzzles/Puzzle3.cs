using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 3 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 3 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle3 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 3: Lobby";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/3";

    /// <summary>
    /// Constructs the largest possible number by selecting exactly k digits from the input string, preserving their
    /// original order.
    /// </summary>
    /// <remarks>If multiple selections yield the same largest number, the first such occurrence is returned.
    /// The method does not validate that <paramref name="line"/> contains only digit characters; supplying other
    /// characters may result in unexpected behavior.</remarks>
    /// <param name="line">A string consisting of digit characters from which digits are selected to form the largest possible number.</param>
    /// <param name="k">The number of digits to select from the input string. Must be between 1 and the length of <paramref
    /// name="line"/>.</param>
    /// <returns>A string representing the largest possible number that can be formed by selecting k digits from <paramref
    /// name="line"/> in order.</returns>
    private static string LargestPossibleNumber(string line, int k)
    {
        int n = line.Length;
        char[] result = new char[k];
        int start = 0;

        for (int pos = 0; pos < k; pos++)
        {
            // get the remaining characters we must reserve
            int end = n - (k - pos);

            // find the local maximum in range [start, end]
            char maxDigit = '0';
            int maxIndex = start;
            for (int i = start; i <= end; i++)
            {
                if (line[i] > maxDigit)
                {
                    maxDigit = line[i];
                    maxIndex = i;
                }
            }

            result[pos] = maxDigit;
            start = maxIndex + 1;
        }

        return new string(result);
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // read input (each line is a battery bank)
        // process each line
        // get the sum of the two largest numbers -> in order, no sorting
        // add the sum to the total
        // return total

        long total = 0;

        using (StringReader? sr = Core.GetStringReader(inputFile))
        {
            // reader check
            if (sr == null) return -1;

            string line = string.Empty;
            while (sr.Peek() >= 0)
            {
                line = sr.ReadLine() ?? string.Empty;
                string sNumber = LargestPossibleNumber(line, 2);
                long number = long.Parse(sNumber);

                // add the result to the total
                total += number;
            }
        }

        return total;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        long total = 0;

        using (StringReader? sr = Core.GetStringReader(inputFile))
        {
            // reader check
            if (sr == null) return -1;

            string line = string.Empty;
            while (sr.Peek() >= 0)
            {
                line = sr.ReadLine() ?? string.Empty;

                // get the two largest
                string result = LargestPossibleNumber(line, 12);
                long number = long.Parse(result);

                // add the result to the total
                total += number;
            }
        }

        return total;
    }
}
