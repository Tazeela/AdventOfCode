// See https://aka.ms/new-console-template for more information
using AdventLib;
using AdventOfCode2022;

Console.WriteLine("Hello, World 2022!");
IEnumerable<AdventSolver> solvers = new AdventSolver[] { new ElfFood(), new RockPaperScissors(), new RucksackReorg(), new CampCleanup(), new SupplyStacks(), new TuningTrouble(), new NoSpace() };


AdventRunner.ProcessRequests(solvers);