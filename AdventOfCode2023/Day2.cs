using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 2](https://adventofcode.com/2023/day/2)
/// </summary>
public class Day2 : AdventSolver {
    public override string Day => "day2";

    public override void Solve(string filename) {
        int count1 = 0;
        int count2 = 0;

        int line = 0;
        foreach (String game in ReadLines(filename)) {
            line += 1;
            var maxCubes = GetMaxCubesByColor(game);

            var red = maxCubes.GetValueOrDefault("red");
            var green = maxCubes.GetValueOrDefault("green");
            var blue = maxCubes.GetValueOrDefault("blue");

            if (red <= 12 && blue <= 14 && green <= 13) count1 += line;
            count2 += red * blue * green;

        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    /// <summary>
    /// Parse an input line that looks like: `Game 55: 1 blue, 5 green, 3 red; 3 green, 4 red; 6 red, 1 blue, 4 green` and 
    /// find the maximum number of each color pulled at any given time.
    /// </summary>
    /// <param name="line">The line of text to parse.</param>
    /// <returns>A dictionary which contains a mapping of color -> max individual value.</returns>
    private Dictionary<string, int> GetMaxCubesByColor(string line) {
        Dictionary<string, int> maxCubes = new();

        foreach (var pull in line.Split(":")[1].Split(new char[] { ',', ';' }).Select(c => c.Trim())) {
            string[] splitPull = pull.Split(' ');
            int num = int.Parse(splitPull[0]);
            string color = splitPull[1];

            maxCubes[color] = Math.Max(maxCubes.GetValueOrDefault(color, 0), num);
        }

        return maxCubes;
    }
}