using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using AdventLib;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 10](https://adventofcode.com/2023/day/10)
/// </summary>
public class Day10 : AdventSolver {
    public override string Day => "day10";


    // The list of all edge locations, this list assumes diagonally edges as well.
    private static readonly (int, int)[] Edges = [(-1, 0), (0, -1), (0, 1), (1, 0)];

    public override void Solve() {
        char[][] data = ReadInputAsCharArray();

        data.PrintMatrix("Original data", ",");

        Queue<(int, int, int, int, int, HashSet<string>)> q = new();

        // first find the S
        (int startX, int startY) = FindTile(data, 'S');
        Console.WriteLine("Starting from [" + startX + "][" + startY + "]");

        q.Enqueue((startX + 1, startY, startX, startY, 1, new HashSet<string>()));
        q.Enqueue((startX - 1, startY, startX, startY, 1, new HashSet<string>()));
        q.Enqueue((startX, startY + 1, startX, startY, 1, new HashSet<string>()));
        q.Enqueue((startX, startY - 1, startX, startY, 1, new HashSet<string>()));

        while (q.Count > 0) {
            (int edgeX, int edgeY, int lastX, int lastY, int count, HashSet<string> path) = q.Dequeue();
            (int nextX, int nextY) = GetNext(data, edgeX, edgeY, lastX, lastY);

            path.Add(edgeX + "," + edgeY);
            if (nextX == 0 && nextY == 0) {

                // Found another path
                Console.WriteLine("Solution for part 1 is: " + count);
                Console.WriteLine("Solution for part 2 is: " + ScanHelper(data, path));

                break;
            } else if (nextX != -100) {
                Console.WriteLine("Moving from [" + edgeX + "][" + edgeY + "] to [" + (edgeX + nextX) + "][" + (edgeY + nextY) + "]");
                q.Enqueue((edgeX + nextX, edgeY + nextY, edgeX, edgeY, count + 1, path));
            } else {
                Console.WriteLine("Nowhere to go");
            }
        }
    }

    public static int ScanHelper(char[][] data, HashSet<string> path) {
        // scan left to right if odd number found then its inside
        int count = 0;

        for (int y = 0; y < data.Length; y++) {
            bool inside = false;
            for (int x = 0; x < data[0].Length; x++) {
                if (path.Contains(x + "," + y)) {
                    // TODO: If the S if a J or L then it needs to be here, if not it needs to be skipped, should find that out and replace it
                    if (data[y][x] == '|' || data[y][x] == 'J' || data[y][x] == 'L') {
                        inside = !inside;
                    }
                } else {
                    if (inside) {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    // Locate the starter tile
    public static (int, int) FindTile(char[][] data, char target) {
        for (int y = 0; y < data.Length; y++) {
            for (int x = 0; x < data[0].Length; x++) {
                if (data[y][x] == target) {
                    return (x, y);
                }
            }
        }

        return (-1, -1);
    }

    // Given a current location, find the xy diff if you follow the path
    // Or return -100,-100 if this isnt valid, or 0,0 if we are done
    public static (int, int) GetNext(char[][] data, int edgeX, int edgeY, int lastX, int lastY) {
        Console.WriteLine(edgeX + "," + edgeY);
        if (edgeX < 0 || edgeY < 0 || edgeY >= data.Length || edgeX >= data[0].Length) return (-100, -100);
        if (data[edgeY][edgeX] == 'S') return (0, 0);

        if (lastX + 1 == edgeX && lastY == edgeY) {
            switch (data[edgeY][edgeX]) {
                case '-': return (1, 0);
                case 'J': return (0, -1);
                case '7': return (0, 1);
            }
        } else if (lastX - 1 == edgeX && lastY == edgeY) {
            switch (data[edgeY][edgeX]) {
                case '-': return (-1, 0);
                case 'L': return (0, -1);
                case 'F': return (0, 1);
            }
        } else if (lastX == edgeX && lastY + 1 == edgeY) {
            switch (data[edgeY][edgeX]) {
                case '|': return (0, 1);
                case 'L': return (1, 0);
                case 'J': return (-1, 0);
            }
        } else if (lastX == edgeX && lastY - 1 == edgeY) {
            switch (data[edgeY][edgeX]) {
                case '|': return (0, -1);
                case 'F': return (+1, 0);
                case '7': return (-1, 0);
            }
        }

        return (-100, -100);
    }


    [TestClass]
    public class Day10Test {

        [TestMethod]
        public void Test1() {
            char[][] map = [['S', '-', '7'], ['L', '-', 'J']];
            //Assert.AreEqual(6, FindDistanceToTile(map).Item1);
        }
    }
}