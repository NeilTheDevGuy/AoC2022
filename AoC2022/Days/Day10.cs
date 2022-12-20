namespace AoC2022.Days;

public static class Day10
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(10);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var instructionsList = new Dictionary<int, int>();
        var cycle = 1;
        var x = 1;
        foreach (var line in input)
        {
            cycle++;
            if (line != "noop")            
            {
                var val = int.Parse(line.Split(" ")[1]);
                instructionsList.Add(cycle + 2,val);
                x += val;
                Console.WriteLine($"Current cycle: {cycle}. Added {val} for cycle {cycle + 2}, making X {x}");    
                cycle++;
            }

        }

        var maxCycle = cycle + 2;
        cycle = 1;
        x = 1;

        var cyclesToCheck = new List<int> { 20, 60, 100, 140, 180, 220 };
        var strengths = new List<int>();

        while (cycle <= maxCycle)
        {
            Console.WriteLine($"Checking cycle {cycle}");
            
            if (instructionsList.TryGetValue(cycle +1, out int val))
            {                                
                x += val;
                Console.WriteLine($"Adding {val} to x, making it {x}");
            }
            if (cyclesToCheck.Contains(cycle))
            {
                Console.WriteLine($"Added strength: {x} X {cycle} = {x * cycle}");
                strengths.Add(x * cycle);
            }
            cycle++;
        }

        var result = strengths.Sum(x => x);
        Console.WriteLine($"Part One - x:{result}");
    }

    private static async Task PartTwo(string[] input)
    {
        var instructionsList = new Dictionary<int, int>();
        var cycle = 1;
        var x = 1;
        foreach (var line in input)
        {
            cycle++;
            if (line != "noop")
            {
                var val = int.Parse(line.Split(" ")[1]);
                instructionsList.Add(cycle , val);
                x += val;
                Console.WriteLine($"Current cycle: {cycle}. Added {val} for cycle {cycle + 2}, making X {x}");
                cycle++;
            }

        }

        var maxCycle = cycle + 2;
        cycle = 0;
        x = 1;

        var screenWidths = new List<int> { 40, 80, 120, 160, 200, 240 };
        var screen = new string[40, 7];

        int screenX = 0, screenY = 0, cycleOffset = 0;

        while (cycle <= maxCycle)
        {
            var offsetCycle = cycle + cycleOffset;
            if (instructionsList.TryGetValue(cycle, out int val))
            {
                x += val;           
            }
            if (offsetCycle == x || offsetCycle == x - 1 || offsetCycle == x + 1)
            {
                screen[screenX, screenY] = "#";
            }
            else
            {
                screen[screenX, screenY] = ".";
            }

            if (screenWidths.Contains(cycle +1))
            {
                screenX = 0;
                screenY++;
                cycleOffset -= 40;
            }
            else
            {
                screenX++;
            }

            cycle++;
        }

        for (int drawY = 0; drawY <= 6; drawY++)
        {
            for (int drawX = 0; drawX <= 39; drawX++)
            {                
                Console.Write(screen[drawX, drawY]);
            }
            Console.Write(Environment.NewLine);
        }
    }
}
