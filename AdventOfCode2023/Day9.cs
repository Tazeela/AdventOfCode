using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 9](https://adventofcode.com/2023/day/9)
/// </summary>
public class Day9 : AdventSolver {
    public override string Day => "day9";

    public override void Solve() {
        int count1 = 0;
        int count2 = 0;

        foreach (String game in ReadInputAsList()) {
            count1 += FindNext(game.Split(" ").Select(int.Parse).ToArray());
            count2 += FindNext(game.Split(" ").Select(int.Parse).Reverse().ToArray());
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    public static int FindNext(int[] current) {
        int[] diffs = new int[current.Length - 1];
        for (int x = 1; x < current.Length; x++) {
            diffs[x - 1] = current[x] - current[x - 1];
        }

        if (diffs.All(i => i == diffs.First())) {
            return current.Last() + diffs.First();
        } else {
            return current.Last() + FindNext(diffs);
        }
    }

    [TestClass]
    public class Day9Test {

        [TestMethod]
        public void Test1() {
            Assert.AreEqual(0, FindNext([0, 0, 0]));
            Assert.AreEqual(1, FindNext([1, 1, 1]));
            Assert.AreEqual(4, FindNext([1, 2, 3]));
            Assert.AreEqual(9, FindNext([3, 5, 7]));
        }
    }
}