using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 1](https://adventofcode.com/2023/day/1)
/// </summary>
public class Day1 : AdventSolver {
    public override string Day => "day1";

    private static readonly Dictionary<string, char> numberMappings = new Dictionary<string, char>() {
        {"one", '1'},
        {"two", '2'},
        {"three", '3'},
        {"four", '4'},
        {"five", '5'},
        {"six", '6'},
        {"seven", '7'},
        {"eight", '8'},
        {"nine", '9'},
    };

    public override void Solve(string filename) {
        int count1 = 0;
        int count2 = 0;

        foreach (string game in ReadLines(filename)) {
            count1 += calculateCalibrationValue(game, false);
            count2 += calculateCalibrationValue(game, true);
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    /// <summary>
    /// Calculate the calibration value using the first and last digit in the line.
    /// Edge cases to consider - 
    ///   eightwo
    /// </summary>
    /// <param name="line">The line to parse the calibration from.</param>
    /// <param name="allowSpelled">True if we should include spelled out digits 1-9, false if not.</param>
    /// <returns>The first and last digit, merged into the calibration value.</returns>
    public static int calculateCalibrationValue(string line, bool allowSpelled) {
        var results = new List<char>();

        for (int x = 0; x < line.Length; x++) {
            if (char.IsDigit(line[x])) {
                results.Add(line[x]);
            } else if (allowSpelled) {
                string next = line.Substring(x);
                foreach (var kvp in numberMappings) {
                    if (next.StartsWith(kvp.Key)) {
                        results.Add(kvp.Value);
                        break;
                    }
                }
            }
        }

        return int.Parse(results[0].ToString() + results[results.Count - 1].ToString());
    }
}