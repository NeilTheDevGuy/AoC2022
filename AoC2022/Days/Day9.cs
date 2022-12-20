using static AoC2022.Days.Day9;

namespace AoC2022.Days;

public static class Day9
{
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(9);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        int headX = 0, headY = 0, tailX = 0, tailY = 0;
        var tailVisits = new HashSet<(int, int)>();
        tailVisits.Add((0, 0));

        foreach (var line in input)
        {
            var dir = line.Split(" ")[0];
            var steps = int.Parse(line.Split(" ")[1]);
            for (int i = 0; i < steps; i++)
            {
                if (dir == "U") headY--;
                if (dir == "D") headY++;
                if (dir == "L") headX--;
                if (dir == "R") headX++;
                if (ShouldMove(headX, headY, tailX, tailY))
                {
                    if (headY == tailY) //Only need to move horizontally
                    {
                        tailX = headX > tailX ? tailX + 1 : tailX - 1;
                    }
                    else if (headX == tailX) //Only need to move vertically
                    {
                        tailY = headY > tailY ? tailY + 1 : tailY - 1;
                    }
                    //Otherwise need to move diaganally
                    else
                    {
                        if (tailX > headX) tailX--;
                        if (headX > tailX) tailX++;
                        if (tailY > headY) tailY--;
                        if (headY > tailY) tailY++;
                    }
                    tailVisits.Add((tailX, tailY));
                }
            }
        }
        Console.WriteLine($"Part One - {tailVisits.Count}, headX - {headX}, tailX - {tailX}, headY - {headY}, tailY - {tailY}");
    }

    private static async Task PartTwo(string[] input)
    {
        var tailVisits = new HashSet<(int, int)>();
        tailVisits.Add((0, 0));

        var knots = new List<Knot>();
        for (int i = 0; i < 10; i++)
        {
            knots.Add(new Knot { X = 0, Y = 0 });
        }

        foreach (var line in input)
        {
            var dir = line.Split(" ")[0];
            var steps = int.Parse(line.Split(" ")[1]);
            
            for (int i = 0; i < steps; i++)
            {
                if (dir == "U") knots[0].Y--;
                if (dir == "D") knots[0].Y++;
                if (dir == "L") knots[0].X--;
                if (dir == "R") knots[0].X++;

                for (int k = 1; k <= 9; k++)
                {
                    var knot = knots[k];
                    var knotAhead = knots[k - 1];

                    if (ShouldMove(knotAhead.X, knotAhead.Y, knots[k].X, knots[k].Y))
                    {
                        if (knot.Y == knotAhead.Y) //Only need to move horizontally
                        {
                            knot.X = knotAhead.X > knot.X ? knot.X + 1 : knot.X - 1;
                        }
                        else if (knot.X == knotAhead.X) //Only need to move vertically
                        {
                        knot.Y = knotAhead.Y > knot.Y ? knot.Y + 1 : knot.Y - 1;
                        }
                        //Otherwise need to move diaganally
                        else
                        {
                            if (knot.X > knotAhead.X) knot.X--;
                            if (knotAhead.X > knot.X) knot.X++;
                            if (knot.Y > knotAhead.Y) knot.Y--;
                            if (knotAhead.Y > knot.Y) knot.Y++;
                        }
                        if (k == 9) tailVisits.Add((knot.X, knot.Y));
                    }                   
                }
            }
        }
        Console.WriteLine($"Part Two - {tailVisits.Count}");
    }

    private static bool ShouldMove(int headX, int headY, int tailX, int tailY)
    {
        return tailX - headX > 1 || headX - tailX > 1 || tailY - headY > 1 || headY - tailY > 1;
    }

    public class Knot 
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
