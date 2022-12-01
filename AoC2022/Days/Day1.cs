namespace AoC2022.Days;

public static class Day1
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(1);
        await Run(input);
    }

    private static async Task Run(string[] input)
    {
        var elfCalories = new List<int>();
        var calorieCount = 0;

        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i];

            if (string.IsNullOrEmpty(line)) //New elf
            {
                elfCalories.Add(calorieCount);
                calorieCount = 0;
            }
            
            if (!string.IsNullOrEmpty(line)) //Add calories
            {
                var calories = int.Parse(line);
                calorieCount += calories;

                if (i == input.Length - 1) //Last one is not a blank line, need to add it to the list.
                {
                    elfCalories.Add(calorieCount);
                }
            }
        }

        var highestCalorieCount = elfCalories.Max();
        Console.WriteLine($"Part 1 - {highestCalorieCount}");

        var highestThree = elfCalories
            .OrderByDescending(x => x)
            .Take(3)
            .Sum();

        Console.WriteLine($"Part 2 - {highestThree}");
    }
}
