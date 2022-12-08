

static class Program
{

    static int x, y;

    public static void Main(string[] args)
    {

        string[] input = File.ReadAllLines(@"../../../../input.txt");
        int forestSize = input.Length;
        int numOutsideTrees = forestSize * 4 - 4;

        List<List<Tree>> horizontalForest = input.Select(line => ToTreeLine(line)).ToList();
        List<List<Tree>> verticalForest = VerticalizeForest(forestSize, horizontalForest);

        List<Tree> visibleTrees = new();
        AddVisibleTrees(horizontalForest, visibleTrees);
        AddVisibleTrees(verticalForest, visibleTrees);
        horizontalForest.ForEach(line => line.Reverse());
        verticalForest.ForEach(line => line.Reverse());
        AddVisibleTrees(horizontalForest, visibleTrees);
        AddVisibleTrees(verticalForest, visibleTrees);
        Console.WriteLine(visibleTrees.Distinct().Count());
    }

    private static List<List<Tree>> VerticalizeForest(int forestSize, List<List<Tree>> horizontalForest)
    {
        List<List<Tree>> verticalForest = new();
        for (int y = 0; y < forestSize; y++)
        {
            List<Tree> verticalTreeLine = new();
            for (int x = 0; x < forestSize; x++)
            {
                verticalTreeLine.Add(horizontalForest[x][y]);
            }
            verticalForest.Add(verticalTreeLine);
        }

        return verticalForest;
    }

    private static void AddVisibleTrees(List<List<Tree>> forest, List<Tree> visibleTrees)
    {
        // Add first trees
        forest.ForEach(line => visibleTrees.Add(line.ElementAt(0)));
        // Check visibility for middle trees
        forest.Skip(1).SkipLast(1).Select(lineOfTrees => VisibleInLine(lineOfTrees)).ToList()
            .ForEach(treeList => treeList.ForEach(tree => visibleTrees.Add(tree)));
    }

    // Get every distinct visible tree in this line
    private static List<Tree> VisibleInLine(List<Tree> lineOfTrees)
    {
        List<Tree> visibleTrees = new();
        int treeIndex = 0;
        int maxTreeHeight = lineOfTrees[0].height;
        while (treeIndex < lineOfTrees.Count)
        {
            Tree currentTree = lineOfTrees[treeIndex++];
            if (currentTree.height > maxTreeHeight)
            {
                maxTreeHeight = currentTree.height;
                visibleTrees.Add(currentTree);
            }
        }
        return visibleTrees;
    }

    private static List<Tree> ToTreeLine(string line)
    {
        var tmp = line.Select(height => new Tree(x++, y, height - '0')).ToList();
        y++;
        x = 0;
        return tmp;
    }

    public struct Tree
    {
        public int x, y, height;

        public Tree(int x, int y, int height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
        }
    }

}