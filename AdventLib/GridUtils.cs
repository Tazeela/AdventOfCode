namespace AdventLib;

public static class GridUtils {

    public static IEnumerable<(int, int)> Adjacent(this (int, int) origin, bool corners = false) {
        yield return (origin.Item1 + 1, origin.Item2);
        yield return (origin.Item1 - 1, origin.Item2);
        yield return (origin.Item1, origin.Item2 - 1);
        yield return (origin.Item1, origin.Item2 + 1);

        if (corners) {
            yield return (origin.Item1 + 1, origin.Item2 + 1);
            yield return (origin.Item1 + 1, origin.Item2 - 1);
            yield return (origin.Item1 - 1, origin.Item2 + 1);
            yield return (origin.Item1 - 1, origin.Item2 - 1);
        }
    }
}