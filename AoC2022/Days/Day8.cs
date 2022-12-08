using System.Text;

namespace AoC2022.Days;

public static class Day8
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(8);
        await Run(input);
    }

    private static async Task Run(string[] input)
    {
        var visible = CountVisible(input);
        Console.WriteLine($"Part One - {visible}");

        var scenic = GetHighestScenicScore(input);
        Console.WriteLine($"Part Two - {scenic}");
    }

    private static int CountVisible(string[] input)
    {
        var totalVisible = 0;
        for (int y = 1; y < input.Length - 1 ; y++)
        {
            var line = input[y];
            for (int x = 1; x < input[x].Length - 1; x++)
            {
                var column = GetColumn(input, x);
                var isVisibleLeft = line[..x].All(i => int.Parse(i.ToString()) > int.Parse(line[x].ToString()));
                var isVisibleRight = line[(x + 1)..].All(i => int.Parse(i.ToString()) < int.Parse(line[x].ToString()));
                var isVisibleUp = column[..y].All(i => int.Parse(i.ToString()) < int.Parse(column[y].ToString()));
                var isVisibleDown = column[(y + 1)..].All(i => int.Parse(i.ToString()) < int.Parse(column[y].ToString()));
                if (isVisibleLeft || isVisibleRight || isVisibleUp || isVisibleDown) totalVisible++;
            }
        }
        return totalVisible + CountEdges(input);
    }

    private static int CountBackwards(string line, int stopAtHeight)
    {
        var treeCount = 1;
        for (int i = line.Length -1; i >= 0; i--)
        {            
            if (int.Parse(line[i].ToString()) >= stopAtHeight)
            {
                return treeCount;
            }
            treeCount++;
        }
        return line.Length;
    }

    private static int CountForwards(string line, int stopAtHeight)
    {
        var treeCount = 1;
        for (int i = 0; i < line.Length -1; i++)
        {
            if (int.Parse(line[i].ToString()) >= stopAtHeight)
            {
                return treeCount;
            }
            treeCount++;
        }
        return line.Length;
    }

    private static int CountEdges(string[] input)
    {
        return (input[0].Length * 2) + ((input.Length - 2) * 2);
    }

    private static string GetColumn(string[] input, int x)
    {
        var col = new StringBuilder();
        foreach (var line in input)
        {
            col.Append(line[x]);
        }
        return col.ToString();
    }

    private static int GetHighestScenicScore(string[] input)
    {
        var highestScore = 0;
        for (int y = 1; y < input.Length - 1; y++)
        {
            var line = input[y];
            for (int x = 1; x < input[x].Length - 1; x++)
            {
                var column = GetColumn(input, x);
                var countLeft = CountBackwards(line[..x], int.Parse(line[x].ToString()));
                var countRight = CountForwards(line[(x + 1)..], int.Parse(line[x].ToString()));
                var countUp = CountBackwards(column[..y], int.Parse(column[y].ToString()));
                var countDown = CountForwards(column[(y + 1)..], int.Parse(column[y].ToString()));
                var scenicScore = countLeft * countRight * countUp * countDown;
                if (scenicScore > highestScore) highestScore = scenicScore;
            }
        }
        return highestScore;
    }
}
