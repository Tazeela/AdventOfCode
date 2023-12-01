using System.Drawing;
using System.Globalization;
using AdventLib;

namespace AdventOfCode2022;

/// <summary>
/// Solver for [day 7](https://adventofcode.com/2022/day/7)
/// </summary>
public class NoSpace : AdventSolver {
   public override string Day => "day7";

   public override void Solve(string filename) {
      int count1 = 0;
      int count2 = 0;

      GraphNode root = new GraphNode("/", null);
      GraphNode current = root;
      foreach (string line in ReadLines(filename)) {
         if (line.StartsWith("$ cd ")) {
            string newPath = line.Substring(5);
            if (newPath == "/") {
               current = root;
            } else if (newPath == "..") {
               if (current.Parent != null) {
                  current = current.Parent;
               }
            } else {
               current = current.GetFolder(newPath);
            }
         } else if (!line.StartsWith("$")) {
            string[] lineSplit = line.Split(" ");
            if (lineSplit[0] != "dir") {
               int size = int.Parse(lineSplit[0]);
               current.AddFile(lineSplit[1], size);
            }
         }
      }

      root.Print(0);

      Console.WriteLine("Solution for part 1 is: " + root.CalculateSizes().Item2);
      Console.WriteLine("Solution for part 2 is: " + root.closestWithoutGoingOver().Item2);
   }

   public class GraphNode {
      public int Size { get; set; }
      public string Name { get; set; }

      public GraphNode Parent { get; set; }

      public bool IsDirectory {
         get {
            return children.Count != 0;
         }
      }

      public GraphNode(string name, GraphNode parent, int size = 0) {
         this.Name = name;
         Parent = parent;
         this.Size = size;
      }

      public Dictionary<string, GraphNode> children = new Dictionary<string, GraphNode>();

      public GraphNode GetFolder(string path) {
         if (children.ContainsKey(path)) {
            return children[path];
         } else {
            GraphNode graphNode = new GraphNode(path, this);
            children[path] = graphNode;
            return graphNode;
         }
      }

      public void AddFile(string filename, int size) {
         this.Size += size;
         //children[filename] = new GraphNode(filename, this, size); // dont need to add children but its helpful for visualizing
      }

      public (int, int) CalculateSizes(int target = 100000) {
         if (this.IsDirectory) {
            // calculate size of children
            int sumTotal = this.Size;
            int sumBelowTarget = 0;
            foreach ((int childSumTotal, int childSumBelowTarget) in children.Values.Select(child => child.CalculateSizes())) {
               sumTotal += childSumTotal;
               sumBelowTarget += childSumBelowTarget;
            }

            if (sumTotal <= target) {
               sumBelowTarget += sumTotal;
            }

            return (sumTotal, sumBelowTarget);
         } else {
            return (this.Size, 0);
         }
      }

      public (int, int) closestWithoutGoingOver(int target = 24933642) {
         if (this.IsDirectory) {
            // calculate size of children
            int sumTotal = this.Size;
            int currentBest = int.MaxValue;
            foreach ((int childSumTotal, int childCurrentBest) in children.Values.Select(child => child.CalculateSizes())) {
               sumTotal += childSumTotal;
               currentBest = Math.Min(currentBest, childCurrentBest);
            }

            if (sumTotal >= target) {
               currentBest = Math.Min(currentBest, sumTotal);
            }

            return (sumTotal, currentBest);
         } else {
            return (this.Size, int.MaxValue);
         }
      }

      public void Print(int depth) {
         for(int x = 0; x < depth; x++) {
            Console.Write("\t");
         }

         var size = this.CalculateSizes();
         Console.WriteLine(this.Name + " - " + size.Item1 + " - " + size.Item2);
         foreach(var child in this.children.Values) {
            child.Print(depth + 1);
         }
      }
   }
}