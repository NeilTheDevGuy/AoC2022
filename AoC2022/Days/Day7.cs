namespace AoC2022.Days;

public static class Day7
{
    private static DirTree _dirs = new DirTree { SubDirs = new List<DirTree>() };
    private static List<DirTree> _dirsOverSize = new List<DirTree>();
    private static List<DirTree> _flattenedDirs = new List<DirTree>();   
    private static int _ptr = 0;

    public static async Task Run()
    {
        var input = await InputGetter.GetFromLinesAsString(7);
        await Run(input);
    }

    private static async Task Run(string[] input)
    {
        _dirs.Name = "/";
        ParseTree(input, _dirs);
        _dirs.SetTotalSizes(_dirs, 0);
        var answer = _dirsOverSize.Sum(s => s.TotalSize);
        Console.WriteLine($"Part One - {answer}");

        var unusedSpace = 70000000 - _dirs.TotalSize;
        var requiredSpace = 30000000 - unusedSpace;

        var closestMatch = _flattenedDirs.OrderBy(o => o.TotalSize).First(f => f.TotalSize > requiredSpace);
        Console.WriteLine($"Part Two - {closestMatch.TotalSize}");
    }

    private static void ParseTree(string[] input, DirTree tree)
    {

        while (_ptr < input.Length - 1)
        {
            var cmd = input[++_ptr];
            if (cmd.StartsWith("dir"))
            {
                tree.SubDirs.Add(new DirTree { Name = cmd.Split(" ")[1], Parent = tree });
            }

            if (long.TryParse(cmd.Split()[0], out var size))
            {
                tree.Files.Add(new File { Name = cmd.Split(" ")[1], Size = size});
            }

            if (cmd == ("$ cd /"))
            {
                tree = _dirs;
            }

            if (cmd == ("$ cd .."))
            {
                tree = tree.Parent;
            }

            if (cmd.StartsWith("$ cd ") && !cmd.EndsWith("/") && !cmd.EndsWith(".."))
            {
                var changeDir = cmd.Split(" ")[2];
                var subDir = tree.SubDirs.First(x => x.Name == changeDir);
                ParseTree(input, subDir);
            }
        }
    }

    public class DirTree
    {
        public List<DirTree> SubDirs { get; set; } = new List<DirTree>();
        public List<File> Files { get; set; } = new List<File>();
        public string Name { get; set; }
        public DirTree Parent { get; set; }
        public long TotalSize { get; set; }

        public long SetTotalSizes(DirTree dir, long totalSize)
        {            
            foreach (var subDir in dir.SubDirs)
            {                
                totalSize += SetTotalSizes(subDir, 0);
            }

            foreach (var file in dir.Files)
            {
                totalSize += file.Size;
            }

            dir.TotalSize = totalSize;
            if (totalSize <= 100000) _dirsOverSize.Add(dir);
            _flattenedDirs.Add(dir);
            return totalSize;
        }
    }

    public class File
    {
        public string Name { get; set; }
        public long Size { get; set; }
    }
}
