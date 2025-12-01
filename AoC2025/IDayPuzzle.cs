namespace AoC2025;

/// <summary>
/// Defines an interface for solving day puzzles.
/// </summary>
/// <remarks>
/// Representing a contract for classes that implement solutions for daily puzzles. Each puzzle in the Advent of Code has two parts - part 1 and part 2. Each of this parts takes a string input and returns an integer result.
/// </remarks>
public interface IDayPuzzle
{
    /// <summary>
    /// Processes the specified input string and computes the result for Part 1 of the puzzle.
    /// </summary>
    /// <param name="input">The input data to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1 based on the provided input.</returns>
    int Part1(string input);

    /// <summary>
    /// Calculates the result for part two of the puzzle using the specified input string.
    /// </summary>
    /// <param name="input">The input data to be processed for part two. Cannot be null.</param>
    /// <returns>An integer representing the computed result for part two based on the provided input.</returns>
    int Part2(string input);
}