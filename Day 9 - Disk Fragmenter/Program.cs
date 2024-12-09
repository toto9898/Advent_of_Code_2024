class Program
{
    static long SumFromAToB(long a, long b)
    {
        long n = b - a + 1;
        return n * (a + b) / 2;
    }
    class Element(int type, int count)
    {
        public int Type { get; set; } = type;
        public int Count { get; set; } = count;
        public static readonly int EmptyType = -1;
    }

    static void ResolvePart1(ref LinkedList<Element> diskMap, ref LinkedListNode<Element> head, ref LinkedListNode<Element> tail)
    {
        if (head == null || tail == null) return;

        if (head.Value.Type == Element.EmptyType && tail.Value.Type != Element.EmptyType)
        {
            if (head.Next == tail)
            {
                (tail.Value, head.Value) = (head.Value, tail.Value);
                head = head.Next;
                return;
            }

            int empty1 = head.Value.Count;
            int repeat2 = tail.Value.Count;
            int toBeAdded = Math.Min(empty1, repeat2);
            if (toBeAdded > 0 && head.Next != null)
            {
                Element newElement = new(tail.Value.Type, toBeAdded);
                LinkedListNode<Element> newElementNode = new(newElement);
                diskMap.AddBefore(head, newElementNode);

                Element newEmpty = new(Element.EmptyType, toBeAdded);
                LinkedListNode<Element> newEmptyNode = new(newEmpty);
                diskMap.AddAfter(tail, newEmptyNode);

                head.ValueRef.Count -= toBeAdded;
                tail.ValueRef.Count -= toBeAdded;
            }

            if (head.Value.Count == 0 && head.Next != null)
                head = head.Next;
            if (tail.Value.Count == 0 && tail.Previous != null)
                tail = tail.Previous;
        }
    }

    static void ResolvePart2(ref LinkedList<Element> diskMap, ref LinkedListNode<Element> head, ref LinkedListNode<Element> tail)
    {
        var headCopy = head;
        while (headCopy != tail && headCopy != null && tail != null)
        {
            if (headCopy.Value.Type == Element.EmptyType && headCopy.Value.Count >= tail.Value.Count)
            {
                ResolvePart1(ref diskMap, ref headCopy, ref tail);
                return;
            }
            headCopy = headCopy.Next;
        }
        tail = tail.Previous;
    }

    static LinkedList<Element> CopyDiskMap(LinkedList<Element> original)
    {
        LinkedList<Element> copy = new();
        foreach (var element in original)
        {
            copy.AddLast(new Element(element.Type, element.Count));
        }
        return copy;
    }

    static LinkedList<Element> Compress(in LinkedList<Element> diskMap, bool part1)
    {
        var diskMapCopy = CopyDiskMap(diskMap);
        var head        = diskMapCopy.First;
        var tail        = diskMapCopy.Last;

        while (head != tail && head != null && tail != null)
        {
            if (part1) 
                ResolvePart1(ref diskMapCopy, ref head, ref tail);
            else
                ResolvePart2(ref diskMapCopy, ref head, ref tail);

            while (head.Value.Type != Element.EmptyType && head != tail && head.Next != null)
                head = head.Next;
            while (tail.Value.Type == Element.EmptyType && head != tail && tail.Previous != null)
                tail = tail.Previous;
        }
        return diskMapCopy;
    }

    static long CheckSum(in LinkedList<Element> diskMap)
    {
        long total = 0;
        long a = 0, b = -1;
        foreach (var item in diskMap)
        {            
            b += item.Count;
            if (item.Type != Element.EmptyType)
                total += SumFromAToB(a, b) * item.Type;
            a += item.Count;
        }

        return total;
    }

    static LinkedList<Element> ReadInput()
    {
        string              projectPath     = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string              inputFilePath   = Path.Combine(projectPath, "input.txt");
        string              mapStr          = File.ReadAllLines(inputFilePath).First();
        LinkedList<Element> diskMap         = [];
        
        for (int i = 0; i < mapStr.Length; i++)
        {
            int type = i % 2 == 0 ? (i / 2) : Element.EmptyType;
            int count = mapStr[i] - '0';
            diskMap.AddLast(new Element(type, count));
        }

        return diskMap;
    }
    
    static void Main()
    {
        LinkedList<Element> diskMap = ReadInput();

        LinkedList<Element> diskMap1 = Compress(diskMap, true);
        LinkedList<Element> diskMap2 = Compress(diskMap, false);

        Console.WriteLine( CheckSum(diskMap1) );
        Console.WriteLine( CheckSum(diskMap2) );
    }
}
