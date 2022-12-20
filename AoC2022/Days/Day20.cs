using System.Collections.Generic;

namespace AoC2022.Days;

public static class Day20
{

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsLong(20);

        var part1 = await Run(input, 1, 1);
        Console.WriteLine($"Part One - {part1}");

        var part2 = await Run(input, 10, 811589153);
        Console.WriteLine($"Part Two - {part2}");
    }

    private static async Task<long> Run(long[] input, int mixCount, int decryptionKey)
    {        
        var originalNumbers = input.Select((i, idx) => (idx, i * decryptionKey)).ToList(); 
        var numbers = new List<(int index, long value)>(originalNumbers);
        
        for (var i = 0; i < mixCount; i++)
        {
            foreach (var (index, value) in originalNumbers)
            {
                var pos = numbers.IndexOf((index, value));
                var newPos = (int)((pos + value) % (numbers.Count - 1));
                if (newPos < 0)
                {
                    newPos += numbers.Count - 1;
                }

                numbers.RemoveAt(pos);
                numbers.Insert(newPos, (index, value));
            }
        }

        var zeroIdx = numbers.FindIndex(i => i.value == 0);
        var result = 
            numbers[(1000 + zeroIdx) % numbers.Count].value +
            numbers[(2000 + zeroIdx) % numbers.Count].value +
            numbers[(3000 + zeroIdx) % numbers.Count].value;


        return result;

    }  

 }
