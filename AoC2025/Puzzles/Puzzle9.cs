using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2025.Puzzles;

/// <summary>
/// Provides solutions for Day 7 of the puzzle challenge by implementing the required parts as defined by the <see
/// cref="IPuzzleSolution"/> interface.
/// </summary>
/// <remarks>Use this class to execute Part 1 and Part 2 solutions for Day 7 by supplying the puzzle input as a
/// string. Each part returns an integer result based on the provided input.</remarks>
public class Puzzle9 : IPuzzleSolution
{
    /// <summary>
    /// Representing the title of this puzzle.
    /// </summary>
    public string Title => "AoC 2025 Day 9: Movie Theater";

    /// <summary>
    /// Gets the puzzle source.
    /// </summary>
    public string Source => "https://adventofcode.com/2025/day/9";

    private struct Vector2L
    {
        public long X;
        public long Y;

        public Vector2L(long x, long y)
        {
            this.X = x;
            this.Y = y;
            return;
        }
    }

    private static long RectArea(Vector2L p1, Vector2L p2)
    {
        long width = Math.Abs(p2.X - p1.X) + 1;
        long height = Math.Abs(p2.Y - p1.Y) + 1;
        return width * height;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 1 of the problem.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 1. Returns -1 if the input does not produce a valid result.</returns>
    public long Part1(string inputFile)
    {
        // in a grid of tiles (floor), there are red tiles and other colored tiles
        // find the biggest red rectangle - defined by red tiles in it's opposite corners
        // (largest possible red rectangle)
        // input data conains coordinates to red tiles in the [x,y] format

        if (File.Exists(inputFile) == false) return -1;

        long result = 0;

        List<Vector2L> vectors = new List<Vector2L>();
        string[] lines = File.ReadAllLines(inputFile);

        // load vectors from file into a list (of a proper vector data type)
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                Vector2L v = new Vector2L(long.Parse(parts[0]), long.Parse(parts[1]));
                vectors.Add(v);
            }
        }

        for (int i = 0; i < vectors.Count - 1; i++)
        {
            for (int j = i + 1; j < vectors.Count; j++)
            {
                long area = RectArea(vectors[i], vectors[j]);
                result = Math.Max(result, area);
            }
        }

        return result;
    }

    /// <summary>
    /// Checks whether the rectangle defined by its corners (<paramref name="p1"/>, <paramref name="p2"/>) is entirely within the <paramref name="polygon"/>.
    /// </summary>
    /// <returns>Calculated result.</returns>
    private bool IsRectangleInsidePolygon(Vector2L p1, Vector2L p2, List<Vector2L> polygon)
    {
        // normalize the coordinates of the vectors
        long minX = Math.Min(p1.X, p2.X);
        long maxX = Math.Max(p1.X, p2.X);
        long minY = Math.Min(p1.Y, p2.Y);
        long maxY = Math.Max(p1.Y, p2.Y);

        // --- STEP 1: Checking edge intersections ---
        // No edge of the polygon may pass through the INSIDE of the rectangle.
        // Contact with the boundary is permitted.

        int n = polygon.Count;
        for (int i = 0; i < n; i++)
        {
            Vector2L v1 = polygon[i];
            Vector2L v2 = polygon[(i + 1) % n]; // Wrap around the first point

            // is the edge vertical?
            if (v1.X == v2.X)
            {
                long edgeX = v1.X;
                long edgeY1 = Math.Min(v1.Y, v2.Y);
                long edgeY2 = Math.Max(v1.Y, v2.Y);

                // if X is sharply between minX and maxX of the rectangle
                if (edgeX > minX && edgeX < maxX)
                {
                    // A pokud se Y rozsahy překrývají
                    if (Math.Max(minY, edgeY1) < Math.Min(maxY, edgeY2))
                    {
                        return false; // intersection with interior -> invalid
                    }
                }
            }

            // is the edge horizontal?
            else if (v1.Y == v2.Y)
            {
                long edgeY = v1.Y;
                long edgeX1 = Math.Min(v1.X, v2.X);
                long edgeX2 = Math.Max(v1.X, v2.X);

                // if Y is sharply between minY and maxY of the rectangle
                if (edgeY > minY && edgeY < maxY)
                {
                    // and if the X ranges overlap
                    if (Math.Max(minX, edgeX1) < Math.Min(maxX, edgeX2))
                    {
                        return false; // intersection with interior -> invalid
                    }
                }
            }
        }

        // --- STEP 2: Point in Polygon (Ray Casting) ---
        // Even if the edges do not pass through the interior, the rectangle may be entirely OUTSIDE.
        // We will test the center of the rectangle. We use double for the center so that we do not hit the edge.

        double midX = (minX + maxX) / 2.0;
        double midY = (minY + maxY) / 2.0;

        bool inside = false;
        for (int i = 0; i < n; i++)
        {
            Vector2L v1 = polygon[i];
            Vector2L v2 = polygon[(i + 1) % n];

            // Ray casting algorithm (ray to the right)
            // We check whether the edge intersects the horizontal ray from point [midX, midY]
            bool intersect = ((v1.Y > midY) != (v2.Y > midY)) &&
                             (midX < (v2.X - v1.X) * (midY - v1.Y) / (double)(v2.Y - v1.Y) + v1.X);

            if (intersect)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    /// <summary>
    /// Processes the specified input string and returns the result for Part 2 of the challenge.
    /// </summary>
    /// <param name="inputFile">The input data file to be evaluated for Part 2. Cannot be null.</param>
    /// <returns>An integer representing the computed result for Part 2. Returns -1 if the input does not produce a valid result.</returns>
    public long Part2(string inputFile)
    {
        // same as part 1, but with some limitations
        // - see: https://adventofcode.com/2025/day/9#part2 (when unlocked)
        if (!File.Exists(inputFile)) return -1;

        // load the coordinates of the red tiles
        List<Vector2L> polygon = new List<Vector2L>();
        string[] lines = File.ReadAllLines(inputFile);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                polygon.Add(new Vector2L(long.Parse(parts[0]), long.Parse(parts[1])));
            }
        }

        long maxArea = 0;

        // check each pair of coordinates
        // time complexity: O(n^3) but ok for the number of AoC values
        for (int i = 0; i < polygon.Count - 1; i++)
        {
            for (int j = i + 1; j < polygon.Count; j++)
            {
                Vector2L p1 = polygon[i];
                Vector2L p2 = polygon[j];

                // if the new area is smaller than the current one, there's no need to check for it
                long currentArea = RectArea(p1, p2);
                if (currentArea <= maxArea) continue;

                // the actual work: check whether the rectangle is inside the polygon
                if (IsRectangleInsidePolygon(p1, p2, polygon))
                {
                    maxArea = currentArea;
                }
            }
        }

        return maxArea;
    }
}
