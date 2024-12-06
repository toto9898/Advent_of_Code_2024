class Program
{
    static int GetUpdateValue(List<int> update, Dictionary<int, List<int>> lessMap)
    {
        bool    badOrder    = false;
        for (int i = 0; i < update.Count; i++)
        {
            int page = update[i];
            if (lessMap.TryGetValue(page, out List<int>? lessPages))
            {
                // find if a page from lessPages is in update after page
                for (int j = i + 1; j < update.Count; j++)
                {
                    if (lessPages.Contains(update[j]))
                    {
                        badOrder = true;
                        break;
                    }
                }

                if (badOrder) break;
            }
        }

        return !badOrder ? update[update.Count / 2] : 0;
    }

    static void GetOrderRulesAndUpdates(string[] parts, out Dictionary<int, List<int>> lessMap, out List<List<int>> updates)
    {
        lessMap = [];
        updates = [];

        foreach (string part in parts)
        {
            string line = part.Trim();
            if (line.Length == 0) continue;
            
            if (line.Length >= 3 && line[2] == '|')
            {
                string[] orderRule = line.Split('|');
                int a = int.Parse(orderRule[0].Trim());
                int b = int.Parse(orderRule[1].Trim());
                if (!lessMap.ContainsKey(b))
                {
                    lessMap[b] = [];
                }
                lessMap[b].Add(a);
            }
            else if (line.Length >= 3 && line[2] == ',')
            {
                updates.Add([]);
                string[] update = line.Split(',');
                foreach ( string pageStr in update )
                {
                    if (pageStr.Trim().Length == 0) continue;
                    int page = int.Parse(pageStr.Trim());
                    updates[updates.Count - 1].Add(page);
                }
            }
        }
    }

    static int FixUpdateOrderAndGetValue(List<int> update, Dictionary<int, List<int>> lessMap)
    {
        // fuck it, it gets the job done
        while (true)
        {
            bool    fixedOrder  = false;
            for (int i = 0; i < update.Count; i++)
            {
                int page = update[i];
                if (lessMap.TryGetValue(page, out List<int>? lessPages))
                {
                    // find if a page from lessPages is in update after page
                    for (int j = i + 1; j < update.Count; j++)
                    {
                        if (lessPages.Contains(update[j]))
                        {
                            int temp = update[i];
                            update[i] = update[j];
                            update[j] = temp;
                            fixedOrder = true;
                            break;
                        }
                    }
                }
            }

            if (!fixedOrder) break;
        }
        
        return GetUpdateValue(update, lessMap);
    }

    static void Main()
    {
        string          projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string          inputFilePath   = Path.Combine(projectPath, "input.txt");
        string[]        parts           = File.ReadAllLines(inputFilePath);

        Dictionary<int, List<int>>  lessMap;
        List<List<int>>             updates;
        GetOrderRulesAndUpdates(parts, out lessMap, out updates);
        
        int result          = 0;
        int resultBadOrder  = 0;
        foreach (List<int> update in updates)
        {
            int currResult = GetUpdateValue(update, lessMap);
            if (currResult == 0)
            {
                resultBadOrder += FixUpdateOrderAndGetValue(update, lessMap);
            }
            else
            {
                result += currResult;
            }
        }

        Console.WriteLine(result);
        Console.WriteLine(resultBadOrder);
    }
}
