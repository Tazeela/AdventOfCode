using System.Data;
using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 16](https://adventofcode.com/2023/day/16)
/// </summary>
public class Day16 : AdventSolver {
    public override string Day => "day16";

    private const int fromLeft = 0x01;
    private const int fromRight = 0x10;
    private const int fromTop = 0x100;
    private const int fromBottom = 0x1000;

    public override void Solve() {
        var map = ReadInputAsCharArray();

        List<int> results = [];
        foreach (int x in map.Indexes()) {
            results.Add(CalculateNumEnergized(map, new(x, 0, fromLeft)));
            results.Add(CalculateNumEnergized(map, new(x, map[x].Length, fromRight)));
        }

        foreach (int x in map[0].Indexes()) {
            results.Add(CalculateNumEnergized(map, new(0, x, fromTop)));
            results.Add(CalculateNumEnergized(map, new(map.Length, x, fromBottom)));
        }

        Console.WriteLine("Solution for part 1 is: " + CalculateNumEnergized(map, new (0, 0, fromLeft)));
        Console.WriteLine("Solution for part 2 is: " + results.Max());
    }

    //
    public static int CalculateNumEnergized(char[][] map, Traversal origin) {
        var energize = new int[map.Length][];
        for (int x = 0; x < energize.Length; x++) {
            energize[x] = new int[map[x].Length];
        }

        Queue<Traversal> toDo = new();
        toDo.Enqueue(origin);

        while (toDo.Count > 0) {
            var next = toDo.Dequeue();
            if (next.Row >= 0 & next.Col >= 0 && next.Row < map.Length && next.Col < map[next.Row].Length && (energize[next.Row][next.Col] & next.Direction) == 0) {
                foreach (var outbound in GetNext(map[next.Row][next.Col], next)) {
                    toDo.Enqueue(outbound);
                }
                energize[next.Row][next.Col] |= next.Direction;
            }
        }

        return energize.SelectMany(e => e).Where(i => i > 0).Count();
    }

    // Give a tile and the traversal onto that tile, find all outbound pathing
    public static IEnumerable<Traversal> GetNext(char tile, Traversal input) {
        if (input.Direction == fromLeft) {
            if (tile == '.' || tile == '-') yield return new(input.Row, input.Col + 1, fromLeft);
            if (tile == '|' || tile == '/') yield return new(input.Row - 1, input.Col, fromBottom);
            if (tile == '|' || tile == '\\') yield return new(input.Row + 1, input.Col, fromTop);
        } else if (input.Direction == fromRight) {
            if (tile == '.' || tile == '-') yield return new(input.Row, input.Col - 1, fromRight);
            if (tile == '|' || tile == '\\') yield return new(input.Row - 1, input.Col, fromBottom);
            if (tile == '|' || tile == '/') yield return new(input.Row + 1, input.Col, fromTop);
        } else if (input.Direction == fromTop) {
            if (tile == '.' || tile == '|') yield return new(input.Row + 1, input.Col, fromTop);
            if (tile == '-' || tile == '\\') yield return new(input.Row, input.Col + 1, fromLeft);
            if (tile == '-' || tile == '/') yield return new(input.Row, input.Col - 1, fromRight);
        } else if (input.Direction == fromBottom) {
            if (tile == '.' || tile == '|') yield return new(input.Row - 1, input.Col, fromBottom);
            if (tile == '-' || tile == '\\') yield return new(input.Row, input.Col - 1, fromRight);
            if (tile == '-' || tile == '/') yield return new(input.Row, input.Col + 1, fromLeft);
        }
    }

     public struct Traversal {

        public Traversal(int row, int col, int direction) {
            this.Row = row;
            this.Col = col;
            this.Direction = direction;
        }

        public int Row { get; set; }
        public int Col { get; set; }
        public int Direction { get; set; }
    }
}