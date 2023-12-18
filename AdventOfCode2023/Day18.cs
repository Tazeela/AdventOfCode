using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 18](https://adventofcode.com/2023/day/18)
/// </summary>
public class Day18 : AdventSolver {
    public override string Day => "day18";

    public override void Solve() {
        long count1 = 0;
        long count2 = 0;

        List<(long, long)> points1 = [(0, 0)];
        List<(long, long)> points2 = [(0, 0)];

        foreach (string game in ReadInputAsIEnumerable()) {
            var split = game.Split(' ');
            var direction1 = split[0];
            var distance1 = int.Parse(split[1]);
            count1 += distance1;
            foreach (var step in GetAllSteps(direction1, distance1, points1.Last())) {
                points1.Add(step);
            }

            var direction2 = split[2][7] switch {
                '0' => "R",
                '1' => "D",
                '2' => "L",
                '3' => "U"
            };
            var distance2 = Convert.ToInt32(split[2].Substring(2, 5), 16);
            
            count2 += distance2;
            foreach (var step in GetAllSteps(direction2, distance2, points2.Last())) {
                points2.Add(step);
            }
        }


        count1 += (long)(InteriorArea(points1) + 1 - (points1.Count / 2));
        count2 += (long)(InteriorArea(points2) + 1 - (points2.Count / 2));
        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }


    static double InteriorArea(List<(long, long)> v) {
        int n = v.Count;
        double a = 0.0;
        for (int i = 0; i < n - 1; i++) {
            a += v[i].Item1 * v[i + 1].Item2 - v[i + 1].Item1 * v[i].Item2;
        }
        return Math.Abs(a + v[n - 1].Item1 * v[0].Item2 - v[0].Item1 * v[n - 1].Item2) / 2.0;
    }

    public IEnumerable<(long, long)> GetAllSteps(string direction, long distance, (long, long) last) {
        switch (direction) {
            case "U":
                for (int x = 1; x <= distance; x++) {
                    yield return (last.Item1 - x, last.Item2);
                }
                break;
            case "D":
                for (int x = 1; x <= distance; x++) {
                    yield return (last.Item1 + x, last.Item2);
                }
                break;
            case "L":
                for (int x = 1; x <= distance; x++) {
                    yield return (last.Item1, last.Item2 - x);
                }
                break;

            case "R":
                for (int x = 1; x <= distance; x++) {
                    yield return (last.Item1, last.Item2 + x);
                }
                break;
        }

    }

    [TestClass]
    public class Day18Test {

        [TestMethod]
        public void Test1() {
        }
    }
}