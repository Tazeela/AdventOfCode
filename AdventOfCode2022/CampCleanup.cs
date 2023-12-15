using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 4](https://adventofcode.com/2022/day/4)
/// </summary>
public class CampCleanup : AdventSolver {
    public override string Day => "day4";

    public override void Solve() {
        int count1 = 0;
        int count2 = 0;
        foreach (string pairing in ReadInputAsIEnumerable()) {
            int[] parts = pairing.Split(['-', ',']).Select(int.Parse).ToArray();

            if (CheckIfOverlapEntirely(parts[0], parts[1], parts[2], parts[3])) {
                count1++;
            }


            if (CheckIfAnyOverlap(parts[0], parts[1], parts[2], parts[3])) {
                count2++;
            }
        }

        Console.WriteLine("Solution for part 1 is " + count1);
        Console.WriteLine("Solution for part 2 is " + count2);
    }



    /// <summary>
    /// Check if there is complete overlap between the start and end range for the two elves.
    /// </summary>
    /// <param name="start1">Elf 1 start</param>
    /// <param name="end1">Elf 1 end</param>
    /// <param name="start2">Elf 2 start</param>
    /// <param name="end2">Elf 2 end</param>
    /// <returns>True if one elfs range entirely encompesses the others.</returns>
    private static bool CheckIfOverlapEntirely(int start1, int end1, int start2, int end2) {
        return (start1 >= start2 && end1 <= end2) || (start2 >= start1 && end2 <= end1);
    }

    /// <summary>
    /// Check if there is any overlap between the start and end range for the two elves.
    /// </summary>
    /// <param name="start1">Elf 1 start</param>
    /// <param name="end1">Elf 1 end</param>
    /// <param name="start2">Elf 2 start</param>
    /// <param name="end2">Elf 2 end</param>
    /// <returns>True if both elves are assigned at least 1 duplicate.</returns>
    private static bool CheckIfAnyOverlap(int start1, int end1, int start2, int end2) {
        return (start1 >= start2 && start1 <= end2) || (end1 <= end2 && end1 > start2) || (start2 <= end1 && start2 >= start1) || (end2 <= end1 && end2 >= start1);
    }
}