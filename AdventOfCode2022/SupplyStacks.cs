using System.Diagnostics;
using System.Text;
using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 5](https://adventofcode.com/2022/day/5)
/// </summary>
public class SupplyStacks : AdventSolver {
    public override string Day => "day5";

    public override void Solve(string filename) {
        Console.WriteLine("Solution for part 1 is: " + SortStacks(filename, 1));
        Console.WriteLine("Solution for part 2 is: " + SortStacks(filename, int.MaxValue));
    }

    /// <summary>
    /// Bulk of the work, does the stack sorting and returns a string with the top item in each stack.
    /// </summary>
    /// <param name="filename">The file to load the game from.</param>
    /// <param name="craneLoadSize">The number of items the crane can move at once</param>
    /// <returns>A string that represents the top item in each stack after all operations.</returns>
    private string SortStacks(string filename, int craneLoadSize) {
        Dictionary<int, LinkedList<char>> stacks = new Dictionary<int, LinkedList<char>>();

        bool moving = false;

        foreach (string game in ReadLines(filename)) {
            if (!moving) {
                // There is a line which labels the number of crates which we use to indicate we are done building
                if (char.IsAsciiDigit(game[1])) {
                    Debug.WriteLine("Done building stacks");
                    PrintStacks(stacks);
                    moving = true;
                    continue;
                }

                int crane = 1; // index the cranes by 1 so the moves happen correctly without needing to modify the value in the file
                int index = 1;

                while (index < game.Length) {
                    char item = game[index];
                    if (item != ' ') {
                        Debug.WriteLine("Adding " + item + " to " + crane);
                        getOrInit(stacks, crane).AddLast(item);
                    }
                    crane += 1;
                    index += 4;
                }
            } else if (!string.IsNullOrEmpty(game)) {
                string[] commands = game.Split(' ');
                int numCrates = int.Parse(commands[1]);
                int startStackNum = int.Parse(commands[3]);
                int endStackNum = int.Parse(commands[5]);

                var startStack = getOrInit(stacks, startStackNum);
                var endStack = getOrInit(stacks, endStackNum);

                Debug.WriteLine(game);

                for (int x = 0; x < numCrates; x += craneLoadSize) {
                    Stack<char> loaded = new Stack<char>();
                    for (int y = 0; y < Math.Min(craneLoadSize, numCrates - x); y++) {
                        if (startStack.Count > 0) {
                            char item = startStack.First();
                            loaded.Push(item);
                            startStack.RemoveFirst();
                        }
                    }

                    while (loaded.Count > 0) {
                        char item = loaded.Pop();
                        endStack.AddFirst(item);
                    }
                }

                //PrintStacks(stacks);
            }
        }

        StringBuilder sb = new StringBuilder();
        for (int x = 0; x < stacks.Count; x++) {
            sb.Append(stacks[x + 1].FirstOrDefault());
        }

        return sb.ToString();
    }

    /// <summary>
    /// Helper which displays the stacks in output.
    /// </summary>
    /// <param name="stacks">The stacks</param>
    private static void PrintStacks(Dictionary<int, LinkedList<char>> stacks) {
        int max = 0;
        foreach (var kvp in stacks) {
            max = Math.Max(kvp.Value.Count, max);
        }

        Debug.WriteLine("-----------------");
        for (int x = 0; x < max; x++) {
            for (int y = 0; y < stacks.Count; y++) {
                Debug.Write("[" + stacks[y + 1].Skip(x).FirstOrDefault() + "]");
            }

            Debug.WriteLine("");
        }
        Debug.WriteLine("-----------------");
    }


    /// <summary>
    /// Helper which gets a stack from the collection, or adds it if its not already there.
    /// </summary>
    /// <param name="stacks">The stacks.</param>
    /// <param name="stackNum">The stack number.</param>
    /// <returns>The stack for stackNum.</returns>
    private static LinkedList<char> getOrInit(Dictionary<int, LinkedList<char>> stacks, int stackNum) {
        if (!stacks.ContainsKey(stackNum)) {
            stacks.Add(stackNum, new LinkedList<char>());
        }
        return stacks[stackNum];
    }
}