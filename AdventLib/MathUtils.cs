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
    
    /// <summary>
    /// Calcualte the interior area of a shape given all points on the exterior using Shoelace formula.
    /// E.G. [(0,0),(0,1),(0,2),(1,2),(2,2),(2,1),(2,0),(1,0),(0,0)]
    /// </summary>
    /// <param name="points">The list of points.</param>
    /// <returns>The total interior area.</returns>
    public static double InteriorArea(List<(long, long)> points) {
        int n = points.Count;
        double a = 0.0;
        for (int i = 0; i < n - 1; i++) {
            a += points[i].Item1 * points[i + 1].Item2 - points[i + 1].Item1 * points[i].Item2;
        }
        return Math.Abs(a + points[n - 1].Item1 * points[0].Item2 - points[0].Item1 * points[n - 1].Item2) / 2.0;
    }

    /// <summary>
    /// Calculate the total area of a polygon using Picks Theorum
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static double PolygonArea(List<(long, long)> points) {
        return (long)(InteriorArea(points) + 1 - (points.Count() / 2));
    }
}