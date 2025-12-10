using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 7 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 7 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle10 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 10: Factory";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/10";

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        using (StringReader? reader = Core.GetStringReader(inputFile)) // Tvoje metoda pro čtení
        {
            if (reader == null) return -1;

            long totalPresses = 0;

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 1. Parsing (Your logic, slightly modified for security)
                // Split by characters, discard empty ones
                string[] parts = line.Split(new char[] { '[', ']', '(', ')', '{', '}' },
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                string startupSeq = parts[0];
                string[] stepParts = parts[1..^1];

                // 2. convert to a birtmask
                long targetMask = 0;
                for (int i = 0; i < startupSeq.Length; i++)
                {
                    if (startupSeq[i] == '#')
                    {
                        targetMask |= (1L << i); // set the bit position i to 1
                    }
                }

                // 3. Transfer of buttons to the bitmask list
                List<long> buttons = new List<long>();
                foreach (string step in stepParts)
                {
                    long btnMask = 0;
                    foreach (string bitIndexStr in step.Split(','))
                    {
                        if (int.TryParse(bitIndexStr, out int bitIndex))
                        {
                            btnMask |= (1L << bitIndex);
                        }
                    }
                    buttons.Add(btnMask);
                }

                // 4. Solving one machine and adding to the result
                totalPresses += SolveMachine(targetMask, buttons);
            }

            return totalPresses;
        }
    }

    /// <summary>
    /// Meet-in-the-middle algorithm for finding the smallest number of keystrokes
    /// </summary>
    private long SolveMachine(long target, List<long> buttons)
    {
        int n = buttons.Count;
        int mid = n / 2;

        // We will divide the buttons into left and right halves.
        List<long> leftBtns = buttons.GetRange(0, mid);
        List<long> rightBtns = buttons.GetRange(mid, n - mid);

        // Step A: Generate all possible results for the left side
        // Dictionary: <ResultingState, NumberOfPresses>
        Dictionary<long, int> leftResults = new Dictionary<long, int>();
        GenerateCombinations(leftBtns, 0, 0, 0, leftResults);

        // Step B: We go through the combinations on the right side and look for a match on the left side.
        long minPresses = long.MaxValue;

        // We will use the same recursion, but instead of storing, we will search directly.
        // We need to find RightPart such that: LeftPart ^ RightPart == Target.
        // It follows that: LeftPart = Target ^ RightPart.

        // For simplicity, we will also generate the right side of the sheet (if there are not too many buttons, this is OK).
        Dictionary<long, int> rightResults = new Dictionary<long, int>();
        GenerateCombinations(rightBtns, 0, 0, 0, rightResults);

        foreach (var right in rightResults)
        {
            long rightState = right.Key;
            int rightCount = right.Value;

            long neededLeft = target ^ rightState; // jey XOR operation

            if (leftResults.ContainsKey(neededLeft))
            {
                int total = leftResults[neededLeft] + rightCount;
                if (total < minPresses)
                {
                    minPresses = total;
                }
            }
        }

        return (minPresses == long.MaxValue) ? 0 : minPresses; // 0 if there happens to be no solution
    }

    /// <summary>
    /// Recursive helper method for generating combinations
    /// </summary>
    private void GenerateCombinations(List<long> btns, int index, long currentMask, int count, Dictionary<long, int> results)
    {
        if (index == btns.Count)
        {
            // We save the result. If the same state already exists, we keep the one with fewer presses.
            if (!results.ContainsKey(currentMask))
            {
                results[currentMask] = count;
            }
            else
            {
                results[currentMask] = Math.Min(results[currentMask], count);
            }
            return;
        }

        // Branch 1: I will NOT press the button
        GenerateCombinations(btns, index + 1, currentMask, count, results);

        // Branch 2: I WILL press button (XOR mask and add 1 to count)
        GenerateCombinations(btns, index + 1, currentMask ^ btns[index], count + 1, results);
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        return 0;
    }
}
