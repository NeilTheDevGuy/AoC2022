using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AoC2022.Days;

public static class Day12 //UNFINISHED
{
    private static int _steps = int.MaxValue;
    private static int _startX = 0;
    private static int _startY = 0;
    private static int _endX = 0;
    private static int _endY = 0;

    private static int _followCount = 0;

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(12);
        await PartOne(input);
    }

    private static async Task PartOne(string[] input)
    {
        var grid = input.Select(line => line.ToCharArray()).ToArray();
        var visited = new int[grid.Length, grid[0].Length];

        int startX = 0;
        int startY = 0;
        int steps = 1;

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == 'E')
                {
                    startX = x;
                    startY = y;
                    grid[y][x] = 'z';
                }
            }
        }

        visited[startY, startX] = 1;

        while (CanFollow(visited, grid, steps))
        {
            steps++;
        }

        Console.WriteLine($"Part One -  {steps}");
    }

    private static bool CanFollow(int[,] visited, char[][] grid, int steps)
    {
        var newVisited = (int[,])visited.Clone();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                var currentHeight = grid[y][x];

                if (visited[y, x] != 0)
                {
                    if (grid[y][x] == 'a')
                    {
                        return true;
                    }

                    if (y > 0 && visited[y - 1, x] == 0 && currentHeight - grid[y - 1][x] <= 1)
                    {
                        newVisited[y - 1, x] = steps;
                    }

                    if (y < grid.Length - 1 && visited[y + 1, x] == 0 && currentHeight - grid[y + 1][x] <= 1)
                    {
                        newVisited[y + 1, x] = steps;
                    }

                    if (x > 0 && visited[y, x - 1] == 0 && currentHeight - grid[y][x - 1] <= 1)
                    {
                        newVisited[y, x - 1] = steps;
                    }

                    if (x < grid[0].Length - 1 && visited[y, x + 1] == 0 && currentHeight - grid[y][x + 1] <= 1)
                    {
                        newVisited[y, x + 1] = steps;
                    }
                }
            }
        }

        visited = newVisited;

        return false;
    }

    private static async Task Run(string[] input)
    {
        var grid = new string[input[0].Length, input.Length];
      
                
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == 'S')
                {
                    _startX = x;
                    _startY = y;
                }
                if (input[y][x] == 'E')
                { 
                    _endX = x;
                    _endY = y;
                }
            }
        }

        Follow(_startX, _startY, 0, input);

        Console.WriteLine($"Part One - {_steps}");
     }





    private static void Follow(int x, int y, int steps, string[] input)
    {
        var possibleRoutes = new Queue<Route>();
   
        var currentElevation = input[y][x];
        if (currentElevation == 'S') currentElevation = 'a';
        
        if (x < input[y].Length - 1)
        {
            //Check elevations
            if (input[y][x + 1] - currentElevation <= 1 && input[y][x + 1] != x )
                possibleRoutes.Enqueue(new Route { X = x + 1, Y = y }); //Right
        }
        if (x > 0)
        {
            if (input[y][x - 1] - currentElevation <= 1 && input[y][x - 1] != x)
                possibleRoutes.Enqueue(new Route { X = x - 1, Y = y }); //Left
        }
        if (y < input.Length - 1)
        {
            if (input[y + 1][x] - currentElevation <= 1 && input[y + 1][x] != y)
                possibleRoutes.Enqueue(new Route { X = x, Y = y + 1 }); //Down
        }
        if (y > 0)
        {
            if (input[y - 1][x] - currentElevation <= 1 && input[y - 1][x] != y)
                possibleRoutes.Enqueue(new Route { X = x, Y = y - 1}); //Up
        }

        var visited = new int[input[0].Length, input.Length];
        visited[_startX, _startY] = 1;
        while (possibleRoutes.Any())
        {

            var route = possibleRoutes.Dequeue();            
            if (visited[route.X, route.Y] != 1)
            {

                visited[route.X, route.Y] = 1;

                if (route.X == _startX && route.Y == _startY && steps > 0) //we have reached the start again without reaching the end.
                {
                    Console.WriteLine("Reached START");
                }

                if (route.X == _endX && route.Y == _endY)
                {
                    Console.WriteLine("Reached END");
                    _steps = steps < _steps ? steps : _steps;
                    break;
                }
                steps++;
                var routesAdded = 0;

                currentElevation = input[route.Y][route.X];
                if (currentElevation == 'S') currentElevation = 'a';

                if (route.X < input[route.Y].Length - 1)
                {
                    if (input[route.Y][route.X + 1] - currentElevation <= 1)
                    {
                        possibleRoutes.Enqueue(new Route { X = route.X + 1, Y = route.Y }); //Right
                        routesAdded++;
                    }
                }
                if (route.X > 0)
                {
                    if (input[route.Y][route.X - 1] - currentElevation <= 1)
                    {
                        possibleRoutes.Enqueue(new Route { X = route.X - 1, Y = route.Y }); //Left
                        routesAdded++;
                    }
                }
                if (route.Y < input.Length - 1)
                {
                    if (input[route.Y + 1][route.X] - currentElevation <= 1)
                    {
                        possibleRoutes.Enqueue(new Route { X = route.X, Y = route.Y + 1 }); //Down
                        routesAdded++;
                    }
                }
                if (route.Y > 0)
                {
                    if (input[route.Y - 1][route.X] - currentElevation <= 1)
                    {
                        possibleRoutes.Enqueue(new Route { X = route.X, Y = route.Y - 1 }); //Up
                        routesAdded++;
                    }
                }
                
            }
        }
    }

    public class Route
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
