using System.Numerics;

namespace AoC2022.Days;

public static class Day11
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(11);
        var result1 = await PartOne(input);
        Console.WriteLine($"Part One - {result1}");

        var result2 = await PartTwo(input);
        Console.WriteLine($"Part Two - {result2}");
    }

    private static async Task<long> PartOne(string[] input)
    {
        var monkeys = BuildMonkeys(input);
        var round = 1;
        var maxRounds = 20;
        while (round <= maxRounds) 
        {
            for (int i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                foreach (var worryLevel in monkey.Items.ToList())
                {
                    monkey.Inspections++;
                    var newWorryLevel = GetNewWorryLevel(monkey, worryLevel, true);
                    var toMonkey = newWorryLevel % monkey.Test == 0 ? monkeys.First(m => m.Id == monkey.IfTestTrue) : monkeys.First(m => m.Id == monkey.IfTestFalse);
                    monkey.Items.Remove(worryLevel);
                    toMonkey.Items.Add(newWorryLevel);
                }
            }
            round++;
        }
        var mostActive = monkeys.OrderByDescending(o => o.Inspections).Take(2).Select(s => s.Inspections).ToList();
        var result = mostActive[0] * mostActive[1];
        
        return result;
    }

    private static async Task<long> PartTwo(string[] input)
    {
        var monkeys = BuildMonkeys(input);
        var round = 1;
        var worryFactor = monkeys.Aggregate(1L, (w, m) => w * m.Test);           

        while (round <= 10000)
        {
            for (int i = 0; i < monkeys.Count; i++)
            {
                var monkey = monkeys[i];
                foreach (var worryLevel in monkey.Items.ToList())
                {
                    monkey.Inspections++;
                    var newWorryLevel = GetNewWorryLevel(monkey, worryLevel, false) % worryFactor;                    
                    var toMonkey = newWorryLevel % monkey.Test == 0 ? monkeys.First(m => m.Id == monkey.IfTestTrue) : monkeys.First(m => m.Id == monkey.IfTestFalse);
                    monkey.Items.Remove(worryLevel);
                    toMonkey.Items.Add(newWorryLevel);
                }
            }
            round++;
        }
        var mostActive = monkeys.OrderByDescending(o => o.Inspections).Take(2).Select(s => s.Inspections).ToList();
        var result = mostActive[0] * mostActive[1];

        return result;
    }


    public static List<Monkey> BuildMonkeys(string[] input)
    {
        var monkeys = new List<Monkey>();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].StartsWith("Monkey"))
            {
                var monkeyNumber = int.Parse(input[i].Replace(":", "").Split(" ")[1]);
                var monkey = new Monkey{ Id = monkeyNumber };
                monkey.Items = input[i + 1].Split(":")[1].Split(",").Select(x => long.Parse(x)).ToList();
                monkey.Operation = input[i + 2].Split("=")[1].Trim();
                monkey.Test = int.Parse(input[i + 3].Split(" ").Last());
                monkey.IfTestTrue = int.Parse(input[i + 4].Split(" ").Last());
                monkey.IfTestFalse = int.Parse(input[i + 5].Split(" ").Last());
                monkeys.Add(monkey);
                i += 5;
            }
        }
        return monkeys;
    }

    public static long GetNewWorryLevel(Monkey monkey, long worryLevel, bool divide)
    {
        var op = monkey.Operation.Split(" ")[1];
        var opVal = monkey.Operation.Split(" ")[2] == "old" ? worryLevel : long.Parse(monkey.Operation.Split(" ")[2]);
        if (op == "+") worryLevel += opVal;
        if (op == "*") worryLevel *= opVal;
        if (divide) worryLevel /= 3;
        return worryLevel;
    }

    public class Monkey
    {
        public int Id { get; set; }
        public int Test { get; set; }
        public int IfTestTrue { get; set; }
        public int IfTestFalse { get; set; }
        public List<long> Items { get; set; }
        public string Operation { get; set; }
        public long Inspections { get; set; }        
    }
}
