class Program
{
    static int CountXMASInRows(string[] parts)
    {
        int count = 0;

        int rows    = parts.Length;
        int cols    = parts[0].Length;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols - 3; j++)
            {
                if (parts[i].Substring(j, 4) == "XMAS" || 
                    parts[i].Substring(j, 4) == "SAMX")
                {
                    count++;
                }
            }
        }

        return count;
    }

    static int CountXMASInColumns(string[] parts)
    {
        int count = 0;

        int rows    = parts.Length;
        int cols    = parts[0].Length;
        for (int i = 0; i < rows - 3; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (parts[i][j] == 'X' && parts[i + 1][j] == 'M' && parts[i + 2][j] == 'A' && parts[i + 3][j] == 'S' ||
                    parts[i][j] == 'S' && parts[i + 1][j] == 'A' && parts[i + 2][j] == 'M' && parts[i + 3][j] == 'X')
                {
                    count++;
                }
            }
        }

        return count;
    }

    static int CountXMASInDiagonals(string[] parts)
    {
        int count = 0;

        int rows    = parts.Length;
        int cols    = parts[0].Length;
        for (int i = 0; i < rows - 3; i++)
        {
            for (int j = 0; j < cols - 3; j++)
            {
                if (parts[i][j] == 'X' && parts[i + 1][j + 1] == 'M' && parts[i + 2][j + 2] == 'A' && parts[i + 3][j + 3] == 'S' ||
                    parts[i][j] == 'S' && parts[i + 1][j + 1] == 'A' && parts[i + 2][j + 2] == 'M' && parts[i + 3][j + 3] == 'X')
                {
                    count++;
                }

                if (parts[i][j + 3] == 'X' && parts[i + 1][j + 2] == 'M' && parts[i + 2][j + 1] == 'A' && parts[i + 3][j] == 'S' ||
                    parts[i][j + 3] == 'S' && parts[i + 1][j + 2] == 'A' && parts[i + 2][j + 1] == 'M' && parts[i + 3][j] == 'X')
                {
                    count++;
                }
            }
        }

        return count;
    }

    static int CountX_MAS(string[] parts)
    {
        int count = 0;

        int rows    = parts.Length;
        int cols    = parts[0].Length;
        for (int i = 0; i < rows - 2; i++)
        {
            for (int j = 0; j < cols - 2; j++)
            {
                if (parts[i+1][j+1] != 'A')
                    continue;

                if ((   parts[i][j] == 'M' && parts[i+2][j+2] == 'S' ||
                        parts[i][j] == 'S' && parts[i+2][j+2] == 'M'   ) &&
                    (   parts[i][j+2] == 'M' && parts[i+2][j] == 'S' ||
                        parts[i][j+2] == 'S' && parts[i+2][j] == 'M'   ))
                {
                    count++;
                }
            }
        }

        return count;
    }

    static void Main()
    {
        string          projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string          inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]        parts           = File.ReadAllLines(inputFilePath);

        int result1  = 0;
        result1 += CountXMASInRows(parts);
        result1 += CountXMASInColumns(parts);
        result1 += CountXMASInDiagonals(parts);
        Console.WriteLine(result1);

        int result2  = CountX_MAS(parts);
        Console.WriteLine(result2);
    }
}
