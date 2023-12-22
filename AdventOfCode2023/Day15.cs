using System.Security.Cryptography.X509Certificates;
using System.Text;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 15](https://adventofcode.com/2023/day/15)
/// </summary>
public class Day15 : AdventSolver {
    public override string Day => "day15";

    private Dictionary<int, List<Lens>> map = [];

    public override void Solve() {
        int count1 = 0;

        foreach (string game in ReadInputAsIEnumerable()) {
            foreach (string operation in game.Split(",")) {
                count1 += HolidayHash(operation);

                if (operation.EndsWith('-')) {
                    string label = operation.Substring(0, operation.Length - 1);
                    int hash = HolidayHash(label);
                    if (map.TryGetValue(hash, out List<Lens>? box) && box.Count > 0) {
                        for (int x = 0; x < box.Count; x++) {
                            if (box[x].Label == label) {
                                box.RemoveAt(x);
                            }
                        }
                    }
                } else {
                    var split = operation.Split('=');
                    string label = split[0];
                    int length = int.Parse(split[1]);
                    int hash = HolidayHash(label);
                    Lens lens = new (label, length);
                    if (!map.TryGetValue(hash, out List<Lens>? box)) {
                        map[hash] = [lens];
                    } else {
                        var existing = box.Find(l => l.Label == label);
                        if (existing != null) {
                            existing.Length = length;
                        } else {
                            box.Add(lens);
                        }
                    }
                }
            }
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + GetFocusingPower());
    }

    public class Lens(string label, int length) {
        public string Label = label;
        public int Length = length;

        public override string ToString() {
            return string.Format("[{0} {1}]", Label, Length);
        }
    }

    public void Print() {
        for (int x = 0; x < 255; x++) {
            if (map.TryGetValue(x, out List<Lens>? value)) {
                Console.WriteLine("Box {0}: {1}", x, string.Join(" ", value));
            }
        }
    }

    public int GetFocusingPower() {
        int result = 0;
        foreach((int hash, var box) in map.AsEnumerable()) {
            for (int x = 0; x < box.Count; x++) {
                result += (1 + hash) * (1 + x) * box[x].Length;
            }
        }
        return result;
    }


    public static int HolidayHash(string str) {
        int result = 0;
        foreach (var b in Encoding.ASCII.GetBytes(str)) {
            result += b;
            result *= 17;
            result %= 256;
        }
        return result;
    }


    [TestClass]
    public class Day15Test {

        [TestMethod]
        public void Test1() {
            Assert.AreEqual(52, HolidayHash("HASH"));
        }
    }
}