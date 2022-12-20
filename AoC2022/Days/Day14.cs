using System.Diagnostics;
using System.Drawing;

namespace AoC2022.Days;

public static class Day14
{
    private static List<Line> _linePoints = new List<Line>();
    private const int _sandStartX = 500;
    private const int _sandStartY = 0;

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(14);
        await PartOne(input);
        await PartTwo(input);
    }

    private static async Task PartOne(string[] input)
    {
        var sw = new Stopwatch();
        sw.Start();

        var sandCount = 0;
        var done = false;

        (var grid, var minX, var minY, var maxX, var maxY) = BuildGrid(input);

        while (!done)
        {
            var sandX = _sandStartX;
            var sandY = _sandStartY;

            while (true)
            {                
                if ((sandX == minX || sandX == maxX ) && (sandY == maxY -1))
                {
                    done = true;
                    break;
                }

                var intersects = _linePoints.Intersects(sandX, sandY);

                if (!intersects && (!_linePoints.Intersects(sandX, sandY + 1) && string.IsNullOrEmpty(grid[sandX, sandY + 1]))) //Go straight down
                {
                    sandY++;
                }
                else if (!intersects && (!_linePoints.Intersects(sandX - 1, sandY + 1) && string.IsNullOrEmpty(grid[sandX - 1, sandY + 1]))) //Diagonal Left
                {
                    sandY++;
                    sandX--;
                }
                else if (!intersects && (!_linePoints.Intersects(sandX + 1, sandY + 1) && (string.IsNullOrEmpty(grid[sandX + 1, sandY + 1])))) //Diaganal Right
                {
                    sandY++;
                    sandX++;
                }
                else //Can't go any further - set the sand here.
                {
                    grid[sandX, sandY] = "S";
                    break;
                }               
            }
            sandCount++;
        }

        sw.Stop();
        Console.WriteLine($"Part One - {sandCount -1} in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static async Task PartTwo(string[] input)
    {
        var sw = new Stopwatch();
        sw.Start();

        var sandCount = 0;
        var done = false;

        (var grid, _, _, _, var maxY) = BuildGrid(input);

        while (!done)
        {
            var sandX = _sandStartX;
            var sandY = _sandStartY;

            while (true)
            {

                var intersects = _linePoints.Intersects(sandX, sandY);

                if (!intersects && (!_linePoints.Intersects(sandX, sandY + 1) && string.IsNullOrEmpty(grid[sandX, sandY + 1]) && sandY <= maxY)) //Go straight down
                {
                    sandY++;
                }
                else if (!intersects && (!_linePoints.Intersects(sandX - 1, sandY + 1) && string.IsNullOrEmpty(grid[sandX - 1, sandY + 1])) && sandY <= maxY) //Diagonal Left
                {
                    sandY++;
                    sandX--;
                }
                else if (!intersects && (!_linePoints.Intersects(sandX + 1, sandY + 1) && (string.IsNullOrEmpty(grid[sandX + 1, sandY + 1])) && sandY <= maxY)) //Diaganal Right
                {
                    sandY++;
                    sandX++;
                }
                else //Can't go any further - set the sand here.
                {
                    if (sandX == _sandStartX && sandY == _sandStartY) //If it's reached the start point then we are done.
                    {
                        done = true;
                        break;
                    }

                    grid[sandX, sandY] = "S";
                    break;
                }
            }
            sandCount++;
        }

        sw.Stop();
        Console.WriteLine($"Part Two - {sandCount} in {sw.Elapsed.TotalMinutes} minutes");
    }

    private static bool Intersects(this List<Line> points, int x, int y)
    {
        foreach (var line in _linePoints)
        {
            if ((((x >= line.Start.X && x <= line.End.X) || (x <= line.Start.X && x >= line.End.X)) && y == line.Start.Y)
                ||
               (((y >= line.Start.Y && y <=line.End.Y) || (y <= line.Start.Y && y >= line.End.Y)) && x == line.Start.X))
                return true;
        }
        return false;
    }

    private static (string[,], int, int, int, int) BuildGrid(string[] input)
    {
        var grid = new string[10000, 10000];
        var minX = 1000;
        var minY = 1000;
        var maxX = 1;
        var maxY = 1;

        foreach (var line in input)
        {
            var coords = line.Split("->");
            for (int i = 0; i < coords.Length; i++)
            {
                var startX = int.Parse(coords[i].Split(",")[0].Trim());
                var startY = int.Parse(coords[i].Split(",")[1].Trim());

                if (i < coords.Length - 1)
                {
                    var endX = int.Parse(coords[i + 1].Split(",")[0].Trim());
                    var endY = int.Parse(coords[i + 1].Split(",")[1].Trim());
                    _linePoints.Add(new Line { Start = new Point { X = startX, Y = startY }, End = new Point { X = endX, Y = endY } });
                }

                if (startX > maxX) maxX = startX;
                if (startX < minX) minX = startX;
                if (startY > maxY) maxY = startY;
                if (startY < minY) minY = startY;
            }
        }

        return (grid, minX, minY, maxX, maxY);
    }

    public class Line
    {
        public Point Start { get; set; }
        public Point End { get; set; }
    }
}
