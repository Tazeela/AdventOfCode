using System.Diagnostics;

namespace AdventLib;

/// <summary>
/// Class which provides a shell for running solutions.
/// </summary>
public static class AdventRunner {
   /// <summary>
   /// Given the list of solvers allows user to choose a solver and a filename.
   /// </summary>
   /// <param name="solversEnum"></param>
   public static void ProcessRequests(IEnumerable<AdventSolver> solversEnum) {
      // Generate a dictionary so we can lookup the solvers by name
      Dictionary<string, AdventSolver> solversMap = new Dictionary<string, AdventSolver>(StringComparer.OrdinalIgnoreCase);
      foreach (AdventSolver solver in solversEnum) {
         solversMap.Add(solver.Day, solver);
      }

      string[] args = Environment.GetCommandLineArgs();
      if (args.Length != 3 || (args.Length > 2 && !solversMap.ContainsKey(args[1]))) {
         Console.WriteLine("Must specify the day and the input file.");

         if (args.Length > 0 && solversMap.ContainsKey(args[1])) {
            var solver = solversMap[args[1]];
            Console.WriteLine(String.Format("* [{0}] Available files - {1}", args[1], String.Join(',', Directory.GetFiles(solver.DataFolder).Select(fullPath => Path.GetFileName(fullPath)))));
         } else {
            Console.WriteLine(String.Format("* Available days: {0}", String.Join(',', string.Join(',', solversMap.Keys))));
         }
      } else {
         // TODO: Check file exists?
         var solver = solversMap[args[1]];
         Stopwatch sw = Stopwatch.StartNew();
         solver.Solve(args[2]);
         sw.Stop();
         Console.WriteLine(String.Format("Solved in {0}ms", sw.ElapsedMilliseconds));
      }
   }
}
