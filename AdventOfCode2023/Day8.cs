using System.Text.RegularExpressions;
using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 8](https://adventofcode.com/2023/day/8)
/// </summary>
public class Day8 : AdventSolver {
    public override string Day => "day8";

    public override void Solve(string filename) {
        Solver solver = new([.. ReadLines(filename)]);
        Console.WriteLine("Solution for part 1 is: " + solver.GetNumSteps("AAA", (s) => s == "ZZZ"));

        // TODO: This is clearly what they wanted but its only correct for the inputs they provide which 
        // take the same number of steps to loop through z -> z as for a -> z. 
        List<long> counts = [];
        foreach (var str in solver.Maps.Keys) {
            if (str.EndsWith('A')) counts.Add(solver.GetNumSteps(str, (s) => s.EndsWith('Z')));
        }

        Console.WriteLine("Solution for part 2 is: " + IEnumerableUtils.Lcm(counts));
    }

    public class Solver {
        private readonly char[] steps;

        public readonly Dictionary<string, string[]> Maps = [];

        public Solver(List<string> allData) {
            steps = allData[0].ToCharArray();

            for (int x = 2; x < allData.Count; x++) {
                Regex regex = new Regex(@"(\w*) = \((\w*), (\w*)\)");
                var match = regex.Match(allData[x]);
                Maps[match.Groups[1].Value] = [match.Groups[2].Value, match.Groups[3].Value];
            }
        }

        public long GetNumSteps(string current, Func<string, bool> checkDone) {
            long currentStep = 0;
            while (!checkDone(current)) {
                bool right = steps[currentStep % steps.Length] == 'R';
                if (right) {
                    current = Maps[current][1];
                } else {
                    current = Maps[current][0];
                }
                currentStep += 1;
            }

            return currentStep;
        }
    }
}