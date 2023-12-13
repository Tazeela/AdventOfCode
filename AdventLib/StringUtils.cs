namespace AdventLib;

public static class StringUtils
{
    /// <summary>
    /// Tests if the original string matches searchString at an offset.
    /// </summary>
    /// <param name="line">The full line of text we are checking.</param>
    /// <param name="searchString">The specific string we are searching for.</param>
    /// <param name="offset">The offset in line we are checking at.</param>
    /// <returns>True if the string matches, otherwise false.</returns>
    public static bool IsPartialSubstring(string line, string searchString, int offset) {
        if (line.Length < searchString.Length + offset) return false;

        for (int x = 0; x < searchString.Length; x++) {
            if (line[offset + x] != searchString[x]) return false;
        }

        return true;
    }

    /// <summary>
    /// Count the number of characters that are different between two strings of equal length;
    /// </summary>
    /// <param name="str1">First string.</param>
    /// <param name="str2">Second string.</param>
    /// <returns>A number representing the count of differences.</returns>
    public static int CountDifferences(string str1, string str2) {
        if(str1.Length != str2.Length) throw new InvalidOperationException("Must be samee length");

        int count = 0;
        for(int x = 0; x < str1.Length; x++) {
            if(str1[x] != str2[x]) count++;
        }
        return count;
    }

    /// <summary>
    /// Given a list of strings transpose them (so the first string will be the first element in each of the original strings, etc.).
    /// </summary>
    /// <param name="strings">The list of strings to transpose</param>
    /// <returns>The strings transposed</returns>
    public static IEnumerable<string> Transpose(IEnumerable<string> strings) {
        return strings.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => new string(g.ToArray()));
    }

    
    /// <summary>
    /// Replace the character at a specific position in the string.
    /// </summary>
    /// <param name="original">The original string.</param>
    /// <param name="toReplace">the character to insert</param>
    /// <param name="index">The index to insert it at.</param>
    /// <returns>The original string with the character at index changed to toReplace</returns>
    public static string ReplaceCharAt(string original, char toReplace, int index) {
        return original[..index] + toReplace + original[(index + 1)..];
    }
}
