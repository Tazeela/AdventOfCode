namespace AdventOfCode2022;

using System.Diagnostics;
using AdventLib;

/// <summary>
/// Solver for https://adventofcode.com/2022/day/1
/// </summary>
public class ElfFood : AdventSolver {
   public override string Day => "day1";

   public override void Solve(string filename) {
      int limit = 3;
      int current = 0;

      // Initialize with a capacity of limit + 1 so we have room to add an extra and remove it
      PriorityQueue<int, int> priorityQueue = new(limit + 1);

      foreach (string text in base.ReadLines(filename)) {
         if (String.IsNullOrEmpty(text)) {
            EnqueueIfRoom(priorityQueue, current, limit);
            current = 0;
         } else {
            current += int.Parse(text);
         }
      }

      // Need to make sure the last item is added
      EnqueueIfRoom(priorityQueue, current, limit);

      int topN = 0;
      int most = 0;
      while (priorityQueue.Count > 0) {
         int next = priorityQueue.Dequeue();
         most = next;
         topN += next;
         Console.WriteLine("Next = " + next);
      }

      Console.WriteLine("Part 1 Solution is: " + most);
      Console.WriteLine("Part 2 Solution is: " + topN);
   }

   /// <summary>
   /// Helper which adds an item into a sorted list, removing the lowest item if the limit is exceeded, 
   /// </summary>
   /// <param name="queue">The queue to add the item to.</param>
   /// <param name="item">The item to try and add to the queue.</param>
   /// <param name="limit">The number of items to keep.</param>
   private static void EnqueueIfRoom(PriorityQueue<int, int> queue, int item, int limit) {
      if (queue.Count >= limit) {
         Debug.WriteLine("Enqueueing " + item);
         Debug.WriteLine("Dequeued : " + queue.EnqueueDequeue(item, item));
      } else {
         Debug.WriteLine("Enqueueing " + item);
         queue.Enqueue(item, item);
      }
   }
}
