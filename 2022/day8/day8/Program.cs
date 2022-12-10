

using System.Diagnostics;

static class Program
{

    static int x, y;

    public static void Main(string[] args)
    {

        string[] input = File.ReadAllLines(@"../../../../input.txt");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int forestSize = input.Length;
        int numOutsideTrees = forestSize * 4 - 4;

        List<List<Tree>> horizontalForest = input.Select(line => ToTreeLine(line)).ToList();
        List<List<Tree>> verticalForest = VerticalizeForest(forestSize, horizontalForest);

        DetermineVisibleTrees(horizontalForest);
        DetermineVisibleTrees(verticalForest);
        // Reverse forests to handle looking from other sides
        horizontalForest.ForEach(line => line.Reverse());
        verticalForest.ForEach(line => line.Reverse());
        DetermineVisibleTrees(horizontalForest);
        DetermineVisibleTrees(verticalForest);

        // Select distinct tree count to ignore any duplicates
        List<Tree> visibleTrees = new();
        horizontalForest.ForEach(treeLine => treeLine.ForEach(tree => AddToVisible(tree, visibleTrees)));
        Console.WriteLine(visibleTrees.Distinct().Count());
      
        
        stopwatch.Stop();
        Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    private static void AddToVisible(Tree tree, List<Tree> visibleTrees)
    {
        if (tree.visible) { visibleTrees.Add(tree); }
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

    private static void DetermineVisibleTrees(List<List<Tree>> forest)
    {
        // Add first trees
        forest.ForEach(treeLine => treeLine[0].MarkVisible());
        // Check visibility for middle trees
        forest.Skip(1).SkipLast(1).ToList().ForEach(lineOfTrees => DetermineVisibilityFor(lineOfTrees));
    }

    // Get every distinct visible tree in this line
    private static void DetermineVisibilityFor(List<Tree> lineOfTrees)
    {
        int treeIndex = 0;
        int maxTreeHeight = 0;
        while (treeIndex < lineOfTrees.Count)
        {
            Tree currentTree = lineOfTrees[treeIndex++];
            if (currentTree.height > maxTreeHeight)
            {
                // Each time we find a bigger tree we mark it as visible
                maxTreeHeight = currentTree.height;
                currentTree.MarkVisible();
            }
        }
    }

    private static List<Tree> ToTreeLine(string line)
    {
        var tmp = line.Select(height => new Tree(x++, y, height - '0')).ToList();
        y++;
        x = 0;
        return tmp;
    }

    public class Tree
    {
        public int x, y, height;
        public bool visible { get; private set; }

        public Tree(int x, int y, int height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.visible = false;
        }

        public void MarkVisible()
        {
            this.visible = true;
        }
    }

}