List<int> list1 = [];
List<int> list2 = [];
Dictionary<int, int> similarities = [];

string projectPath  = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
string inputPath    = Path.Combine(projectPath, "input.txt");

string[] lines = File.ReadAllLines(Path.Combine(inputPath, "input.txt"));

foreach (string line in lines)
{
    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    int location1 = int.Parse(parts[0].Trim());
    int location2 = int.Parse(parts[1].Trim());
    list1.Add(location1);
    list2.Add(location2);
    
    if (similarities.ContainsKey(location2))
    {
        similarities[location2]++;
    }
    else
    {
        similarities.Add(location2, 1);
    }
}

list1.Sort();
list2.Sort();

Int64 totalDistance = 0;
Int64 similarityScore = 0;
for (int i = 0; i < list1.Count; i++)
{
    totalDistance += Math.Abs(list1[i] - list2[i]);
    if ( similarities.TryGetValue(list1[i], out int value))
    {
        similarityScore += list1[i] * value;
    }
}

Console.WriteLine(totalDistance); // Part 1 answer
Console.WriteLine(similarityScore); // Part 2 answer
