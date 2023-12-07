using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 3](https://adventofcode.com/2023/day/3)
/// </summary>
public class Day3 : AdventSolver {

    public override string Day => "day3";

    // The list of all edge locations, this list assumes diagonally edges as well.
    private static readonly (int, int)[] Edges = [(-1, 0), (-1, -1), (-1, 1), (0, -1), (0, 1), (1, 0), (1, -1), (1, 1)];

    public override void Solve(string filename) {
        int count1 = 0;
        int count2 = 0;

        List<string> allData = [.. ReadLines(filename)];

        count1 = GetAllPartNumbers(allData, null).Values.SelectMany(parts => parts).Sum();
        count2 = GetAllPartNumbers(allData, ['*']).Values.Where(parts => parts.Count == 2).Select(parts => parts[0] * parts[1]).Sum();

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    /// <summary>
    /// Get all of the part numbers, and the location of the symbol which they are matched to.
    /// Note that if a part number is connected to multiple symbols then it will be duplicated, its unclear from 
    /// the problem whether or not this is the correct course, but it never comes up.
    /// </summary>
    /// <param name="data">The data array.</param>
    /// <param name="allowedSymbols">Optional list of allowed symbols, if null allows any non '.'</param>
    /// <returns>A dictionary that maps the location of the symbol and a list of all connected part numbers.</returns>
    private Dictionary<string, List<int>> GetAllPartNumbers(List<string> data, char[]? allowedSymbols) {
        Dictionary<string, List<int>> adjacentValues = [];
        for (int x = 0; x < data.Count; x++) {

            var adjacentLocations = new HashSet<string>();
            int number = 0;
            for (int y = 0; y < data[0].Length; y++) {
                if (char.IsDigit(data[x][y])) {
                    // We have found a digit, add it to the in progress number
                    number = number * 10 + (int)char.GetNumericValue(data[x][y]);
                    foreach (var loc in GetAdjacentSymbolLocations(data, x, y, allowedSymbols)) {
                        adjacentLocations.Add(loc);
                    }
                } else {
                    // Otherwise check to see if we need to flush out a location
                    foreach (var loc in adjacentLocations) {
                        if (!adjacentValues.ContainsKey(loc)) adjacentValues.Add(loc, []);
                        adjacentValues[loc].Add(number);
                    }

                    number = 0;
                    adjacentLocations.Clear();
                }
            }
        }

        return adjacentValues;
    }


    /// <summary>
    /// Given an x and y coordinate, determine if there is an adjacent symbol, and if there is where to find it.
    /// From an optimization perspective this could definitely be improved as we check the previous digits in the word, 
    /// and we duplicate checks for symbols in tiles we already checked. It also continues after finding the first symbol, 
    /// though only because I expected the problem to care about multiple edges while doing part 1 it never gets used.
    /// </summary>
    /// <param name="data">The data array.</param>
    /// <param name="xCoord">The x coord to test around.</param>
    /// <param name="yCoord">The y coord to test around.</param>
    /// <param name="allowedSymbols">Optional list of allowed symbols, if null allows any non '.'</param>
    /// <returns>A list of strings representing the symbol locations if a symbol is found.</returns>
    private IEnumerable<string> GetAdjacentSymbolLocations(List<string> data, int xCoord, int yCoord, char[]? allowedSymbols) {
        foreach ((int xdiff, int ydiff) in Edges) {
            int edgeX = xCoord + xdiff;
            int edgeY = yCoord + ydiff;
            if (edgeX >= 0 && edgeY >= 0 && edgeX < data.Count && edgeY < data[0].Length) {
                char c = data[edgeX][edgeY];
                if (!char.IsDigit(c) && c != '.' && (allowedSymbols == null || allowedSymbols.Contains(c))) {
                    yield return edgeX + "," + edgeY;
                }
            }
        }
    }
}