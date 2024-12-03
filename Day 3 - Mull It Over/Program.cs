using System.Text.RegularExpressions;

class Program
{
    static List<bool> GetOrder(string input)
    {
        string[] separators = ["do()", "don't()"];
        string pattern = string.Join("|", Array.ConvertAll(separators, Regex.Escape));

        List<bool> separatorBools = [];

        foreach (Match match in Regex.Matches(input, pattern))
        {
            separatorBools.Add(match.Value == "do()");
        }

        return separatorBools;
    } 


    static void Main()
    {
        string          input   = File.ReadAllText("e:/Programming/Advent_of_Code_2024/Day 3 - Mull It Over/input.txt");
        
        string[]        parts   = input.Split(["do()", "don't()"], StringSplitOptions.RemoveEmptyEntries);
        string          pattern = @"mul\(\d{1,3},\d{1,3}\)";
        Regex           regex   = new(pattern);
        long            result1 = 0;
        long            result2 = 0;
        
        List<bool> order = GetOrder(input);
        for (int i = 0; i < parts.Length; i++)
        {
            
            MatchCollection matchCollection = regex.Matches(parts[i]);
            foreach (Match match in matchCollection)
            {
                string[]    numbers = match.Value.Split(['(', ',', ')'], StringSplitOptions.RemoveEmptyEntries);
                int         num1    = int.Parse(numbers[1]);
                int         num2    = int.Parse(numbers[2]);
                
                result1 += num1 * num2;
                
                if (i == 0 || order[i-1])
                {
                    result2 += num1 * num2;
                }
            }
        }

        Console.WriteLine(result1);
        Console.WriteLine(result2);
    }
}
