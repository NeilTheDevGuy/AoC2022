namespace AoC2022.Utils;

public static class StringManipulation
{
    public static string ReplaceCharAt(this string input, int index, char newChar)
    {
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }
}