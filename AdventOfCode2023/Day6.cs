using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 6](https://adventofcode.com/2023/day/6)
/// </summary>
public class Day6 : AdventSolver {
    public override string Day => "day6";

    public override void Solve(string filename) {
        long count1 = 1;

        List<string> allData = [.. ReadLines(filename)];

        var times = allData[0].Split(":")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        var distances = allData[1].Split(":")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        Console.WriteLine(string.Join(",", times));

        for (int x = 0; x < times.Length; x++) {
            count1 *= CalculateNumberWaysToWin(distances[x], times[x]);
        }

        long times2 = long.Parse(allData[0].Split(":")[1].Replace(" ", ""));
        long distances2 = long.Parse(allData[1].Split(":")[1].Replace(" ", ""));


        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + CalculateNumberWaysToWin(distances2, times2));
    }

    public static long CalculateNumberWaysToWin(long requiredDistance, long availableTime) {
        long minTime = 1;

        for (int x = 1; x < availableTime; x++) {
            if ((availableTime - x) * x > requiredDistance) {
                minTime = x;
                break;
            }
        }

        long maxTime = minTime;
        for (long x = availableTime - 1; x > minTime; x--) {
            if ((availableTime - x) * x > requiredDistance) {
                maxTime = x;
                break;
            }
        }

        return maxTime - minTime + 1;
    }


    [TestClass]
    public class Day6Test {

        [TestMethod]
        public void Test1() {
            Assert.AreEqual(4, CalculateNumberWaysToWin(9, 7));
            Assert.AreEqual(8, CalculateNumberWaysToWin(40, 15));
            Assert.AreEqual(9, CalculateNumberWaysToWin(200, 30));
        }
    }
}