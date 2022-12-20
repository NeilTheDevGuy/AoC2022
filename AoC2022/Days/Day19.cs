namespace AoC2022.Days;

public static class Day19
{   
    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(19);
        await Run(input);
    }

    private static async Task Run(string[] input)
    {
        var blueprints = new List<Blueprint>();
        foreach (var line in input)
        {
            var splitStr = line.Split(" ");
            blueprints.Add(new Blueprint
            {
                Id = int.Parse(splitStr[1].Replace(":", "")),
                OreRobot = int.Parse(splitStr[6]),
                ClayRobot = int.Parse(splitStr[12]),
                ObsidianRobotOre = int.Parse(splitStr[18]),
                ObsidianRobotClay = int.Parse(splitStr[21]),
                GeodeRobotOre = int.Parse(splitStr[27]),
                GeodeRobotObsidian = int.Parse(splitStr[30])
            });
        }

        PartOne(blueprints, 24);
        PartTwo(blueprints, 32);
    }

    private static void PartOne(List<Blueprint> blueprints, int minutes)
    {
        var quality = 0;

        foreach (var blueprint in blueprints)
        {
            var mostGeodes = GetMostGeodes(minutes, blueprint);
            quality += (blueprint.Id * mostGeodes);
        }

        Console.WriteLine($"Part One - {quality}");
    }

    private static void PartTwo(List<Blueprint> blueprints, int minutes)
    {
        var quality = 1;

        foreach (var blueprint in blueprints.Take(3))
        {
            var mostGeodes = GetMostGeodes(minutes, blueprint);
            quality *= mostGeodes;
        }

        Console.WriteLine($"Part Two - {quality}");
    }


    private static int GetMostGeodes(int remainingTime, Blueprint blueprint)
    {
        int highest = 0;
                var queue = new Queue<State>();
        var attempts = new HashSet<State>();

        var state = new State { Minute = remainingTime, Ore = 0, Clay = 0, Obsidian = 0, Geodes = 0, OreRobots = 1, ClayRobots = 0, ObsidianRobots = 0 };
        queue.Enqueue(state);

        while (queue.TryDequeue(out state))
        {
            highest =state.Geodes > highest ? state.Geodes : highest;

            if (state.Minute > 0)
            {
                state.OreRobots = state.OreRobots < blueprint.MaxOre() ? state.OreRobots : blueprint.MaxOre();
                state.ClayRobots = state.ClayRobots < blueprint.ObsidianRobotClay ? state.ClayRobots : blueprint.ObsidianRobotClay;
                state.ObsidianRobots = state.ObsidianRobots < blueprint.GeodeRobotObsidian ? state.ObsidianRobots : blueprint.GeodeRobotObsidian;

                state.Ore = Math.Min(state.Ore, (state.Minute * blueprint.MaxOre()) - (state.OreRobots * (state.Minute - 1)));
                state.Clay = Math.Min(state.Clay, (state.Minute * blueprint.ObsidianRobotClay) - (state.ClayRobots * (state.Minute - 1)));
                state.Obsidian = Math.Min(state.Obsidian, (state.Minute * blueprint.GeodeRobotObsidian) - (state.ObsidianRobots * (state.Minute - 1)));

                if (attempts.Contains(state)) continue;
                attempts.Add(state);

                //Queue a "don't buy, just mine" option
                var newState = GetState(state, blueprint);
                queue.Enqueue(newState);

                //Check if we can buy more robots, and queue the possibilities.
                if (state.Ore >= blueprint.OreRobot)
                {
                    var newOreState = GetState(state, blueprint);
                    newOreState.OreRobots++;
                    newOreState.Ore -= blueprint.OreRobot;
                    if (!attempts.Contains(newOreState)) queue.Enqueue(newOreState);
                }
                if (state.Ore >= blueprint.ClayRobot)
                {
                    var newClayState = GetState(state, blueprint);
                    newClayState.ClayRobots++;
                    newClayState.Ore -= blueprint.ClayRobot;
                    if (!attempts.Contains(newClayState)) queue.Enqueue(newClayState);
                }
                if (state.Ore >= blueprint.ObsidianRobotOre && state.Clay >= blueprint.ObsidianRobotClay)
                {
                    var newObsidianState = GetState(state, blueprint);
                    newObsidianState.ObsidianRobots++;
                    newObsidianState.Ore -= blueprint.ObsidianRobotOre;
                    newObsidianState.Clay -= blueprint.ObsidianRobotClay;
                    if (!attempts.Contains(newObsidianState)) queue.Enqueue(newObsidianState);
                }
                if (state.Ore >= blueprint.GeodeRobotOre && state.Obsidian >= blueprint.GeodeRobotObsidian) 
                {
                    var newGeodeState = GetState(state, blueprint);
                    newGeodeState.GeodeRobots++;
                    newGeodeState.Ore -= blueprint.GeodeRobotOre;
                    newGeodeState.Obsidian -= blueprint.GeodeRobotObsidian;
                    if (!attempts.Contains(newGeodeState)) queue.Enqueue(newGeodeState);
                }
            }
        }
        return highest;
    }

    public static State GetState(State state, Blueprint blueprint)
    {
        return new State {
            Minute = state.Minute - 1,
            Ore = state.Ore + state.OreRobots,
            Clay = state.Clay + state.ClayRobots,
            Obsidian = state.Obsidian + state.ObsidianRobots,            
            Geodes = state.Geodes + state.GeodeRobots,
            OreRobots = state.OreRobots,
            ClayRobots = state.ClayRobots,
            ObsidianRobots = state.ObsidianRobots,
            GeodeRobots = state.GeodeRobots
        };
    }

    public class Blueprint
    {
        public int Id { get; set; }
        public int OreRobot { get; set; }
        public int ClayRobot { get; set; }        
        public int ObsidianRobotOre { get; set; }
        public int ObsidianRobotClay { get; set; }
        public int GeodeRobotOre { get; set; }
        public int GeodeRobotObsidian { get; set; }

        public int MaxOre()
        {
            var highest = OreRobot;
            if (ClayRobot > highest) highest = ClayRobot;
            if (ObsidianRobotOre > highest) highest = ObsidianRobotOre;
            if (GeodeRobotOre > highest) highest = GeodeRobotOre;
            return highest;
        }
    }

    public class State
    {        
        public int Minute { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geodes { get; set; }
        public int OreRobots { get; set; }
        public int ClayRobots { get; set; }
        public int ObsidianRobots { get; set; }
        public int GeodeRobots { get; set; }

        public override int GetHashCode()
        {
            var hc = HashCode.Combine(Ore, Clay, Obsidian, Geodes, OreRobots, ClayRobots, ObsidianRobots, GeodeRobots);
            return HashCode.Combine(hc, Minute);
        }

        public override bool Equals(object? obj)
        {
            return obj is State state &&
                state.Minute == Minute &&
                state.Ore == Ore &&
                state.Clay == Clay &&
                state.Obsidian == Obsidian &&
                state.Geodes == Geodes &&
                state.OreRobots == OreRobots &&
                state.ClayRobots == ClayRobots &&
                state.ObsidianRobots == ObsidianRobots &&
                state.GeodeRobots == GeodeRobots;
        }
    }
}


