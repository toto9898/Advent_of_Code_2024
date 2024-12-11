class Program
{
    const long BLINKS = 75;

    public static (long, long) Mutate(long value)
    {
        long stone1 = value;
        long stone2 = -1;
        long  numDigits = (long)Math.Floor(Math.Log10(stone1)) + 1;

        if (stone1 == 0)
        {
            stone1 = 1;
        }
        else if (numDigits % 2 == 0 && numDigits >= 2)
        {
            string valueStr = stone1.ToString();
            stone1 = long.Parse(valueStr[..(valueStr.Length / 2)]);
            stone2 = long.Parse(valueStr[(valueStr.Length / 2)..]);
        }
        else
        {
            stone1 *= 2024;
        }

        return (stone1, stone2);
    }

    static List<long> ReadInput(string fileName)
    {
        string  projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string  inputFilePath   = Path.Combine(projectPath, fileName);
        string  stonesStr       = File.ReadAllLines(inputFilePath).First();
    
        return  stonesStr.Split(' ').Select(long.Parse).ToList();
    }

    static List<(long, (long, long))> CreateGraph(List<long> stonesList)
    {

        List<(long, (long, long))> graph = [];

        foreach (var stone in stonesList)
        {
            graph.Add((stone, (-1, -1)));
        }
        
        for (int i = 0; i < BLINKS; i++)
        {
            int graphSize = graph.Count;
            for (int k = 0; k < graphSize; k++)
            {
                var adjList = graph[k].Item2;
                var node = graph[k].Item1;

                (long stone1, long stone2) = Mutate(node);
                
                graph[k] = (node, (stone1, stone2));

                if (!graph.Any(x => x.Item1 == stone1))
                    graph.Add((stone1, (-1, -1)));
                if (stone2 != -1 && !graph.Any(x => x.Item1 == stone2))
                    graph.Add((stone2, (-1, -1)));
            }
        }

        return graph;
    }

    static List<long[]> CreateStonesAfterBlinkMap(List<(long, (long, long))> graph)
    {
        List<long[]> stonesAfterBlinks = new(graph.Count);
        for (int i = 0; i < graph.Count; i++)
        {
            stonesAfterBlinks.Add(new long[BLINKS + 1]);
            stonesAfterBlinks[i][0] = 1;
        }

        return stonesAfterBlinks;
    }

    static long CountStones(long root, List<(long, (long, long))> graph, List<long[]> stonesAfterBlink, ref long result, long blinks = BLINKS)
    {
        var index = graph.FindIndex(x => x.Item1 == root);        

        if (stonesAfterBlink[index][blinks] != 0)
            return stonesAfterBlink[index][blinks];

        var adjList = graph[index].Item2;
        if (adjList.Item1 == -1 && adjList.Item2 == -1)
            return 1;

        long currResult =   CountStones(adjList.Item1, graph, stonesAfterBlink, ref result, blinks - 1) + 
                            (adjList.Item2 == -1 ? 0 : CountStones(adjList.Item2, graph, stonesAfterBlink, ref result, blinks - 1));
        
        stonesAfterBlink[index][blinks] = currResult;
        return currResult;
    }

    static void Main()
    {
        var  stonesList = ReadInput("input.txt");
        var  graph      = CreateGraph(stonesList);
        long result     = 0;

        List<long[]> stonesAfterBlinks = CreateStonesAfterBlinkMap(graph);

        foreach (var stone in stonesList)
        {
            result += CountStones(stone, graph, stonesAfterBlinks, ref result);
        }
        
        Console.WriteLine(result);
    }
}
