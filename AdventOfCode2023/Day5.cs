using System.Text.RegularExpressions;
using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 5](https://adventofcode.com/2023/day/5)
/// </summary>
public class Day5 : AdventSolver {
    public override string Day => "day5";

    public override void Solve(string filename) {

        List<string> allData = [.. ReadLines(filename)];

        // This is the collection of all seeds
        long[] seeds = allData[0].Split(":", StringSplitOptions.TrimEntries)[1].Split(" ").Select(long.Parse).ToArray();

        // Conversion maps for sourceCategory -> Converter
        Dictionary<string, CategoryConverter> converters = new Dictionary<string, CategoryConverter>();

        // Parse all of the conversions
        int line = 2;
        while (line < allData.Count) {
            CategoryConverter map = new CategoryConverter(allData[line++]);
            while (line < allData.Count && !string.IsNullOrWhiteSpace(allData[line])) {
                var conversion = new RangeModifier(allData[line++]);
                map.conversions.Add(conversion);
            }

            converters.Add(map.SourceCategory, map);

            line++; // Skips the empty line
        }

        Queue<Range> part1Ranges = new Queue<Range>(seeds.Select(s => new Range(s, s)));
        Console.WriteLine("Solution for part 1 is: " + ConvertToLocation(converters, part1Ranges).Select(r => r.Start).Min());

        Queue<Range> part2Ranges = new Queue<Range>(seeds.Chunk(2).Select(arr => new Range(arr[0], arr[0] + arr[1] - 1)));
        Console.WriteLine("Solution for part 2 is: " + ConvertToLocation(converters, part2Ranges).Select(r => r.Start).Min());
    }

    /// <summary>
    /// Helper which converts the original seed ranges into locations
    /// </summary>
    /// <param name="converters"></param>
    /// <param name="ranges"></param>
    /// <returns></returns>
    private static Queue<Range> ConvertToLocation(Dictionary<string, CategoryConverter> converters, Queue<Range> ranges) {
        string category = "seed";
        while (category != "location") {
            var converter = converters[category];

            ranges = converter.Map(ranges);
            category = converter.DestinationCategory;

            //Console.WriteLine("After mapping from " + converter.SourceCategory + " to " + converter.DestinationCategory + " ranges are " + string.Join(",", ranges));
        }

        return ranges;
    }

    public class CategoryConverter {
        public string SourceCategory { get; private set; }
        public string DestinationCategory { get; private set; }

        public List<RangeModifier> conversions { get; private set; }

        public CategoryConverter(string line) {
            Regex regex = new Regex(@"(\w*)-to-(\w*) map:");
            var match = regex.Match(line);
            SourceCategory = match.Groups[1].Value;
            DestinationCategory = match.Groups[2].Value;
            conversions = new List<RangeModifier>();
        }

        /// <summary>
        /// Convert a collection of ranges into the modified collection of ranges.
        /// </summary>
        /// <param name="ranges">The original set of ranges</param>
        /// <returns>A new set of ranges after they have been converted by this classes converters.</returns>
        public Queue<Range> Map(Queue<Range> ranges) {
            Queue<Range> results = new Queue<Range>();

            while (ranges.Count > 0) {
                Range next = ranges.Dequeue();

                bool foundOverlap = false;

                foreach (var conversion in conversions) {
                    if (conversion.OverlapWith(next)) {
                        foundOverlap = true;
                        var (unprocessed, result) = conversion.ApplyOffset(next);
                        unprocessed.ForEach(ranges.Enqueue);
                        results.Enqueue(result);
                        break;
                    }
                }

                if (!foundOverlap) results.Enqueue(next);
            }

            return results;
        }

        public override string ToString() {
            return SourceCategory + "->" + DestinationCategory + " = " + string.Join(",", conversions);
        }
    }

    /// <summary>
    /// Modifier which applies an offset to a range of numbers.
    /// </summary>
    public class RangeModifier {
        public Range sourceRange { get; private set; }

        public long offset { get; private set; }

        public RangeModifier(string line) {
            var split = line.Split(' ').Select(long.Parse).ToArray();
            sourceRange = new Range(split[1], split[1] + split[2] - 1);
            offset = split[0] - sourceRange.Start;
        }

        /// <summary>
        /// Checksx if this RangeModifier at least partially overlaps with the range.
        /// </summary>
        /// <param name="other">The range of numbers looking to be mapped.</param>
        /// <returns>True if any part overlaps</returns>
        public bool OverlapWith(Range other) {
            return sourceRange.OverlapWith(other);
        }

        /// <summary>
        /// Given a range of numbers [1...100] apply the offset to any of the numbers that are in the range of this modifier.
        /// Any elements contained in range but *not* modified by this will be returned as unprocessed.
        /// </summary>
        /// <param name="range">The range of source numbers.</param>
        /// <returns>A tuple, the first is a list of any ranges that weren't covered by this apply, the second is the modified value that was covered.</returns>
        public (List<Range>, Range) ApplyOffset(Range range) {
            // For numbers in the range which aren't covered by this modifier gather them into a list so they can be processed.
            List<Range> unprocessed = new List<Range>();
            if (range.Start < sourceRange.Start) {
                unprocessed.Add(new Range(range.Start, sourceRange.Start - 1));
            }

            if (range.End > sourceRange.End) {
                unprocessed.Add(new Range(sourceRange.End + 1, range.End));
            }

            // For the numbers in range, add the new mapped range
            long start = Math.Max(range.Start, sourceRange.Start);
            long end = Math.Min(range.End, sourceRange.End);

            return (unprocessed, new Range(start + offset, end + offset));
        }

        public override string ToString() {
            return sourceRange + " + " + offset;
        }
    }

    /// <summary>
    /// Represents an inclusive range of numbers
    /// </summary>
    public class Range {
        public long Start { get; set; }
        public long End { get; set; }

        public Range(long start, long end) {
            this.Start = start;
            this.End = end;
        }

        public bool OverlapWith(Range other) {
            return !(other.Start > End || other.End < Start);
        }


        public override string ToString() {
            return "[" + Start + "..." + End + "]";
        }
    }
}