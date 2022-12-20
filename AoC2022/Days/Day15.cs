using System.ComponentModel.Design;
using System.Drawing;

namespace AoC2022.Days;

public static class Day15
{
    private static Dictionary<Point, int> _sensors = new Dictionary<Point, int>();
    private static HashSet<(int, int)> _beacons = new HashSet<(int, int)>();
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(15);
        BuildSensorsAndBeacons(input);
        await PartOne();
        await PartTwo();
    }

    private static async Task PartOne()
    {
        var covered = new HashSet<(int, int)>();
        
        var yRow = 10; //Example
        //var yRow = 2000000; //Real

        foreach (var sensor in _sensors)
        {
            for (int i = 1; i <= sensor.Value; i++)
            {
                var xDiff = sensor.Value - i;
                var yDiff = sensor.Value - xDiff;

                var x = sensor.Key.X + xDiff;
                var y = sensor.Key.Y + yDiff;
                if (sensor.Key.Y <= yRow && y >= yRow) covered.Add((x, yRow));

                x = sensor.Key.X + xDiff;
                y = sensor.Key.Y - yDiff;
                if (sensor.Key.Y >= yRow && y <= yRow) covered.Add((x, yRow));

                x = sensor.Key.X - xDiff;
                y = sensor.Key.Y + yDiff;
                if (sensor.Key.Y <= yRow && y >= yRow) covered.Add((x, yRow));

                x = sensor.Key.X - xDiff;
                y = sensor.Key.Y - yDiff;
                if (sensor.Key.Y >= yRow && y <= yRow) covered.Add((x, yRow));
            }

            foreach (var beacon in _beacons)
            {
                covered.Remove((beacon.Item1, beacon.Item2));
            }
        }

        Console.WriteLine($"Part One - {covered.Count}");
    }

    private static async Task PartTwo()
    {
        var min = 0;
        var max = 20; //Example
        //var max = 4000000; //Real

        //Needs to allow
        

        var grid = new int[max +1, max +1];
        for (int x = min; x <= max; x++)
        {
            for (int y = min; y <= max; y++) 
            {
                grid[x, y] = 1;
            }
        }

        for (int x = min; x <= max; x++)
        {
            for (int y = min; y <= max; y++)
            {
                foreach (var sensor in _sensors)
                {
                    if (sensor.Key.X + sensor.Value - x >= x|| sensor.Key.X - sensor.Value -x  <= x)
                        grid[x, y] = 0;

                    if (sensor.Key.Y + sensor.Value - y >= y || sensor.Key.Y - sensor.Value - y <= y)
                        grid[x, y] = 0;
                }
            }
        }

        //foreach (var sensor in _sensors)
        //{
        //    for (int i = 0; i <= sensor.Value; i++)
        //    {
        //        var xDiff = sensor.Value - i;
        //        var yDiff = sensor.Value - xDiff;

        //        var x = AdjustForRange(sensor.Key.X + xDiff, min, max);
        //        var y = AdjustForRange(sensor.Key.Y + yDiff, min, max);

        //            Console.WriteLine($"Removing {x},{y}");
        //            grid[x, y] = 0; 


        //        x = AdjustForRange(sensor.Key.X + xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y - yDiff, min, max);

        //            Console.WriteLine($"Removing {x},{y}");
        //            grid[x, y] = 0;


        //        x = AdjustForRange(sensor.Key.X - xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y + yDiff, min, max);

        //            Console.WriteLine($"Removing {x},{y}");
        //            grid[x, y] = 0;


        //        x = AdjustForRange(sensor.Key.X - xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y - yDiff, min, max);

        //            Console.WriteLine($"Removing {x},{y}");
        //            grid[x, y] = 0;
        //    }

        //    for (int i = 0; i <= sensor.Value; i++)
        //    {
        //        var yDiff = sensor.Value - i;
        //        var xDiff = sensor.Value - yDiff;

        //        var x = AdjustForRange(sensor.Key.X + xDiff, min, max);
        //        var y = AdjustForRange(sensor.Key.Y + yDiff, min, max);

        //        Console.WriteLine($"Removing {x},{y}");
        //        grid[x, y] = 0;


        //        x = AdjustForRange(sensor.Key.X + xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y - yDiff, min, max);

        //        Console.WriteLine($"Removing {x},{y}");
        //        grid[x, y] = 0;


        //        x = AdjustForRange(sensor.Key.X - xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y + yDiff, min, max);

        //        Console.WriteLine($"Removing {x},{y}");
        //        grid[x, y] = 0;


        //        x = AdjustForRange(sensor.Key.X - xDiff, min, max);
        //        y = AdjustForRange(sensor.Key.Y - yDiff, min, max);

        //        Console.WriteLine($"Removing {x},{y}");
        //        grid[x, y] = 0;
        //    }

        //}

        for (int x = min; x <= max; x++)
        {
            for (int y = min; y <= max; y++)
            {
                if (grid[x, y] == 1)
                {
                    var freq = (x * 4000000) + y;
                    Console.WriteLine($"Part Two - {freq} at coord {x},{y}");
                }
            }
        }
    }


    private static int GetManhattanDistance(int startX, int startY, int endX, int endY)
    {
        return (Math.Abs(startX - endX)) + (Math.Abs(startY -endY));
    }

    private static void BuildSensorsAndBeacons(string[] input)
    {
        foreach (var line in input)
        {
            var parseLine = line.Replace("Sensor at ", String.Empty).Replace(" closest beacon is at ", String.Empty);
            var splitLine = parseLine.Split(":");
            var sensorX = int.Parse(splitLine[0].Split(",")[0].Replace("x=", String.Empty));
            var sensorY = int.Parse(splitLine[0].Split(",")[1].Replace("y=", String.Empty));
            var beaconX = int.Parse(splitLine[1].Split(",")[0].Replace("x=", String.Empty));
            var beaconY = int.Parse(splitLine[1].Split(",")[1].Replace("y=", String.Empty));

            _sensors.Add(new Point { X = sensorX, Y = sensorY }, GetManhattanDistance(sensorX, sensorY, beaconX, beaconY));
            _beacons.Add((beaconX, beaconY));
        }
    }

    private static int AdjustForRange(int val, int min, int max)
    {
        val = val >= max ? max : val;
        val = val <= min ? min : val;

        return val;
    }
}
