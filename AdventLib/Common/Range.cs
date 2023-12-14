namespace AdventLib.Common;

/// <summary>
/// Represents an inclusive range of numbers
/// </summary>
public class Range {
    public long Start { get; private set; }
    public long End { get; private set; }

    public Range(long start, long end) {
        Start = start;
        End = end;
    }

    public bool OverlapWith(Range other) {
        return !(other.Start > End || other.End < Start);
    }


    public override string ToString() {
        return "[" + Start + "..." + End + "]";
    }
}