namespace AdventLib;
using System.Text;

/// <summary>
/// Base implementation for a solver which supports streaming files from the data folder.
/// </summary>
public abstract class AdventSolver
{
   /// <summary>
   /// Gets the Day this solver is for.
   /// </summary>
   public abstract string Day { get; }

   /// <summary>
   /// Gets the path to the data folder which should be in data/{day}/
   /// </summary>
   public string DataFolder
   {
      get
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", Day);
      }
   }

   /// <summary>
   /// The solve function.
   /// </summary>
   /// <param name="filename"></param>
   public abstract void Solve(string filename);

   /// <summary>
   /// Helper which streams the text from the file line by line.
   /// </summary>
   /// <param name="fileName">The name of the file to stream data from.</param>
   /// <returns>A stream of lines from the file.</returns>
   public IEnumerable<string> readLines(string fileName)
   {
      string fullPath = Path.Combine(this.DataFolder, fileName);

      using (var fileStream = File.OpenRead(fullPath))
      {
         using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
         {
            string? line;
            while ((line = streamReader.ReadLine()) != null)
            {
               yield return line;
            }
         }
      }
   }
}
