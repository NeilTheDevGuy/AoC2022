namespace AoC2022.Days;

public static class Day5
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(5);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var (idx,stacks) = await GetStacks(input);
        var instructions = input.ToList().GetRange(idx + 2, input.Length - (idx + 2));

        //Go through the instructions
        foreach (var instruction in instructions)
        {
            (var amountToMove, var moveFrom, var moveTo) = GetInstructions(instruction);

            for (int i = 1; i <= amountToMove; i++)
            {
                var crate = stacks[moveFrom].Pop();
                stacks[moveTo].Push(crate);
            }            
        }

        //Build the output        
        string output = stacks.Aggregate("", (s, stacks) => s + stacks.Value.Peek());
        Console.WriteLine($"Part One - {output}");
    }

    private static async Task PartTwo(string[] input)
    {
        var (idx, stacks) = await GetStacks(input);
        var instructions = input.ToList().GetRange(idx + 2, input.Length - (idx + 2));

        //Go through the instructions
        foreach (var instruction in instructions)
        {
            (var amountToMove, var moveFrom, var moveTo) = GetInstructions(instruction);

            var tempStack = new Stack<char>();
            for (int i = 1; i <= amountToMove; i++)
            {
                var crate = stacks[moveFrom].Pop();
                tempStack.Push(crate);                
            }

            while (tempStack.Count > 0)
            {
                var crate = tempStack.Pop();
                stacks[moveTo].Push(crate);
            }
        }

        //Build the output
        string output = stacks.Aggregate("", (s, stacks) => s + stacks.Value.Peek());
        Console.WriteLine($"Part Two - {output}");
    }

     private static (int, int, int) GetInstructions(string instruction)
    {
        var instructionSplit = instruction.Split(" ");
        var amountToMove = int.Parse(instructionSplit[1]);
        var moveFrom = int.Parse(instructionSplit[3]);
        var moveTo = int.Parse(instructionSplit[5]);
        return (amountToMove, moveFrom, moveTo);
    }

    private static async Task<(int, Dictionary<int, Stack<char>>)> GetStacks(string[] input)
    {
        var idx = 0;
        var stackNumbers = "";
        while (true)
        {
            if (input[idx].Contains("1"))
            {
                stackNumbers = input[idx];
                break;
            }
            idx++;
        }

        var stacksInput = input.ToList().GetRange(0, idx);
        var stacks = new Dictionary<int, Stack<char>>();

        //Get the index positions of the stack numbers
        var stackIndexLocations = new Dictionary<int, int>();
        for (int i = 0; i < stackNumbers.Length; i++)
        {
            if (int.TryParse(stackNumbers[i].ToString(), out int stackNum))
            {
                stackIndexLocations.Add(i, stackNum);
                stacks.Add(stackNum, new Stack<char>());
            }
        }

        //Parse the input to build up the initial Stacks state
        for (int i = stacksInput.Count - 1; i >= 0; i--)
        {
            var stacksIdx = 0;
            var bracketsCount = stacksInput[i].Count(s => s == '[');
            for (int x = 0; x <= bracketsCount - 1; x++)
            {
                var nextNumberIdx = stacksInput[i].IndexOf('[', stacksIdx) + 1;
                var stackLocation = stackIndexLocations[nextNumberIdx];
                var crate = stacksInput[i][nextNumberIdx];
                stacks[stackLocation].Push(crate);
                stacksIdx = nextNumberIdx;
            }
        }

        return (idx,stacks);
    }
}
