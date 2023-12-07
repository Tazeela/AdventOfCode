using System.Collections;
using System.ComponentModel.DataAnnotations;
using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 4](https://adventofcode.com/2023/day/4)
/// </summary>
public class Day4 : AdventSolver {
    public override string Day => "day4";

    public override void Solve(string filename) {
        int count1 = 0;

        // preread all lines so we know the count to allocate our array for
        List<string> allData = [.. ReadLines(filename)];

        int[] numCards = Enumerable.Repeat(1, allData.Count).ToArray();

        for (int currentCard = 0; currentCard < allData.Count; currentCard++) {
            int numWins = GetNumWins(allData[currentCard]);
            if (numWins > 0) {
                count1 += (int)Math.Pow(2, numWins - 1);

                for (int x = 0; x < numWins; x++) {
                    int nextCard = currentCard + x + 1;
                    if (nextCard < numCards.Length) numCards[nextCard] += numCards[currentCard];
                }
            }
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + numCards.AsEnumerable().Sum());

    }

    /// <summary>
    /// Given a line like "Game 1: 1 2 3 | 2 4 5" find how many wins you get where a number before the | matches a number after.
    /// </summary>
    /// <param name="game">The game data</param>
    /// <returns>The number of wins</returns>
    private int GetNumWins(string game) {
        var s1 = game.Split(":")[1].Split("|");
        var winningNumbers = s1[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        return s1[1].Trim().Split(' ').Where(winningNumbers.Contains).Count();
    }
}