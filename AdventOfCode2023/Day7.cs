using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 7](https://adventofcode.com/2023/day/7)
/// </summary>
public class Day7 : AdventSolver {
    public override string Day => "day7";

    private static readonly char[] Points = ['X', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];

    public override void Solve(string filename) {
        int count1 = 0;
        int count2 = 0;

        List<Hand> problem1Hands = [];
        List<Hand> problem2Hands = [];

        foreach (String game in ReadLines(filename)) {
            problem1Hands.Add(new Hand(game, false));
            problem2Hands.Add(new Hand(game, true));
        }

        problem1Hands.Sort();
        problem2Hands.Sort();

        for (int x = 0; x < problem1Hands.Count; x++) {
            count1 += (x + 1) * problem1Hands[x].Bet;
        }


        for (int x = 0; x < problem2Hands.Count; x++) {
            count2 += (x + 1) * problem2Hands[x].Bet;
        }

        Console.WriteLine("Solution for part 1 is: " + count1);
        Console.WriteLine("Solution for part 2 is: " + count2);
    }

    public static int GetStrength(char[] cards) {
        int numJokers = cards.Where(c => c == 'X').Count();
        int[] counts = [.. cards.Where(c => c != 'X').GroupBy(s => s).Select(s => s.Count()).OrderByDescending(s => s)];

        // Special case where all jokers
        if(numJokers == 5) return 99;

        return (counts[0] + numJokers) switch {
            5 => 99,
            4 => 88,
            3 => (counts[1] == 2) ? 77 : 66,
            2 => (counts[1] == 2) ? 55 : 44,
            _ => 33,
        };
    }

    public class Hand : IComparable<Hand> {
        public Hand(string text, bool allowJokers = false) {
            var split = text.Split(" ");
            var rawCards = split[0];
            if (allowJokers) {
                rawCards = rawCards.Replace('J', 'X');
            }

            Cards = rawCards.ToCharArray();
            Bet = int.Parse(split[1]);
        }

        public char[] Cards { get; set; }
        public int Bet { get; set; }

        public int CompareTo(Hand? other) {
            int handCompareResult = GetStrength(Cards).CompareTo(GetStrength(other?.Cards));

            if (handCompareResult == 0) {
                for (int x = 0; x < Cards.Length; x++) {
                    int cardCompareResult = Array.IndexOf(Points, Cards[x]).CompareTo(Array.IndexOf(Points, other.Cards[x]));
                    if (cardCompareResult != 0) return cardCompareResult;
                }
            }

            return handCompareResult;
        }
    }

    [TestClass]
    public class Day7Test {

        [TestMethod]
        public void GradeHand_TestVariousHands() {
            Assert.AreEqual(99, GetStrength(['a', 'a', 'a', 'a', 'a']));
            Assert.AreEqual(99, GetStrength(['a', 'a', 'a', 'a', 'X']));
            Assert.AreEqual(99, GetStrength(['a', 'a', 'a', 'X', 'X']));
            Assert.AreEqual(99, GetStrength(['a', 'a', 'X', 'X', 'X']));
            Assert.AreEqual(99, GetStrength(['a', 'X', 'X', 'X', 'X']));
            Assert.AreEqual(99, GetStrength(['X', 'X', 'X', 'X', 'X']));


            Assert.AreEqual(88, GetStrength(['a', 'a', 'b', 'a', 'a']));
            Assert.AreEqual(88, GetStrength(['a', 'a', 'b', 'X', 'a']));
            Assert.AreEqual(88, GetStrength(['a', 'a', 'b', 'X', 'X']));
            Assert.AreEqual(88, GetStrength(['a', 'X', 'b', 'X', 'X']));


            Assert.AreEqual(77, GetStrength(['a', 'a', 'a', 'b', 'b']));
            Assert.AreEqual(77, GetStrength(['a', 'a', 'X', 'b', 'b']));

            Assert.AreEqual(66, GetStrength(['a', 'a', 'a', 'b', 'c']));
            Assert.AreEqual(66, GetStrength(['a', 'a', 'X', 'b', 'c']));

            Assert.AreEqual(55, GetStrength(['a', 'a', 'b', 'b', 'c']));

            Assert.AreEqual(44, GetStrength(['a', 'a', 'b', 'c', 'd']));
            Assert.AreEqual(44, GetStrength(['a', 'X', 'b', 'c', 'd']));
        }
    }
}