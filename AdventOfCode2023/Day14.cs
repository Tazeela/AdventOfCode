using System.Data;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 14](https://adventofcode.com/2023/day/14)
/// </summary>
public class Day14 : AdventSolver {
    public override string Day => "day14";

    public Dictionary<string, (char[][], int)> cache = new Dictionary<string, (char[][], int)>();

    public override void Solve(string filename) {
        List<string> allData = [.. ReadLines(filename)];
        char[][] allDataChars1 = allData.Select(s => s.ToCharArray()).ToArray();
        TiltNorth(allDataChars1);
        Console.WriteLine("Solution for part 1 is: " + CalculatePoints(allDataChars1));

        char[][] allDataChars2 = allData.Select(s => s.ToCharArray()).ToArray();
        Cycle(allDataChars2, 1000000000);
        Console.WriteLine("Solution for part 2 is: " + CalculatePoints(allDataChars2));
    }

    public void Cycle(char[][] data, int cycleCount) {
        List<int> loop = [];

        for (int x = 0; x < cycleCount; x++) {
            TiltNorth(data);
            TiltWest(data);
            TiltSouth(data);
            TiltEast(data);
            
            int hash = string.Join(":", data.Select(s => new string(s))).GetHashCode();

            if (loop.Contains(hash)) {
                // We already calculated this before, so take the number of cycles between then and now and skip that far ahead
                int size = x - loop.IndexOf(hash);
                while (x + size < cycleCount) x += size;
            } else {
                loop.Add(hash);
            }
        }
    }


    public static void TiltNorth(char[][] data) {
        for (int col = 0; col < data[0].Length; col++) {
            Tilt(data, Enumerable.Range(0, data.Length).Select(x => (x, col)).ToArray());
        }
    }

    public static void TiltWest(char[][] data) {
        for (int row = 0; row < data.Length; row++) {
            Tilt(data, Enumerable.Range(0, data.Length).Select(x => (row, x)).ToArray());
        }
    }

    public static void TiltEast(char[][] data) {
        for (int row = 0; row < data.Length; row++) {
            Tilt(data, Enumerable.Range(0, data.Length).Reverse().Select(x => (row, x)).ToArray());
        }
    }

    public static void TiltSouth(char[][] data) {
        for (int col = 0; col < data[0].Length; col++) {
            Tilt(data, Enumerable.Range(0, data.Length).Reverse().Select(x => (x, col)).ToArray());
        }
    }

    // Given a list of positions in the array, move all 'O' elements until they are blocked by another 'O', and '#' or the start.
    private static void Tilt(char[][] input, (int, int)[] indexes) {
        int offset = 0;
        for (int x = 0; x < indexes.Length; x++) {
            if (input[indexes[x].Item1][indexes[x].Item2] == 'O') {
                input[indexes[offset].Item1][indexes[offset].Item2] = input[indexes[x].Item1][indexes[x].Item2];
                if(offset != x) input[indexes[x].Item1][indexes[x].Item2] = '.';
                offset++;
            } else if (input[indexes[x].Item1][indexes[x].Item2] == '#') {
                offset = x + 1;
            }
        }
    }

    public static int CalculatePoints(char[][] data) {
        int result = 0;

        for (int row = 0; row < data.Length; row++) {
            for (int column = 0; column < data[row].Length; column++) {
                if (data[row][column] == 'O') {
                    result += data[row].Length - row;
                }
            }
        }

        return result;
    }


    [TestClass]
    public class Day14Test {

        [TestMethod]
        public void Test1() {

        }
    }
}