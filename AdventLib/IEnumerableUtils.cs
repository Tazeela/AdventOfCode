namespace AdventLib;

public static class IEnumerableUtils {
    /// <summary>
    /// Calculate the LCM 
    /// </summary>
    /// <param name="numbers">An enumerable of numbers.</param>
    /// <returns>The least common denominator among all numbers.</returns>
    public static long Lcm(this IEnumerable<long> numbers) {
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
    public static IEnumerable<IEnumerable<T>> ChunkByElement<T>(this IEnumerable<T> elements, T seperator) {
        List<T> current = [];
        foreach (var next in elements) {
            if (next == null || next.Equals(seperator)) {
                yield return current;
                current = [];
            } else {
                current.Add(next);
            }
        }
        yield return current;
    }

    /// <summary>
    /// Given an an list of lists, transpose the items  (so the first list will be the first element in each of the original lists, etc.).
    /// </summary>
    /// <param name="elements">The list of items to transpose</param>
    /// <returns>The elements transposed</returns>
    public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> elements) {
        return elements.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item);
    }

    public static IEnumerable<int> Indexes<T>(this T[] elements) {
        for(int x = 0; x < elements.Length; x++) {
            yield return x;
        }
    }

    public static void PrintArray<T>(this IEnumerable<T> elements, string name, string seperator = ",") {
        Console.WriteLine(string.Format("[{0}] - {1}", name, string.Join(",", elements)));
    }

    public static void PrintMatrix<T>(this IEnumerable<IEnumerable<T>> elements, string name, string seperator = ",") {
        Console.WriteLine(string.Format("[{0}]", name));
        foreach (var element in elements) {
            Console.WriteLine(string.Join(seperator, element));
        }
    }
}