List<Int32> locations1 = new List<Int32>();
List<Int32> locations2 = new List<Int32>();

string[] lines = File.ReadAllLines("e:/Programming/Advent_of_Code_2024/Day 1 - Historian Hysteria/input.txt");

foreach (string line in lines)
{

    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    locations1.Add(int.Parse(parts[0].Trim()));
    locations2.Add(int.Parse(parts[1].Trim()));
}

locations1.Sort();
locations2.Sort();

Int64 totalDistance = 0;
for (int i = 0; i < locations1.Count; i++)
{
    totalDistance += Math.Abs(locations1[i] - locations2[i]);
}

Console.WriteLine(totalDistance);
