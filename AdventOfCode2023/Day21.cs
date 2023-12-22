using System.Data;
using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 21](https://adventofcode.com/2023/day/21)
/// </summary>
public class Day21 : AdventSolver {
    public override string Day => "day21";

    public override void Solve() {
        var map = ReadInputAsCharArray();
        Garden garden = new Garden(map);
        Console.WriteLine("Solution for part 1 is: " + garden.TravelGarden(64));
        (int, int) start = map.FindElement('S');

        char[][] input = ReadInputAsCharArray();

        var gridSize = 131;
        var grids = 26501365 / gridSize;
        var rem = 26501365 % gridSize;
        var sequence = new List<int>();
        var work = new HashSet<(int, int)> { start };
        long steps = 0;
        for (var n = 0; n < 3; n++) {
            for (; steps < n * gridSize + rem; steps++) {
                work = new HashSet<(int, int)>(work
                    .SelectMany(point => point.Adjacent()))
                    .Where(dest => input[MathUtils.PositiveModulo(dest.Item1, gridSize)][MathUtils.PositiveModulo(dest.Item2, gridSize)] != '#').ToHashSet();
            }

            sequence.Add(work.Count);
        }

        // Solve for the quadratic coefficients
        long c = sequence[0];
        long aPlusB = sequence[1] - c;
        long fourAPlusTwoB = sequence[2] - c;
        long twoA = fourAPlusTwoB - (2 * aPlusB);
        long a = twoA / 2;
        long b = aPlusB - a;

        long count2 = a * grids * grids + b * grids + c;

        Console.WriteLine(count2);
    }

    public class Garden(char[][] map) {
        public int TravelGarden(int remaining) {
            (int, int) start = map.FindElement('S');
            Queue<(int, int, int)> todo = [];
            todo.Enqueue((remaining, start.Item1, start.Item2));
            int[][] results = new int[map.Length][];
            foreach (var idx in results.Indexes()) results[idx] = new int[map[0].Length];

            while (todo.Count > 0) {
                var next = todo.Dequeue();
                foreach (var newItem in TravelGardenInner(results, next.Item1, next.Item2, next.Item3)) {
                    todo.Enqueue(newItem);
                }
            }

            return results.SelectMany(s => s).Where(v => (v & 0x01) != 0).Count();
        }

        private IEnumerable<(int, int, int)> TravelGardenInner(int[][] memo, int remaining, int currentX, int currentY) {
            if (currentX < 0 || currentY < 0 || currentX >= map.Length || currentY >= map[0].Length) return [];
            if (map[currentX][currentY] == '#') return [];
            int current = memo[currentX][currentY];

            if (current == 0x11) return [];
            if ((current & 0x01) == 0 && remaining % 2 == 0) {
                memo[currentX][currentY] = current | 0x01;
                return [
                    (remaining - 1, currentX + 1 , currentY),
                (remaining - 1, currentX - 1, currentY),
                (remaining - 1, currentX, currentY - 1),
                (remaining - 1, currentX, currentY + 1)];
            }

            if ((current & 0x10) == 0 && remaining % 2 == 1) {
                memo[currentX][currentY] = current | 0x10;
                return [
                    (remaining - 1, currentX + 1 , currentY),
                (remaining - 1, currentX - 1, currentY),
                (remaining - 1, currentX, currentY - 1),
                (remaining - 1, currentX, currentY + 1)];
            }

            return [];

        }
    }
}