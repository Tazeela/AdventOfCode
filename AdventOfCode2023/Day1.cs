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
            count1 += CalculateCalibrationValue(game, false);
            count2 += CalculateCalibrationValue(game, true);
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    /// <summary>
    /// Calculate the calibration value using the first and last digit in the line.
    /// Edge cases to consider - 
    ///   eightwo = 82
    ///   xxx1xxx = 11
    /// </summary>
    /// <param name="line">The line to parse the calibration from.</param>
    /// <param name="allowSpelled">True if we should include spelled out digits 1-9, false if not.</param>
    /// <returns>The first and last digit, merged into the calibration value.</returns>
    public static int CalculateCalibrationValue(string line, bool allowSpelled) {
        var results = new List<char>();
        int first = 0;
        int last = 0;

        for (int x = 0; x < line.Length; x++) {
            first = ParseDigit(line, x, allowSpelled);
            if (first > 0) break;
        }

        for (int x = line.Length - 1; x >= 0; x--) {
            last = ParseDigit(line, x, allowSpelled);
            if (last > 0) break;
        }

        return (first * 10) + last;
    }

    /// <summary>
    /// Check if the line contains a digit at the offset.
    /// </summary>
    /// <param name="line">The full line of text we are checking.</param>
    /// <param name="offset">The offset in line we are checking.</param>
    /// <param name="allowSpelled">Flag indicating if we support spelled words or not.</param>
    /// <returns>The digit at offset in line, otherwise 0 if no digit can be found.</returns>
    public static int ParseDigit(string line, int offset, bool allowSpelled) {
        if (char.IsDigit(line[offset])) {
            return (int)char.GetNumericValue(line[offset]);
        } else if (allowSpelled) {
            foreach (var kvp in numberMappings) {
                if (StringUtils.IsPartialSubstring(line, kvp.Key, offset)) return kvp.Value;
            }
        }

        return 0;
    }
}