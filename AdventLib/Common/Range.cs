namespace AdventLib.Common;

/// <summary>
/// Represents an inclusive range of numbers
/// </summary>
public class Range(long start, long end) {

    /// <summary>
    /// Represents an empty range.
    /// </summary>
    public static Range Empty = new(-1, -1);

    /// <summary>
    /// The start value of the range.
    /// </summary>
    public long Start { get; private set; } = start;
    
    /// <summary>
    /// The end value of the range.
    /// </summary>
    public long End { get; private set; } = end;

    /// <summary>
    /// Check if this range overlaps at any point with another range, doesnt need to be completely.
    /// </summary>
    /// <param name="target">The target range.</param>
    /// <returns>True if this range overlaps at all, false otherwise.</returns>
    public bool OverlapWith(Range other) {
        return !(other.Start > End || other.End < Start);
    }

    /// <summary>
    /// Given a range get a range that represents the overlap between this range and the target.
    /// [1...100].GetOverlap([75...125]) would return [75...100].
    /// </summary>
    /// <param name="target">The target range.</param>
    /// <returns>The overlap, or Empty if no overlap exists.</returns>
    public Range GetOverlap(Range target) {
        if (!OverlapWith(target)) {
            return Range.Empty;
        } else {
            return new(Math.Max(target.Start, Start), Math.Min(target.End, End));
        }
    }

    /// <summary>
    /// Create a new range by removing any overlap with target from this range.
    /// </summary>
    /// <param name="target">The target range.</param>
    /// <returns>A new range with target removed.</returns>
    public Range Remove(Range target) {
        long newEnd = this.End;

        if (target.Start - 1 > this.Start && target.Start - 1 < this.End) {
            newEnd = target.Start - 1;
        }


        long newStart = this.Start;
        if (target.End + 1 < this.End && target.End + 1 > this.Start) {
            newStart = target.End + 1;
        }

        return new(newStart, newEnd);
    }

    /// <summary>
    /// A valid range is a range where start is less then or equal to the end.
    /// </summary>
    /// <returns>True if valid.</returns>
    public bool IsValid() {
        return Start <= End;
    }

    /// <summary>
    /// Check if a specific number is included in the range.
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public bool Contains(long num) {
        return num >= Start && num <= End;
    }

    /// <summary>
    /// Check if the target range is a subset of this range.
    /// [1...100].IsSubset([75...125]) would return false.
    /// [1...100].IsSubset([75...95]) would return false.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsSubset(Range target) {
        return this.Start >= target.Start && this.End <= target.End;
    }

    /// <summary>
    /// Count the number of distinct elements in this range.
    /// </summary>
    /// <returns>The number of elements from start to end, inclusive.</returns>
    public long Count() {
        return End - Start + 1;
    }

    /// <summary>
    /// ToString implementation which shows the range.
    /// </summary>
    /// <returns>String representing the range.</returns>
    public override string ToString() {
        return "[" + Start + "..." + End + "]";
    }
}