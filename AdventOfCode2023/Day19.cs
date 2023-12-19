using System.Data;
using System.Text.RegularExpressions;
using AdventLib;
using Range = AdventLib.Common.Range;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AdventOfCode2023.Day19.Rule;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 19](https://adventofcode.com/2023/day/19)
/// </summary>
public class Day19 : AdventSolver {
    public override string Day => "day19";

    public override void Solve() {
        var chunks = ReadInputChunkByElement("").ToArray();
        var workflows = ParseWorkflows(chunks[0]);
        var validRanges = FindAllRanges(workflows, XmasRange.All(), "in", "").ToArray();
        var part1Ranges = chunks[1].Select(ParseXmasRange);

        Console.WriteLine("Valid ranges are:");
        foreach(var range in validRanges) {
            Console.WriteLine(range);
        }
        
        Console.WriteLine("Solution for part 1 is: " + part1Ranges.Where(inputRange => validRanges.Any(r => inputRange.IsSubset(r))).Select(r => r.GetPart1Score()).Sum());
        Console.WriteLine("Solution for part 2 is: " + validRanges.Select(r => r.GetPart2Score()).Sum());
    }

    /// <summary>
    /// Perform a depth first search of the workflows to determine which ranges are valid.
    /// </summary>
    /// <param name="workflows">The set of workflows.</param>
    /// <param name="currentRange">The range of current possible values.</param>
    /// <param name="workflow"></param>
    /// <param name="history"></param>
    /// <returns></returns>
    public IEnumerable<XmasRange> FindAllRanges(Dictionary<string, List<Rule>> workflows, XmasRange currentRange, string currentWorkflow, string history) {
        long result = 0;
        history += "-> " + currentWorkflow;

        foreach (var rule in workflows[currentWorkflow]) {
            Console.WriteLine("[{0}]--------------- Processing rule '{1}' with range {2}", history, rule, currentRange);
            XmasRange coveredRange = currentRange.SplitOffRange(rule.Category, rule.Range);

            if (rule.Goto == "A") {
                yield return coveredRange;
            } else if (rule.Goto != "R") {
                foreach(var range in FindAllRanges(workflows, coveredRange, rule.Goto, history)) {
                    yield return range;
                }
            }
        }
    }

    /// <summary>
    /// Parse the workflows from the inputs.
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public static Dictionary<string, List<Rule>> ParseWorkflows(IEnumerable<string> lines) {
        Dictionary<string, List<Rule>> results = [];
        foreach (var game in lines) {
            Regex regex = new(@"(\w*){(.*)}");
            var match = regex.Match(game);
            var name = match.Groups[1].Value;
            var rules = match.Groups[2].Value.Split(',').Select(s => new Rule(s)).ToList();
            results.Add(name, rules);
        }

        return results;
    }

    /// <summary>
    /// Parse a range from the input.
    /// Format should be '{x=787,m=2655,a=1222,s=2876}'
    /// </summary>
    /// <param name="line">The input</param>
    /// <returns>The xmas range (with start and end the same number).</returns>
    public static XmasRange ParseXmasRange(string line) {
        string[] values = line.Substring(1, line.Length - 2).Split(',');
        return new XmasRange(values.Select(s => s.Split('=')).Select(sa => (sa[0], new Range(long.Parse(sa[1]), long.Parse(sa[1])))).ToDictionary());
    }

    public struct Rule {
        public string? Category { get; set; }
        public Range Range { get; private set; }

        public string Goto { get; private set; }

        public Rule(string value) {
            Regex regex = new(@"(\w*)([><])(\d*):(\w*)");
            var match = regex.Match(value);
            if (match.Success) {
                Category = match.Groups[1].Value;
                long v = long.Parse(match.Groups[3].Value);
                if (match.Groups[2].Value == ">") {
                    Range = new(v + 1, 4000);
                } else {
                    Range = new(1, v - 1);
                }
                Goto = match.Groups[4].Value;
            } else {
                Range = new Range(1, 4000);
                Goto = value;
            }
        }
    }

    public class XmasRange(Dictionary<string, Range> rs) {

        private Dictionary<string, Range> Ranges { get; } = rs;
        public static XmasRange None() {
            return new XmasRange([]);
        }

        public static XmasRange All() {
            return new XmasRange(
                new Dictionary<string, Range> {
                    {"x",new(1,4000)},
                    {"m",new(1,4000)},
                    {"a",new(1,4000)},
                    {"s",new(1,4000)}
                }
            );
        }

        /// <summary>
        ///  Generates a new Range, and removes the generated range from this range.
        /// </summary>
        /// <param name="category">The category to modify, null for all</param>
        /// <param name="range">The range we are checking for overlaps</param>
        /// <returns></returns>
        public XmasRange SplitOffRange(string? category, Range range) {
            XmasRange newRange = Clone();

            if (category == null) {
                Ranges.Clear();
            } else if (Ranges.TryGetValue(category, out Range? value)) {
                if (value.OverlapWith(range)) {
                    newRange.Ranges[category] = value.GetOverlap(range);
                    Ranges[category] = Ranges[category].Remove(newRange.Ranges[category]);

                    // remove empty
                    if (!Ranges[category].IsValid()) {
                        Ranges.Clear(); // Once any one group is invalid all are
                    }
                }
            }

            return newRange;
        }

        public bool IsSubset(XmasRange other) {
            foreach((string category, Range range) in this.Ranges) {
                if(!other.Ranges.ContainsKey(category) || !range.IsSubset(other.Ranges[category])) {
                    return false;
                }
            }

            return true;
        }

        public long GetPart2Score() {
            return Ranges.Values.Select(r => r.Count()).Aggregate((total, current) => total * current);
        }

        public long GetPart1Score() {
            return Ranges.Values.Select(r => r.Start).Sum();
        }

        public XmasRange Clone() {
            Dictionary<string, Range> ranges = [];
            foreach(var kvp in this.Ranges) ranges.Add(kvp.Key, kvp.Value); 
            return new XmasRange(ranges);
        }

        public override string ToString() {
            return string.Join(" ", Ranges.Select(kvp => kvp.Key + "=" + kvp.Value));
        }
    }
}