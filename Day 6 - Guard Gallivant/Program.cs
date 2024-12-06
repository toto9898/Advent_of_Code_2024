using System.Numerics;

class Guard(Vector2 position, char[][] map)
{
    // True if the guard is able to exit the map
    // False if the guard is stuck in a loop
    public bool Patrol()
    {
        while (Move())
        {
            if (visited.Contains((pos, direction)))
                return false;
            else
                visited.Add((pos, direction));
        }

        return true;
    }

    private bool Move()
    {
        while (!OutOfBounds() && !IsWall())
        {
            if (OutOfBounds())
                return false;
         
            _map[(int)pos.Y][(int)pos.X] = 'X';
            pos += direction;
        }

        if (OutOfBounds())
            return false;
        else
        {
            pos -= direction;
            direction = directions[++directionIndex % directions.Length];
        }

        return true;
    }

    private bool OutOfBounds()
    {
        return pos.X < 0 || pos.X >= _map[0].Length || pos.Y < 0 || pos.Y >= _map.Length;
    }
    
    private bool IsWall()
    {
        return _map[(int)pos.Y][(int)pos.X] == '#';
    }

    static readonly Vector2[] directions =
    [
        new Vector2(0, -1), // up
        new Vector2(1, 0),  // right
        new Vector2(0, 1),  // down
        new Vector2(-1, 0)  // left
    ];

    private Vector2 pos = position;
    private char[][] _map = map;
    private Vector2 direction = directions[0];
    private int directionIndex = 0;
    private HashSet<(Vector2, Vector2)> visited = [];
}


class Program
{
    static void Main()
    {
        string          projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string          inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]        mapStr          = File.ReadAllLines(inputFilePath);

        char[][]        map             = mapStr.Select(row => row.ToCharArray()).ToArray();
        
        var position    = map.Select((row, y) => (row, y)).First(rowY => rowY.row.Contains('^'));
        int x           = Array.IndexOf(position.row, '^');
        int y           = position.y;       
        map[y][x]       = 'X';
        Vector2 pos     = new(x, y);
        
        Guard guard = new(pos, map);
        guard.Patrol();
        int result1 = map.Sum(row => row.Count(c => c == 'X'));

        int result2 = 0;
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                char c = map[i][j];
                if (c == '.' || c == 'X')
                {
                    map[i][j] = '#';
                    Guard guard2 = new(pos, map);
                    if (!guard2.Patrol())
                        result2++;
                    map[i][j] = c;
                }

            }
        }

        Console.WriteLine(result1);
        Console.WriteLine(result2);
    }
}
