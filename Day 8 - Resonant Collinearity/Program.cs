using System.Drawing;

class Program
{
    static bool InBounds(Point point, Size mapSize) => point.X >= 0 && point.X < mapSize.Width && point.Y >= 0 && point.Y < mapSize.Height;

    static void PopulateAntinodesForPair(Point[] points, ref HashSet<Point> antinodes, Size mapSize, bool PART2)
    {
        Point direction = points[1] - ((Size)points[0]);
        Point antidote1 = points[1] + ((Size)direction);
        Point antidote2 = points[0] - ((Size)direction);
        
        if (InBounds(antidote1, mapSize))
            antinodes.Add(antidote1);
        if (InBounds(antidote2, mapSize))
            antinodes.Add(antidote2);

        if (PART2)
        {
            antinodes.Add(points[0]);
            antinodes.Add(points[1]);

            while (InBounds(antidote1, mapSize))
            {
                antinodes.Add(antidote1);
                antidote1 += (Size)direction;
            }
            while (InBounds(antidote2, mapSize))
            {
                antinodes.Add(antidote2);
                antidote2 -= (Size)direction;
            }
        }
    }

    static void PopulateAntinodesForAntennaType(char antenna, string[] map, ref HashSet<Point> antinodes, bool PART2)
    {
        Point[] antennas  = map.SelectMany((row, i) => row
                                    .Select((cell, j) => new Point(j, i))
                                    .Where(cell => map[cell.Y][cell.X] == antenna))
                                .ToArray();
        
        Size mapSize = new(map[0].Length, map.Length);

        for (int i = 0; i < antennas.Length; i++)
            for (int j = i + 1; j < antennas.Length; j++)
            {
                Point[]   points  = [antennas[i], antennas[j]];
                PopulateAntinodesForPair(points, ref antinodes, mapSize, PART2);
            }
    }
    
    static void PopulateAntinodes(string[] map, ref HashSet<Point> antinodes, bool PART2 = false)
    {
        for (char antenna = '0'; antenna <= '9'; antenna++)
            PopulateAntinodesForAntennaType(antenna, map, ref antinodes, PART2);
        for (char antenna = 'A'; antenna <= 'Z'; antenna++)
            PopulateAntinodesForAntennaType(antenna, map, ref antinodes, PART2);
        for (char antenna = 'a'; antenna <= 'z'; antenna++)
            PopulateAntinodesForAntennaType(antenna, map, ref antinodes, PART2);
    }
 
    static void Main()
    {
        string          projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string          inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]        map             = File.ReadAllLines(inputFilePath);

        HashSet<Point> antinodes = [];
        PopulateAntinodes(map, ref antinodes);
        Console.WriteLine(antinodes.Count);

        antinodes.Clear();
        PopulateAntinodes(map, ref antinodes, true);
        Console.WriteLine(antinodes.Count);
    }
}
