
using System.Text;

class ProgramB
{
    public static void Main()
    {
        SolveA();
    }

    static List<(int x, int y)> rocks = new();
    static List<(int x, int y)> heap = new();
    static int boundsY;


    private static void SolveA()
    {
        (int x, int y) sandOrigin;
        ParseData(File.ReadAllLines("../../../../input.txt"), out rocks, out sandOrigin, out boundsY);
        SimulateSand(new Sand(sandOrigin.x, sandOrigin.y));
    }

    private static void ParseData(string[] input, out List<(int x, int y)> rocks, out (int x, int y) sandOrigin, out int boundsY)
    {
        var paths = input.Select(line => line.Split(" -> ")
                        .Select(parts => parts.Split(',')
                        .Select(part => int.Parse(part)).ToArray())
                        .Select(intCombo => (x: intCombo[0], y: intCombo[1])).ToArray()).ToArray();
        sandOrigin = (500, 0);
        rocks = new();
        int highestY = int.MinValue;
        foreach ((int, int)[] path in paths)
        {
            int cnt = 0;
            while (++cnt < path.Length)
            {
                (int prevX, int prevY) = path[cnt - 1];
                (int currX, int currY) = path[cnt];
                (int lowX, int lowY) = (Math.Min(prevX, currX), Math.Min(prevY, currY));
                (int highX, int highY) = (Math.Max(prevX, currX), Math.Max(prevY, currY));

                if (highY > highestY)
                    highestY = highY;

                for (int x = lowX; x <= highX; x++)
                {
                    for (int y = lowY; y <= highY; y++)
                    {
                        rocks.Add((x, y));
                    }
                }
            }
        }
        boundsY = highestY;
    }

    private static void SimulateSand(Sand sand)
    {
        while (true)
        {
            if (!sand.moving)
            {
                if (sand.x == 500 && sand.y == 0)
                {
                    break;
                }    
                sand = new Sand(500, 0);

            }
            sand = DropSand(sand);
        }

        Console.WriteLine(heap.Count());
    }

    private static Sand DropSand(Sand sand)
    {
        int[] offsets = new int[] { 0, -1, 1 };

        foreach (int offset in offsets)
        {
            (int newX, int newY) = (sand.x + offset, sand.y + 1);
            if (!(rocks.Contains((newX, newY)) || heap.Contains((newX, newY))) && newY <= boundsY + 1)
            {
                return DropSand(new Sand(newX, newY));
            }
        }
        heap.Add((sand.x, sand.y));
        sand.moving = false;
        return sand;
    }

    public class Sand { 
        public int x, y; 
        public bool moving = true;
        internal bool fellToInfinity;

        public Sand(int x, int y, bool moving = true)
        {
            this.x = x;
            this.y = y;
            this.moving = moving;
        }
    }

    //public class Grid
    //{
    //    const char AIR = '.';
    //    const char ROCK = 'x';
    //    const char SAND = 'o';
    //    const char SANDORIGIN = '+';

    //    char[][] grid;
    //    (int x, int y)[][] paths;
    //    (int x, int y) bounds, sandOrigin;
        
    //    public Grid(string[] input)
    //    {
    //        PopulateGrid(input);
    //        SimulateSand();
    //    }





    //    private void PopulateGrid(string[] input)
    //    {
    //        ParseData(input);


    //    }


    //}
}