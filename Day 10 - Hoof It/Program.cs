using System.Security;

class Program
{
    static int[,] ReadInput()
    {
        string              projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string              inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]            mapStr          = File.ReadAllLines(inputFilePath);
        
        int[,]              map             = new int[mapStr.Length, mapStr[0].Length];

        for (int i = 0; i < mapStr.Length; i++)
            for (int j = 0; j < mapStr[i].Length; j++)
                map[i, j] = mapStr[i][j] - '0';
        return map;
    }
    
    static int GetTrailheadScore(int[,] map, int x, int y, bool part2)
    {
        if (map[x, y] != 0)
            return 0;

        Queue<(int, int)> trail = new();
        trail.Enqueue((x, y));

        int          score      = 0;
        (int, int)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        HashSet<(int, int)> visitedNines = [];

        while (trail.Count > 0)
        {
            (int, int) current = trail.Dequeue();
            int i = current.Item1;
            int j = current.Item2;

            foreach ((int, int) direction in directions)
            {
                int newI = i + direction.Item1;
                int newJ = j + direction.Item2;

                if (newI < 0 || newI >= map.GetLength(1) || newJ < 0 || newJ >= map.GetLength(0))
                    continue;

                if (map[newI, newJ] != map[i, j] + 1) 
                    continue;

                if ( map[newI, newJ] == 9 &&
                    (part2 || !visitedNines.Contains((newI, newJ))) )
                {
                    visitedNines.Add((newI, newJ));
                    score++;
                    continue;
                }

                trail.Enqueue((newI, newJ));
            }
        }
        return score;
    }

    static void Main()
    {
        int[,]  map     = ReadInput();
        int     score1  = 0;
        int     score2  = 0;

        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                score1 += GetTrailheadScore(map, i, j, false);
                score2 += GetTrailheadScore(map, i, j, true);
            }
        }

        Console.WriteLine( score1 );
        Console.WriteLine( score2 );
    }
}
