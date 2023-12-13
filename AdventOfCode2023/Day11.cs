using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 11](https://adventofcode.com/2023/day/11)
/// </summary>
public class Day11 : AdventSolver {
    public override string Day => "day11";

    public override void Solve(string filename) {
        long count1 = 0;
        int count2 = 0;

        List<string> originalData = [.. ReadLines(filename)];
        
        var galaxies = GetAllGalaxies(originalData);
        Console.WriteLine("All galaxies " + string.Join(",", galaxies));
        int numPairs = 0;
        for(int x = 0; x < galaxies.Length; x++) {
            for(int y = x + 1; y < galaxies.Length; y++) {
                numPairs++;
                count1 +=  CalculateDistance(galaxies[x], galaxies[y]);
            }
        }

         Console.WriteLine("Solution for part 1 is: " + count1 + "   " + numPairs);
         Console.WriteLine("Solution for part 2 is: " + count2);
    }

    public static long CalculateDistance((long, long) p1, (long, long) p2) {
       long distance = Math.Abs(p2.Item1 - p1.Item1) + Math.Abs(p2.Item2 - p1.Item2);
       return distance;
    }

    public static (long, long)[] GetAllGalaxies(List<string> originalData) {
        long[] numGalaxiesInColumn = new long[originalData.First().Length];
        long[] numGalaxiesInRow = new long[originalData.Count];

        HashSet<(int, int)> galaxies = new HashSet<(int, int)>();
        
        // Generate two arrays with the number in x and y
        for(int y = 0; y < numGalaxiesInRow.Length; y++) {
            for (int x = 0; x < numGalaxiesInColumn.Length; x++) {
                if(originalData[y][x] == '#') {
                    numGalaxiesInColumn[x]++;
                    numGalaxiesInRow[y]++;
                    galaxies.Add((x, y));
                }
            }
        }

        // Builds a translation matrix for x and y
        long offset = 0;
        for(int x = 0; x < numGalaxiesInColumn.Length; x++) {
            if(numGalaxiesInColumn[x] == 0) {
                offset += 1000000 - 1;
            } 

            numGalaxiesInColumn[x] = x + offset;
        }

        offset = 0;
        for(int y = 0; y < numGalaxiesInRow.Length; y++) {
            if(numGalaxiesInRow[y] == 0) {
                offset += 1000000 - 1;
            } 

            numGalaxiesInRow[y] = y + offset;
        }

        Console.WriteLine("Column translations " + string.Join(".", numGalaxiesInColumn));
        Console.WriteLine("Row translations " + string.Join(".", numGalaxiesInRow));

        return galaxies.Select(orig => {
            (long, long) newCoord =  (numGalaxiesInColumn[orig.Item1], numGalaxiesInRow[orig.Item2]);
            return newCoord;
        }).ToArray();
    } 
}