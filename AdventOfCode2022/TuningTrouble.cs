using System.Diagnostics;
using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 6](https://adventofcode.com/2022/day/6)
/// </summary>
public class TuningTrouble : AdventSolver {
    public override string Day => "day6";

    public override void Solve(string filename) {
        foreach (string game in ReadLines(filename)) {
            Console.WriteLine("Solution for part 1 is: " + FindFirstMarker(game, 4));
            Console.WriteLine("Solution for part 2 is: " + FindFirstMarker(game, 14));
        }
    }

    /// <summary>
    /// Finds the first marker in the data stream where the last numPackets have all been distinct.
    /// The general approach here is to scan backwords once from each packet, when we find a duplicate then 
    /// note its index so the min index is tracked
    /// </summary>
    /// <param name="input">The input text to parse.</param>
    /// <param name="numPackets">The number of distinct packets required.</param>
    /// <returns>The index of the position after the first group of numPackets distinct unique items.</returns>
    private static int FindFirstMarker(string input, int numPackets) {
        int minSolution = numPackets - 1; // -1 because its 0 indexed
        for (int x = 1; x < input.Length; x++) {
            // Scan back through up to num packets to look for duplicates
            for (int y = 1; y < numPackets; y++) {
                if (x - y < 0) continue; // if we have hit the start of the string just continue
                Debug.WriteLine("Comparing " + input[x] + " to " + input[x - y]);
                if (input[x] == input[x - y]) { // if there is a match then move the minSolution ahead
                    minSolution = Math.Max(minSolution, x - y + numPackets);
                    Debug.WriteLine("Found duplicate new minSolution is " + minSolution);
                    break;
                }
            }

            if (x >= minSolution) {
                Debug.WriteLine("Found solution");
                return x + 1;
            }
        }

        return -1;
    }
}