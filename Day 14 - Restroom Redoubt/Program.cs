struct Vector2Int(long x, long y)
{
    public override readonly bool Equals(object? obj)
    {
        if (obj is Vector2Int other)
        {
            return this == other;
        }
        return false;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
    public readonly long X => x;
    public readonly long Y => y;
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }
    public static Vector2Int operator *(Vector2Int a, long b)
    {
        return new Vector2Int(a.X * b, a.Y * b);
    }
    public static Vector2Int operator %(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int((a.X % b.X + b.X) % b.X, (a.Y % b.Y + b.Y) % b.Y);
    }
    public static readonly Vector2Int Zero = new(0, 0);
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.X == b.X && a.Y == b.Y;
    }
    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return a.X != b.X || a.Y != b.Y;
    }
}

class Robot(Vector2Int position, Vector2Int velocity)
{
    public Vector2Int Position { get; private set; } = position;
    public Vector2Int Velocity { get; private set; } = velocity;
    public static readonly Vector2Int RES = new(101, 103);
    public void Move(long times = 100)
    {
        Position = (Position + Velocity * times) % RES;
        if (Position.X < RES.X / 2 && Position.Y < RES.Y / 2)
        {
            if (!Q[0].ContainsKey(Position))
                Q[0][Position] = 0;
            Q[0][Position]++;
        }
        else if (Position.X > RES.X / 2 && Position.Y < RES.Y / 2)
        {
            if (!Q[1].ContainsKey(Position))
                Q[1][Position] = 0;
            Q[1][Position]++;
        }
        else if (Position.X < RES.X / 2 && Position.Y > RES.Y / 2 )
        {
            if (!Q[2].ContainsKey(Position))
                Q[2][Position] = 0;
            Q[2][Position]++;
        }
        else if (Position.X > RES.X / 2  && Position.Y > RES.Y / 2 )
        {
            if (!Q[3].ContainsKey(Position))
                Q[3][Position] = 0;
            Q[3][Position]++;
        }
    }

    public static Dictionary<Vector2Int, long>[] Q = [[], [], [], []];
}

class Program
{
    static List<Robot> ReadInput()
    {
        string                      projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string                      inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]                    input           = File.ReadAllLines(inputFilePath);
        List<Robot>                 result          = [];

        foreach (string line in input)
        {
            // parse line like this: p=0,4 v=3,-3
            string[]    parts       = line.Split(' ');
            string[]    p           = parts[0].Split(',');
            string[]    v           = parts[1].Split(',');
            long         x           = long.Parse(p[0].Substring(2));
            long         y           = long.Parse(p[1]);
            var         position    = new Vector2Int(x, y);
            x                       = long.Parse(v[0].Substring(2));
            y                       = long.Parse(v[1]);
            var         velocity    = new Vector2Int(x, y);
            result.Add(new Robot(position, velocity));
        }

        return result;
    }

    static char[,] CreateAsciiArtFromInputAndRobots(List<Robot> robots)
    {
        char[,] asciiArt = new char[Robot.RES.X, Robot.RES.Y];
        for (int i = 0; i < Robot.RES.X; i++)
            for (int j = 0; j < Robot.RES.Y; j++)
                asciiArt[i, j] = '.';

        foreach (Robot robot in robots)
            asciiArt[robot.Position.X, robot.Position.Y] = '#';

        return asciiArt;
    }

    static string GetAsciiFilePath(string fileName)
    {
        string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string asciiPath = Path.Combine(projectPath, "ASCII");
        if (!Directory.Exists(asciiPath))
            Directory.CreateDirectory(asciiPath);
        string asciiFilePath = Path.Combine(asciiPath, fileName);
        return asciiFilePath;
    }

    static void WriteSpaceToAsciiPages(List<Robot> robots, int pagesCount)
    {
        using StreamWriter sw = File.AppendText(GetAsciiFilePath("asciiArt.txt"));
        
        for (int i = 0; i < pagesCount; i++)
        {
            foreach (Robot robot in robots)
                robot.Move(1);
            
            sw.WriteLine(i + 1);

            var asciiArt = CreateAsciiArtFromInputAndRobots(robots);
            for (int ii = 0; ii < Robot.RES.X; ii++)
            {
                for (int j = 0; j < Robot.RES.Y; j++)
                {
                    sw.Write(asciiArt[ii, j]);
                }
                sw.WriteLine();
            }
            sw.WriteLine();
            sw.WriteLine();
        }

        sw.Close();
    }

    public static void Main()
    {
        List<Robot> robots1  = ReadInput();
        List<Robot> robots2 = [.. robots1];

        // Part 1
        foreach (Robot robot in robots1)
          robot.Move(100);

        long res = 1;
        foreach (var q in Robot.Q)
           res *= q.Sum(x => x.Value);

        Console.WriteLine(res);

        // Part 2
        WriteSpaceToAsciiPages(robots2, 10000);
        // Then search manually for the page with the Christmas tree
        // It was on page 7603
        // The game is the game :D
    }
}
