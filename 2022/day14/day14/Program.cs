
using System.Text;

class Program
{
    public static void Main()
    {
        SolveA();
    }
    
    private static void SolveA()
    {
        Grid grid = new Grid(File.ReadAllLines("../../../../input.txt"));
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

    public class Grid
    {
        const char AIR = '.';
        const char ROCK = 'x';
        const char SAND = 'o';
        const char SANDORIGIN = '+';

        char[][] grid;
        (int x, int y)[][] paths;
        (int x, int y) bounds, sandOrigin;
        
        public Grid(string[] input)
        {
            PopulateGrid(input);
            SimulateSand();
        }

        private void SimulateSand()
        {
            Sand sand = new Sand(sandOrigin.x, sandOrigin.y);
            while (true)
            {
                if (!sand.moving)
                    sand = new Sand(sandOrigin.x, sandOrigin.y);
                
                DropSand(sand);
                if (sand.fellToInfinity)
                {
                    Console.WriteLine("Finished!");
                    break;
                }
                DrawGrid();
                //System.Threading.Thread.Sleep(500);
            }
            Console.WriteLine(grid.Select(line => line.Where(c => c == SAND).Count()).Sum());
        }

        private Sand DropSand(Sand sand)
        {
            int[] offsets = new int[] { 0, -1, 1 };

            foreach (int offset in offsets)
            {
                (int newX, int newY) = (sand.x + offset, sand.y + 1);
                if ((!(newX >= 0 && newX < bounds.x) || newY >= bounds.y))
                {
                    sand.moving = true;
                    sand.fellToInfinity = true;
                    return sand;
                }
                if (grid[newY][newX] == AIR)
                {
                    sand.x = newX;
                    sand.y = newY;
                    return DropSand(sand);
                }
            }
            grid[sand.y][sand.x] = SAND;
            sand.moving = false;
            return sand;
        }

        private void PopulateGrid(string[] input)
        {
            ParseData(input);

            grid = new char[bounds.y][].Select(line => Enumerable.Repeat('.', bounds.x).ToArray()).ToArray();

            foreach ((int, int)[] path in paths)
            {
                int cnt = 0;
                while (++cnt < path.Length)
                {
                    (int prevX, int prevY) = path[cnt - 1];
                    (int currX, int currY) = path[cnt];
                    (int lowX, int lowY) = (Math.Min(prevX, currX), Math.Min(prevY, currY));
                    (int highX, int highY) = (Math.Max(prevX, currX), Math.Max(prevY, currY));

                    for (int x = lowX; x <= highX; x++)
                    {
                        for (int y = lowY; y <= highY; y++)
                        {
                            grid[y][x] = ROCK;
                        }
                    }
                }
            }
        }

        private void ParseData(string[] input)
        {
            paths = input.Select(line => line.Split(" -> ")
                            .Select(parts => parts.Split(',')
                            .Select(part => int.Parse(part)).ToArray())
                            .Select(intCombo => (x: intCombo[0], y: intCombo[1])).ToArray()).ToArray();

            // Adjust boundaries to start at 0 for sanity's sake
            int minX = paths.Select(path => path.Select(coord => coord.x).Min()).Min();
            int minY = 0;
            int maxX = paths.Select(path => path.Select(coord => coord.x).Max()).Max();
            int maxY = paths.Select(path => path.Select(coord => coord.y).Max()).Max();
            paths = paths.Select(path => path.Select(coord => (coord.x - minX, coord.y - minY)).ToArray()).ToArray();

            bounds = (maxX - minX + 1, maxY - minY + 1);
            sandOrigin = (500 - minX, 0);
        }

        public void DrawGrid()
        {
            StringBuilder sb = new();
            for (int y = 0; y < bounds.y; y++)
            {
                for (int x = 0; x < bounds.x; x++)
                {
                    if (x == sandOrigin.x && y == sandOrigin.y)
                    {
                        sb.Append(SANDORIGIN);
                        continue;
                    }
                    sb.Append(grid[y][x]);
                }
                sb.Append('\n');
            }
            sb.Append("\n\n");
            Console.WriteLine(sb.ToString());
        }
    }
}