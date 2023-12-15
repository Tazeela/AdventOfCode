namespace AdventLib;

public static class MathUtils {
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
    /// Calcutate the distance between two points, if you can only travel horizontally and vertically.
    /// </summary>
    /// <param name="p1">Start point.</param>
    /// <param name="p2">End point.</param>
    /// <returns>The distance between the points.</returns>
    public static long CalculateDistance((long, long) p1, (long, long) p2) {
        return Math.Abs(p2.Item1 - p1.Item1) + Math.Abs(p2.Item2 - p1.Item2);
    }

}