using System.Data;
using System.Net.Http.Headers;
using AdventLib;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 23](https://adventofcode.com/2023/day/23)
/// </summary>
public class Day23 : AdventSolver {
    public override string Day => "day23";

    private static Dictionary<(int, int, int, int), (int, int, int)> cache = [];

    public override void Solve() {
        var map = ReadInputAsCharArray();

        int[][] steppedOn = new int[map.Length][];
        foreach (var idx in steppedOn.Indexes()) {
            steppedOn[idx] = new int[map[idx].Length];
        }

        Console.WriteLine("Solution for part 1 is: " + NextStep(map, steppedOn, new Step(0, 1, 0, null), false));

        // Replace everything else with .
        foreach (var x in map.Indexes()) {
            foreach (var y in map[x].Indexes()) {
                if (map[x][y] != '#') map[x][y] = '.';
            }
        }

        Console.WriteLine("Solution for part 2 is: " + NextStep(map, steppedOn, new Step(0, 1, 0, null), true));
    }


    public int NextStep(char[][] map, int[][] hike, Step currentStep, bool skipAdjacent = false) {
        int max = 0;

        if (IsValidTile(map, currentStep.Tile) && hike[currentStep.X][currentStep.Y] == 0) {
            hike[currentStep.X][currentStep.Y] = 1;

            if (currentStep.X == map.Length - 1) {
                // destination
                //Console.WriteLine("Found hike length " + currentStep.Steps);
                //PrintHike(map, currentStep);
                //currentStep.PrintPath();
                //Console.WriteLine("----------------");
                max = currentStep.Steps;
            } else {
                int numValid = 0;
                foreach (var diff in new (int, int)[] { (0, 1), (0, -1), (1, 0), (-1, 0) }) {
                    (int, int) testTile = (currentStep.X + diff.Item1, currentStep.Y + diff.Item2);
                    int distance = 1;
                    if (IsValidStep(map, currentStep.Tile, testTile)) {
                        // This speeds up the solution for part 2, but makes the > checks hard since we are jumping ahead
                        // Could make that slower by terminating a node earlier, or just not use it for part 1
                        if (skipAdjacent) {
                            var adjacentStep = GetAdjacentTile(map, (currentStep.X, currentStep.Y), (testTile.Item1, testTile.Item2), 0);
                            testTile = (adjacentStep.Item1, adjacentStep.Item2);
                            distance = adjacentStep.Item3 + 1;
                        }

                        max = Math.Max(max, NextStep(map, hike, new Step(testTile.Item1, testTile.Item2, currentStep.Steps + distance, currentStep), skipAdjacent));
                        numValid++;
                    }
                }
            }

            hike[currentStep.X][currentStep.Y] = 0;
        }

        return max;
    }

    // Just check that the tile is possible to walk on
    public static bool IsValidTile(char[][] map, (int, int) target) {
        return target.Item1 >= 0 && target.Item2 >= 0 && target.Item1 < map.Length && target.Item2 < map[0].Length && map[target.Item1][target.Item2] != '#';
    }

    // Check the movement rules for part 1
    public static bool IsValidStep(char[][] map, (int, int) prev, (int, int) target) {
        bool valid = false;
        if (IsValidTile(map, target)) {
            char mapChar = map[target.Item1][target.Item2];
            if (mapChar == '.') valid = true;
            if (mapChar == '>' && prev.Item2 <= target.Item2) valid = true;
            if (mapChar == '<' && prev.Item2 >= target.Item2) valid = true;
            if (mapChar == '^' && prev.Item1 >= target.Item1) valid = true;
            if (mapChar == 'v' && prev.Item1 <= target.Item1) valid = true;
        }
        return valid;
    }

    /// <summary>
    /// So the data has long paths we are spending a ton of time traversing over and over, so this caches distance between edges.
    /// </summary>
    public static (int, int, int) GetAdjacentTile(char[][] map, (int, int) prev, (int, int) current, int distance) {
        var cacheKey = (prev.Item1, prev.Item2, current.Item1, current.Item2);
        if (!cache.ContainsKey(cacheKey)) {
            List<(int, int)> nextPath = [];

            foreach (var diff in new (int, int)[] { (0, 1), (0, -1), (1, 0), (-1, 0) }) {
                (int, int) testTile = (current.Item1 + diff.Item1, current.Item2 + diff.Item2);
                if (testTile.Item1 == prev.Item1 && testTile.Item2 == prev.Item2) {
                    continue; // dont walk back to the last previous
                } else if (IsValidStep(map, current, testTile)) {
                    nextPath.Add(testTile);
                }
            }

            if (nextPath.Count == 1) {
                // There is only one out node, so we can cache more
                cache[cacheKey] = GetAdjacentTile(map, current, nextPath[0], distance + 1);
            } else {
                cache[cacheKey] = (current.Item1, current.Item2, distance);
            }
        }

        return cache[cacheKey];
    }

    public static void PrintHike(char[][] map, Step final) {
        foreach (var x in map.Indexes()) {
            foreach (var y in map[x].Indexes()) {
                if (final.HasTraversed(x, y)) {
                    Console.Write('0');
                } else {
                    Console.Write(map[x][y]);
                }
            }
            Console.WriteLine();
        }
    }

    public class Step {
        public Step(int X, int Y, int steps, Step? last) {
            this.X = X;
            this.Y = Y;
            this.Steps = steps;
            this.last = last;
        }

        public int X;
        public int Y;
        public int Steps;
        public Step? last;

        public (int, int) Tile {
            get {
                return (X, Y);
            }
        }

        public (int, int) LastStep() {
            if (last == null) return (X, Y);
            return (last.X, last.Y);
        }
        public bool HasTraversed(int x, int y) {
            if (X == x && Y == y) return true;
            if (last != null) return last.HasTraversed(x, y);
            return false;
        }

        public void PrintPath() {
            if (last != null) last.PrintPath();
            Console.WriteLine("{0},{1}", X, Y);
        }
    }
}