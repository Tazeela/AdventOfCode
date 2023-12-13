using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 12](https://adventofcode.com/2023/day/12)
/// </summary>
public class Day12 : AdventSolver {
    public override string Day => "day12";

    public static Dictionary<string, long> cache = new Dictionary<string, long>();

    public override void Solve(string filename) {
        long count1 = 0;
        int count2 = 0;

        foreach (var game in ReadLines(filename).Select(s => ParseLine(s, 5))) {
            count1 += GetNumPermutations(game.Item1, game.Item2);
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    public static long GetNumPermutations(string str, List<int> nums) {
        string cacheKey = str + ":" + string.Join(",", nums);

        if (!cache.ContainsKey(cacheKey)) {
            cache[cacheKey] = CalculateNumPermutations(str, nums);
        }

        return cache[cacheKey];
    }

    public static long CalculateNumPermutations(string str, List<int> nums) {
        var (newStr, foundNums) = Shorten(str);
        if (nums.Count < foundNums.Count || (foundNums.Count < nums.Count && newStr.IndexOf('?') < 0)) {
            return 0;
        } else {

            for(int x = 0; x < foundNums.Count; x++) {
                if(nums[x] != foundNums[x]) return 0;
            }

            if(nums.Count == foundNums.Count && newStr.IndexOf('#') < 0) return 1;

            // check if we can make another then do it
            int indexOfNextWild = newStr.IndexOf('?'); 
            if (indexOfNextWild >= 0) {
                List<int> numsCopy = new(nums);
                foreach(var num in foundNums) numsCopy.Remove(num);
                return GetNumPermutations(StringUtils.ReplaceCharAt(newStr, '.', indexOfNextWild), numsCopy) + GetNumPermutations(StringUtils.ReplaceCharAt(newStr, '#', indexOfNextWild), numsCopy);
            } else {
                return 0; // there are no more wildcards so we cant make the rest of the groups
            }
        }
    }


    // Parse all completed groups before the first ?
    public static (string str, List<int>) Shorten(string str) {
        StringBuilder newString = new();
        StringBuilder current = new();
        List<int> results = [];
        bool ambiguous = false;

        // Find all currently 'done' groups prior to first ?
        for (int x = 0; x < str.Length; x++) {
            if (ambiguous) {
                newString.Append(str[x]);
            } else if (str[x] == '#') {
                current.Append(str[x]);
            } else if (str[x] == '.' && current.Length > 0) {
                results.Add(current.Length);
                newString.Append('.');
                current.Clear();
            } else if (str[x] == '?') {
                newString.Append(current.ToString() + str[x]);
                current.Clear();
                ambiguous = true;
            }
        }

        // if we end with any number of  #'s remove that number as well 
        if (current.Length > 0) results.Add(current.Length);
        return (newString.ToString(), results.ToList());
    }


    public (string, List<int>) ParseLine(string game, int reps) {
        var strs = game.Split(" ");
        var chars =  string.Join('?', Enumerable.Repeat(strs[0], reps));
        var digits = Enumerable.Repeat(strs[1], reps).SelectMany(s => s.Split(',')).Select(s => int.Parse(s)).ToList();

        return (chars, digits);
    }

    [TestClass]
    public class Day12Test {

        [TestMethod]
        public void Test1() {
            Assert.AreEqual(1, GetNumPermutations("???.###",  [1,1,3]));

            Assert.AreEqual(0, GetNumPermutations(".?.?##.",  [1,1,3]));
            
            Assert.AreEqual(4, GetNumPermutations(".??..??...?##.", [1, 1, 3]));
            Assert.AreEqual(1, GetNumPermutations("?#?#?#?#?#?#?#?", [1, 3, 1, 6]));

            Assert.AreEqual(1, GetNumPermutations("????.#...#...", [4, 1, 1]));
            Assert.AreEqual(4, GetNumPermutations("????.######..#####.", [1, 6, 5]));
            Assert.AreEqual(10, GetNumPermutations("?###????????", [3, 2, 1]));
        }
    }
}