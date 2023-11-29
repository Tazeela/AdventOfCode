using System.Diagnostics;
using System.Dynamic;
using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 3](https://adventofcode.com/2022/day/3)
/// </summary>
public class RucksackReorg : AdventSolver {
   public override string Day => "day3";

   public override void Solve(string filename) {
      int points1 = 0;
      int points2 = 0;

      foreach (string[] rucksacks in ReadLines(filename).Chunk(3)) {
         points1 += GetPriority(FindMisplacedItems(rucksacks[0]));
         points1 += GetPriority(FindMisplacedItems(rucksacks[1]));
         points1 += GetPriority(FindMisplacedItems(rucksacks[2]));

         points2 += GetPriority(FindBadge(rucksacks));
      }

      Console.WriteLine("Solution for part 1 is " + points1);
      Console.WriteLine("Solution for part 2 is " + points2);
   }

   /// <summary>
   /// Find the first item that is in both the first half of the rucksack and the second.
   /// </summary>
   /// <param name="rucksack">The list of items in a rucksack.</param>
   /// <returns>The first character in common between both sides of hte rucksack.</returns>
   private static char FindMisplacedItems(string rucksack) {
      int half = rucksack.Length / 2;
      string firstPart = rucksack.Substring(0, half);
      string secondPart = rucksack.Substring(half, half);

      return FindDuplicateItem(firstPart, secondPart).First();
   }

   /// <summary>
   /// Given a group of 3 rucksacks find the element thats shared between all 3
   /// </summary>
   /// <param name="threeRucksacks">A group of 3 rucksacks.</param>
   /// <returns>The first character in common between all 3 rucksacks.</returns>
   private static char FindBadge(string[] threeRucksacks) {
      return FindDuplicateItem(FindDuplicateItem(threeRucksacks[0], threeRucksacks[1]), threeRucksacks[2]).First();
   }

   /// <summary>
   ///  Given two lists of items, find the duplicates.
   /// </summary>
   /// <param name="first">The first list of items.</param>
   /// <param name="second">The second list of items.</param>
   /// <returns>A stream of any duplicate items.</returns>
   private static IEnumerable<char> FindDuplicateItem(IEnumerable<char> first, IEnumerable<char> second) {
      HashSet<char> items = new();

      foreach (char c in first) {
         items.Add(c);
      }

      foreach (char c in second) {
         if (items.Contains(c)) {
            yield return c;
         }
      }
   }

   /// <summary>
   /// Converts the item into its priorit. Lowercase item types a through z have priorities 1 through 26.
   /// Uppercase item types A through Z have priorities 27 through 52. 
   /// </summary>
   /// <param name="item">The item we are converting.</param>
   /// <returns>The priority value.</returns>
   private static int GetPriority(char item) {
      int itemVal = (int)item;
      int result;
      if (itemVal > 96) { // lowercase letters are 97 - 122
         result = itemVal - 96;
      } else { // uppercase letters are 65 - 90
         result = itemVal - 38;
      }

      Debug.WriteLine(string.Format("Converted {0} to {1}", item, result));
      return result;
   }
}
