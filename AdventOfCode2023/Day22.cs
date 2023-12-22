using AdventLib;

namespace AdventOfCode2023;

/// <summary>
/// Solver for [day 22](https://adventofcode.com/2023/day/22)
/// </summary>
public class Day22 : AdventSolver {
    public override string Day => "day22";

    public override void Solve() {
        List<Brick> bricks = [.. ReadInputAsIEnumerable().Select(s => new Brick(s)).OrderBy(s => s.Z1)]; 

        BuildGraph(bricks);
        Drop(bricks);
        
        Console.WriteLine("Solution for part 1 is: " + CountCanDisentigrate(bricks));
        Console.WriteLine("Solution for part 2 is: " + CountHowManyFall(bricks));
    }

    public static void BuildGraph(List<Brick> bricks) {
        Dictionary<string, Brick> highestBrick = [];

        foreach (Brick currentBrick in bricks) {
            HashSet<Brick> below = [];
            foreach(var coord in currentBrick.GetAllCoords()) {
                if(highestBrick.TryGetValue(coord, out Brick? value)) {
                    below.Add(value);
                }

                highestBrick[coord] = currentBrick;
            }

            currentBrick.Below.AddRange(below);
            foreach(var brickBelow in below) {
                brickBelow.Above.Add(currentBrick);
            }
        }
    }


    public static int CountCanDisentigrate(List<Brick> bricks) {
        int count = 0;

        foreach(var brick in bricks) {
            bool canDisentigrate = true;
            foreach(var above in brick.GetDirectlyAbove()) {
                if(above.GetDirectlyBelow().Count() == 1) {
                    canDisentigrate = false;
                }
            }

            if(canDisentigrate) count++;
        }

        return count;
    }

    public static int CountHowManyFall(List<Brick> bricks) {
        int count = 0;

        foreach(Brick brick in bricks) {
            List<Brick> removed = [];
            Queue<Brick> todo = [];
            todo.Enqueue(brick);

            while(todo.Count > 0) {
                var nextBrick = todo.Dequeue();
                removed.Add(nextBrick);
                foreach(var aboveNext in nextBrick.GetDirectlyAbove()) {
                    if(aboveNext.GetDirectlyBelow().All(removed.Contains)) {
                        todo.Enqueue(aboveNext);
                        count++;
                    }
                }
            }
        }
        return count;
    }

    public static void Drop(List<Brick> bricks) {
        // Sort all of the bricks
        foreach(var currentBrick in bricks) {
            int newHeight = currentBrick.Below.Count == 0 ? 1 : currentBrick.Below.Max(b => b.Z2) + 1;
            currentBrick.Z2 = currentBrick.Z2 - currentBrick.Z1 + newHeight ;
            currentBrick.Z1 = newHeight;
        }
    }

    public class Brick {
        public int X1, Y1, Z1, X2, Y2, Z2;

        public string Name;

        public bool Settled = false;

        public readonly List<Brick> Above = [];
        public readonly List<Brick> Below = [];

        public Brick(string line) {
            var split = line.Split('~');

            var pos1 = split[0].Split(',');
            X1 = int.Parse(pos1[0]);
            Y1 = int.Parse(pos1[1]);
            Z1 = int.Parse(pos1[2]);

            var pos2 = split[1].Split(',');
            X2 = int.Parse(pos2[0]);
            Y2 = int.Parse(pos2[1]);
            Z2 = int.Parse(pos2[2]);
        }

        public IEnumerable<Brick> GetDirectlyBelow() {
            return Below.Where(b => b.Z2 + 1 == Z1);
        }

        public IEnumerable<Brick> GetDirectlyAbove() {
            return Above.Where(b => Z2 + 1 == b.Z1);
        }

        public IEnumerable<string> GetAllCoords() {
            for (int x = X1; x <= X2; x++) {
                for (int y = Y1; y <= Y2; y++) {
                    yield return x + "," + y;
                }
            }
        }

        public override string ToString() {
            return string.Format("([{0}] {1},{2},{3}~{4},{5},{6})", Name, X1, Y1, Z1, X2, Y2, Z2);
        }
    }
}