namespace AdventLib;

public static class AggregationUtils {

    /// <summary>
    /// Calculate the greatest common denominator between two numbers.
    /// </summary>
    /// <param name="a">The first num</param>
    /// <param name="b">The second num</param>
    /// <returns>The greatest common denominator.</returns>
    public static long Gcd(long a, long b) {
        while (a != 0 && b != 0) {
            if (a > b) {
                a %= b;
            } else {
                b %= a;
            }
        }

        return a | b;
    }

    /// <summary>
    /// Calculate the least common multiple between two numbers.
    /// </summary>
    /// <param name="a">The first num</param>
    /// <param name="b">The second num</param>
    /// <returns>The least common multiple.</returns>
    public static long Lcm(long a, long b) {
        return a * b / Gcd(a, b);
    }

    /// <summary>
    /// Calculate the LCM 
    /// </summary>
    /// <param name="numbers">An enumerable of numbers.</param>
    /// <returns>The least common denominator among all numbers.</returns>
    public static long Lcm(IEnumerable<long> numbers) {
        return numbers
            .Skip(1)
            .Aggregate(numbers.First(), Lcm);
    }

}