using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;

namespace AoC2022.Days;

public static class Day13 //UNFINISHED
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(13);
        await Run(input); //5196, 22134
    }

    public static async Task Run(string[] input)
    {
        var packets = input.Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        
        var packetPairs = packets.Chunk(2);
        var validPackets = packetPairs
            .Select((pair, idx) => Compare(pair[0], pair[1]) == -1 ? idx + 1 : 0)
            .Where(i => i != 0).ToList();

        Console.WriteLine($"Part 1: {validPackets.Sum()}");

        var part2Packets = packets.ToList();
        part2Packets.Add("[[2]]");
        part2Packets.Add("[[6]]");
        part2Packets.Sort(Compare);
        var d1 = part2Packets.IndexOf("[[2]]") + 1;
        var d2 = part2Packets.IndexOf("[[6]]") + 1;
        Console.WriteLine($"Part 2: {d1 * d2}");
    }

    public static (string item, string remain) GetItem(string str)
    {
        if (char.IsDigit(str[0]))
        {
            var item = new string(str.TakeWhile(char.IsDigit).ToArray());
            var next = item.Length == str.Length ? item.Length : item.Length + 1;
            return (item, str[next..]);
        }

        var depth = 0;
        var pos = 0;

        while (!(depth == 0 && (pos == str.Length || str[pos] == ',')))
        {
            if (str[pos] == '[') depth++;
            if (str[pos] == ']') depth--;
            pos++;
        }
        return pos == str.Length ? (str, string.Empty) : (str[..(pos)], str[(pos + 1)..]);
    }

    public static int Compare(string packet1, string packet2)
    {
        while (packet1.Length > 0 && packet2.Length > 0)
        {
            var item1 = GetItem(packet1);
            var item2 = GetItem(packet2);
            if (string.IsNullOrEmpty(item1.item)) return -1;
            if (string.IsNullOrEmpty(item2.item)) return 1;
            if (item1.item.All(char.IsDigit) && item2.item.All(char.IsDigit))
            {
                var n1 = int.Parse(item1.item);
                var n2 = int.Parse(item2.item);
                if (n1 < n2) return -1;
                if (n1 > n2) return 1;
            }
            else
            {
                if (item1.item[0] == '[' ^ item2.item[0] == '[')
                {
                    item1.item = ConvertToList(item1.item);
                    item2.item = ConvertToList(item2.item);
                }
                var compare = Compare(item1.item[1..^1], item2.item[1..^1]);
                if (compare != 0) return compare;
            }
            packet1 = item1.remain;
            packet2 = item2.remain;
        }
        return packet2.Length > 0 ? -1 : packet1.Length > 0 ? 1 : 0;
    }

    public static string ConvertToList(string str) => str.All(char.IsDigit) ? $"[{str}]" : str;
}
