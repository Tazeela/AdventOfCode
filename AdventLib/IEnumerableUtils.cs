namespace AdventLib;

public static class IEnumerableUtils {

    
    /// <summary>
    /// Calculate the LCM 
    /// </summary>
    /// <param name="numbers">An enumerable of numbers.</param>
    /// <returns>The least common denominator among all numbers.</returns>
    public static long Lcm(IEnumerable<long> numbers) {
        return numbers
            .Skip(1)
            .Aggregate(numbers.First(), MathUtils.Lcm);
    }

    /// <summary>
    /// Given an enumerable chunk it up by elements.
    /// </summary
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="elements">The collection of elements to chunk.</param>
    /// <param name="seperator">The seperator</param>
    /// <returns>Chunks of all items.</returns>
    public static IEnumerable<IEnumerable<T>> ChunkByElement<T>(IEnumerable<T> elements, T seperator) {
        List<T> current = [];
        foreach(var next in elements) {
            if(next == null || next.Equals(seperator)) {
                yield return current;
                current = [];
            } else {
                current.Add(next);
            }
        }
        yield return current;
    }

    public static void PrintArray<T>(string name, IEnumerable<T> elements, string seperator = ",") {
        Console.WriteLine(string.Format("[{0}] - {1}", name, string.Join(",", elements)));
    }

    public static void PrintMatrix<T>(string name, IEnumerable<IEnumerable<T>> elements, string seperator = ",") {
        Console.WriteLine(string.Format("[{0}]", name));
        foreach(var element in elements) {
            Console.WriteLine(string.Join(seperator, element));
        }
    }
}