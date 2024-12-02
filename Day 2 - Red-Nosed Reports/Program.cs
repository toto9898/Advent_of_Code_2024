static bool SafeCheck(int currLevel, int nextLevel, bool decreasing)
{
    int diff = currLevel - nextLevel;
    bool monotonic = diff * (decreasing ? 1 : -1) > 0;
    return Math.Abs(diff) <= 3 && monotonic;
}

static int IsSafeReportPart1(string[] parts)
{

    bool decreasing = int.Parse(parts[0]) - int.Parse(parts[1]) > 0;
    int sign = decreasing ? 1 : -1;
    for (int i = 0; i < parts.Length - 1; i++)
    {
        int currLevel = int.Parse(parts[i]);
        int nextLevel = int.Parse(parts[i + 1]);
        if (!SafeCheck(currLevel, nextLevel, decreasing))
        {
            return i + 1;
        }
    }
    return 0;
}

static bool IsSafeReportPart2_BruteForce(string[] parts)
{
    for (int i = 0; i < parts.Length; i++)
    {
        List<string> sublist = [.. parts];
        sublist.RemoveAt(i);
        bool decreasing = int.Parse(sublist[0]) - int.Parse(sublist[1]) > 0;
        if (IsSafeReportPart1([.. sublist]) == 0)
        {
            return true;
        }
    }
    return false;
}


int         safeReports1 = 0;
int         safeReports2 = 0;
string[]    lines       = File.ReadAllLines("e:/Programming/Advent_of_Code_2024/Day 2 - Red-Nosed Reports/input.txt");

foreach (string line in lines)
{
    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
    safeReports1 += IsSafeReportPart1(parts) == 0 ? 1 : 0;
    safeReports2 += IsSafeReportPart2_BruteForce(parts) ? 1 : 0;
}

Console.WriteLine(safeReports1); // Part 1 answer
Console.WriteLine(safeReports2); // Part 2 answer
