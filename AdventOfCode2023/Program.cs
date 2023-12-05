using AdventLib;
using AdventOfCode2023;

Console.WriteLine("Hello, World 2023!");
IEnumerable<AdventSolver> solvers = new AdventSolver[] { new Day1(), new Day2(), new Day3(), new Day4(), new Day5() };


AdventRunner.ProcessRequests(solvers);