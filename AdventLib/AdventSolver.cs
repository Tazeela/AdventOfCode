namespace AdventLib;
using System.Text;

/// <summary>
/// Base implementation for a solver which supports streaming files from the data folder.
/// </summary>
public abstract class AdventSolver {
    /// <summary>
    /// Gets the Day this solver is for.
    /// </summary>
    public abstract string Day { get; }

    /// <summary>
    /// Gets the filename provided for input.
    /// </summary>
    public string? InputFileName {
        get;
        set;
    }

    /// <summary>
    /// Gets the path to the data folder which should be in data/{day}/
    /// </summary>
    public string DataFolder {
        get {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", Day);
        }
    }

    /// <summary>
    /// The solve function.
    /// </summary>
    public abstract void Solve();

    /// <summary>
    /// Helper which streams the text from the file line by line.
    /// </summary>
    /// <param name="fileName">The name of the file to stream data from.</param>
    /// <returns>A stream of lines from the file.</returns>
    public IEnumerable<string> ReadInputAsIEnumerable() {
        if (InputFileName == null) throw new Exception("Missing file name");

        string fullPath = Path.Combine(this.DataFolder, this.InputFileName);

        using var fileStream = File.OpenRead(fullPath);
        using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

        string? line;
        while ((line = streamReader.ReadLine()) != null) {
            yield return line;
        }
    }

    /// <summary>
    /// Helper which reads the input as a List<string>
    /// </summary>
    /// <returns>The input as a list of string.</returns>
    public List<string> ReadInputAsList() {
        return ReadInputAsIEnumerable().ToList();
    }

    /// <summary>
    /// Helper which reads all of the input and groups it up by a delimeter.
    ///         line 1
    ///         line 2
    ///         line 3
    ///         
    ///         line 4
    ///         line 5
    ///          
    /// becomes [["line 1", "line 2", "line 3"], ["line 4", "line 5]]
    /// </summary>
    /// <param name="element">The element to group by, empty line is the default.</param>
    /// <returns>Input chunked by the element.</returns>
    public IEnumerable<IEnumerable<string>> ReadInputChunkByElement(string element = "") {
        return IEnumerableUtils.ChunkByElement(ReadInputAsIEnumerable(), element);
    }

    /// <summary>
    /// Helper which reads all of the input as an array of characters
    /// </summary>
    /// <returns></returns>
    public char[][] ReadInputAsCharArray() {
        return ReadInputAsIEnumerable().ToCharArray();
    }
}