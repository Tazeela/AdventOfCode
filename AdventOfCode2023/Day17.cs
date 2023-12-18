using System.Data;
using AdventLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 17](https://adventofcode.com/2023/day/17)
/// </summary>
public class Day17 : AdventSolver {
    public override string Day => "day17";


    public override void Solve() {
        int[][] map = ReadInputAsIEnumerable().Select(s => s.Select(c => (int)Char.GetNumericValue(c)).ToArray()).ToArray();
        var traverser = new MapTraverser(map);
        Console.WriteLine("Solution for part 1 is: " + traverser.Traverse(1,3)?.Cost);
        Console.WriteLine("Solution for part 2 is: " + traverser.Traverse(4,10)?.Cost);
    }

    public class MapTraverser(int[][] map) {
        private const int fromRow = 0x01;
        private const int fromCol = 0x10;

        public Traversal? Traverse(int minSteps, int maxSteps) {
            int[][] visited;
            visited = new int[map.Length][];
            foreach (int idx in visited.Indexes()) visited[idx] = new int[map[idx].Length];

            PriorityQueue<Traversal, int> toDo = new();
            toDo.Enqueue(new(0, 0, 0, 0, null), 0);
            while (toDo.Count > 0) {
                Traversal currentNode = toDo.Dequeue();
                if (IsDone(currentNode)) return currentNode;

                if (!HasVisited(currentNode, visited)) {
                    Visit(currentNode, visited);
                    foreach ((var outboundRow, var outBoundCol, var outboundDirection, var outboundCost) in GetNext(currentNode, minSteps, maxSteps)) {
                        Traversal nextNode = new(outboundRow, outBoundCol, outboundDirection, outboundCost, currentNode);
                        toDo.Enqueue(nextNode, nextNode.Cost);
                    }
                }
            }

            return null;
        }

        public bool IsDone(Traversal node) {
            return node.Col == map[0].Length - 1 && node.Row == map.Length - 1;
        }

        public static bool HasVisited(Traversal node, int[][] visited) {
            return (node.Direction & visited[node.Row][node.Col]) == 0;
        }

        public static void Visit(Traversal node, int[][] visited) {
            visited[node.Row][node.Col] |= node.Direction;
        }

        public IEnumerable<(int, int, int, int)> GetNext(Traversal node, int minSteps, int maxSteps) {
            if (node.Direction != fromRow) {
                int newCost = node.Cost;
                for (int x = 1; x < maxSteps + 1 && node.Col + x < map[0].Length; x++) {
                    newCost += map[node.Row][node.Col + x];
                    if (x >= minSteps) yield return new(node.Row, node.Col + x, fromRow, newCost);
                }

                newCost = node.Cost;
                for (int x = 1; x < maxSteps + 1 && node.Col - x >= 0; x++) {
                    newCost += map[node.Row][node.Col - x];
                    if (x >= minSteps) yield return new(node.Row, node.Col - x, fromRow, newCost);
                }
            }

            if (node.Direction != fromCol) {
                int newCost = node.Cost;
                for (int x = 1; x < maxSteps + 1 && node.Row - x >= 0; x++) {
                    newCost += map[node.Row - x][node.Col];
                    if (x >= minSteps) yield return new(node.Row - x, node.Col, fromCol, newCost);
                }

                newCost = node.Cost;
                for (int x = 1; x < maxSteps + 1 && node.Row + x < map.Length; x++) {
                    newCost += map[node.Row + x][node.Col];
                    if (x >= minSteps) yield return new(node.Row + x, node.Col, fromCol, newCost);
                }
            }
        }
    }

    public class Traversal(int row, int col, int direction, int cost, Day17.Traversal? last) {
        public int Cost { get; set; } = cost;
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;
        public int Direction { get; set; } = direction;
        public Traversal? Last { get; set; } = last;

        public void PrintPath() {
            if (Last != null) Last.PrintPath();
            Console.WriteLine(this);
        }

        public override string ToString() {
            return string.Format("[{0},{1}] - cost = {2} from  + {3}", Row, Col, Cost, Direction);
        }
    }
}