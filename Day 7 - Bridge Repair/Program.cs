class Program
{
    static long Concat(long a, long b)
    {
        long c = b;
        if (c == 0)
            return a * 10;
        else while (c > 0)
        {
            a *= 10;
            c /= 10;
        }
        return a + b;
    }

    static bool ValidEquation(long result, List<long> numbers, int index = 0, long resultAcc = 0)
    {
        if (resultAcc > result)
            return false;
        if (index == numbers.Count)
            return result == resultAcc; 

        return ValidEquation(result, numbers, index + 1, resultAcc * numbers[index]) ||
               ValidEquation(result, numbers, index + 1, resultAcc + numbers[index]) ||
               ValidEquation(result, numbers, index + 1, Concat(resultAcc, numbers[index])); // This line is for part 2
    }
    static bool ValidEquation(long result, List<long> numbers)
    {
        if (result == 0 && numbers.Any(x => x == 0))
            return true;
        else if (numbers.Count > 0)
            return ValidEquation(result, numbers, 1, numbers[0]);
        else
            return false;
    }

    static void Main()
    {
        string          projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string          inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]        equationsStr    = File.ReadAllLines(inputFilePath);

        long result = 0;

        foreach (string equationStr in equationsStr)
        {
            string[]    numbersStr  = equationStr.Split(' ');
            long        eqResult    = long.Parse(numbersStr[0].Trim()[..(numbersStr[0].Length - 1)]);
            List<long>  numbers     = numbersStr.Skip(1).Select(long.Parse).ToList(); 

            if (ValidEquation(eqResult, numbers))
                result += eqResult;
        }

        Console.WriteLine(result);
    }
}
