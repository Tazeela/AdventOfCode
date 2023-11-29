using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 2](https://adventofcode.com/2022/day/2)
/// </summary>
public class RockPaperScissors : AdventSolver {
   public override string Day => "day2";

   public override void Solve(string filename) {
      int sum1 = 0;
      int sum2 = 0;
      foreach (string game in ReadLines(filename)) {
         sum1 += DetermineScorePart1(game[0], game[2]);
         sum2 += DetermineScorePart2(game[0], game[2]);
      }

      Console.WriteLine("Score for part 1 was " + sum1);
      Console.WriteLine("Score for part 2 was " + sum2);
   }

   private static int DetermineScorePart1(char opponent, char me) {
      switch (opponent) {
         case 'A': // ROCK
            switch (me) {
               case 'Y': return 8; // PAPER  
               case 'Z': return 3; // SCISSORS
               default: return 4; // ROCK
            }
         case 'B': // PAPER
            switch (me) {
               case 'X': return 1; // ROCK
               case 'Z': return 9; // SCISSORS
               default: return 5; // PAPER
            }
         default: // SCISSORS
            switch (me) {
               case 'X': return 7; // ROCK 
               case 'Y': return 2; // PAPER
               default: return 6; // SCISSORS
            }
      }
   }

   private static int DetermineScorePart2(char opponent, char me) {
      switch (opponent) {
         case 'A': // ROCK
            switch (me) {
               case 'X': return 3; // LOSE 
               case 'Y': return 4; // DRAW
               default: return 8; // WIN
            }
         case 'B': // PAPER
            switch (me) {
               case 'X': return 1; // LOSE 
               case 'Y': return 5; // DRAW
               default: return 9; // WIN
            }
         default: // SCISSORS
            switch (me) {
               case 'X': return 2; // LOSE 
               case 'Y': return 6; // DRAW
               default: return 7; // WIN
            }
      }
   }
}
