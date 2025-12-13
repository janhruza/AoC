using System.Collections.Generic;
using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 11 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 11 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle11 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 11: Reactor";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/11";

    static List<List<string>> FindAllPaths(Dictionary<string, List<string>> graph, string start, string end)
    {
        var result = new List<List<string>>();
        var currentPath = new List<string>();
        DFS(graph, start, end, currentPath, result);
        return result;
    }

    static void DFS(Dictionary<string, List<string>> graph, string current, string end,
                    List<string> currentPath, List<List<string>> result)
    {
        currentPath.Add(current);

        if (current == end)
        {
            // našli jsme cestu
            result.Add(new List<string>(currentPath));
        }
        else if (graph.ContainsKey(current))
        {
            foreach (var neighbor in graph[current])
            {
                // rekurzivně pokračujeme
                DFS(graph, neighbor, end, currentPath, result);
            }
        }

        // backtracking – odstraníme poslední uzel
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // input is a list of devices and their outputs in format: input_device: output1 output2 ...
        // the goal is to count the number of possible paths to get from device 'you' to 'out'

        if (File.Exists(inputFile) == false) return -1;

        // load the input
        Dictionary<string, List<string>> inputData = [];
        using (StringReader? reader = Core.GetStringReader(inputFile))
        {
            if (reader == null) return -2;

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(new char[] { ':', ' ' });
                string key = parts[0];
                string[] devices = parts[1..];

                inputData.Add(key, [.. devices]);
            }
        }

        var result = FindAllPaths(inputData, "you", "out");
        long total = result.Count;
        return total;
    }

    static Dictionary<string, long> memo = new Dictionary<string, long>();

    static long CountPaths(Dictionary<string, List<string>> graph, string current, string end, HashSet<string> visited)
    {
        if (current == end) return 1;
        if (memo.ContainsKey(current)) return memo[current];

        long total = 0;
        if (graph.ContainsKey(current))
        {
            foreach (var neighbor in graph[current])
            {
                if (!visited.Contains(neighbor)) // ochrana proti cyklům
                {
                    visited.Add(neighbor);
                    total += CountPaths(graph, neighbor, end, visited);
                    visited.Remove(neighbor);
                }
            }
        }

        memo[current] = total;
        return total;
    }

    static long CountValidPathsPart2(Dictionary<string, List<string>> graph, string current, string end, HashSet<string> visited, bool foundDac, bool foundFft)
    {
        // 1. Kontrola cíle
        if (current == end)
        {
            // Cesta je platná, POUZE POKUD jsme dorazili do cíle 
            // A navštívili oba povinné uzly.
            return (foundDac && foundFft) ? 1 : 0;
        }

        // 2. Aktualizace stavu povinných uzlů
        bool nextFoundDac = foundDac || current == "dac";
        bool nextFoundFft = foundFft || current == "fft";

        long total = 0;

        // 3. Rekurzivní prohledávání sousedů
        if (graph.ContainsKey(current))
        {
            foreach (var neighbor in graph[current])
            {
                // Ochrana proti cyklům: Lze navštívit pouze uzel, který ještě nebyl na této cestě.
                if (!visited.Contains(neighbor))
                {
                    // A. Přidání souseda do navštívených pro tuto větev
                    visited.Add(neighbor);

                    // B. Rekurzivní volání s novým stavem
                    // Používáme nextFoundDac/Fft, protože je chceme započítat
                    // POUZE při průchodu uzlem.
                    total += CountValidPathsPart2(graph, neighbor, end, visited, nextFoundDac, nextFoundFft);

                    // C. Backtracking: Odstranění souseda po dokončení větve
                    visited.Remove(neighbor);
                }
            }
        }

        return total;
    }

    // Funkce by potřebovala složitější klíč, aby zohlednila stav dac/fft
    // Klíč by mohl být např. "uzel-dacStav-fftStav"
    static Dictionary<string, long> memoPart2 = new Dictionary<string, long>();

    static long CountPathsWithState(Dictionary<string, List<string>> graph, string current, string end, bool foundDac, bool foundFft)
    {
        // Nový klíč memoizace
        string key = $"{current}-{foundDac}-{foundFft}";
        if (memoPart2.ContainsKey(key)) return memoPart2[key];

        if (current == end) return (foundDac && foundFft) ? 1 : 0;

        // Aktualizace stavu
        bool nextFoundDac = foundDac || current == "dac";
        bool nextFoundFft = foundFft || current == "fft";

        long total = 0;
        if (graph.ContainsKey(current))
        {
            foreach (var neighbor in graph[current])
            {
                // POZOR: Tato verze NEZAJIŠŤUJE JEDNODUCHÉ CESTY!
                // Zacyklí se, pokud v grafu existují cykly.
                total += CountPathsWithState(graph, neighbor, end, nextFoundDac, nextFoundFft);
            }
        }

        memoPart2[key] = total;
        return total;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        // same as part 1 but must go from 'svr' to 'out' through 'dac' and 'fft'
        if (File.Exists(inputFile) == false) return -1;

        // load the input
        Dictionary<string, List<string>> inputData = [];
        using (StringReader? reader = Core.GetStringReader(inputFile))
        {
            if (reader == null) return -2;

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(new char[] { ':', ' ' });
                string key = parts[0];
                string[] devices = parts[1..];

                inputData.Add(key, [.. devices]);
            }
        }

        //long total = CountPaths(inputData, "svr", "out", new HashSet<string>());

        HashSet<string> visited = [];
        //long total = CountValidPathsPart2(inputData, "svr", "out", visited, false, false);
        long total = CountPathsWithState(inputData, "svr", "out", false, false);
        return total;
    }
}
