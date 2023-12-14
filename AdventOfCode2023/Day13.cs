using System.Transactions;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 13](https://adventofcode.com/2023/day/13)
/// </summary>
public class Day13 : AdventSolver {
    public override string Day => "day13";

    public override void Solve(string filename) {
        int count1 = 0;
        int count2 = 0;

        IEnumerable<IEnumerable<string>> valleys = IEnumerableUtils.ChunkByElement([.. ReadLines(filename)], "");

        foreach (List<string> valley in valleys.Select((str) => str.ToList())) {
            List<string> valleyT = valley.Transpose().Select(s => new String(s.ToArray())).ToList();
            count1 += CalculateReflections(valley, valleyT, 0);
            count2 += CalculateReflections(valley, valleyT, 1);
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    public int CalculateReflections(List<string> valley, List<string> valleyT, int numAllowed) {
        int result = CalculateReflectionRows(valley, numAllowed);
        if (result == 0) {
            return CalculateReflectionRows(valleyT, numAllowed);
        } else {
            return result * 100;
        }
    }

    public int CalculateReflectionRows(List<string> valley, int numAllowed) {
        // In order for there to be a match there must be consecutive duplicates
        // So we start by scanning through pairs to see if they are duplicates, and if they are then keep scanning
        for (int row = 1; row < valley.Count; row++) {
            int invalid = 0;

            for (int offset = 0; row + offset < valley.Count && row - offset - 1 >= 0; offset++) {
                invalid += StringUtils.CountDifferences(valley[row + offset], valley[row - offset - 1]);
                if (invalid > numAllowed) break;
            }

            if (invalid == numAllowed) {
                return row;
            }
        }

        return 0;
    }
}