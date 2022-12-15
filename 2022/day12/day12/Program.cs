using System.Diagnostics;
using static Program.Grid;
using static Program.Grid.Tile;

class Program
{

    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] input = File.ReadAllLines(@"../../../../input.txt");
        Solve(input, partOne: true);
        Solve(input, partOne: false);

        stopwatch.Stop();
        Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    private static void Solve(string[] input, bool partOne)
    {
        Grid.Initialize(input);
        List<int> distances = new List<int>();

        var ans = -1; 
        if (partOne)
        {
            ans = SolveDijkstra(Start, Target).Count - 1;
        }
        else
        {
            ans = grid.Select(row => row.Where(tile => tile != Target && tile.heightLevel == 'a')
                    .Select(start => SolveDijkstra(start, Target).Count - 1)
                    .Where(distance => distance > 0).Min()).Min();
        }
        Console.WriteLine(String.Format("Part {0} - Steps: {1}", partOne ? 'A' : 'B', ans));
    }

    private static List<Tile> SolveDijkstra(Tile source, Tile? target)
    {
        Dictionary<Tile, int> dist = new();
        Dictionary<Tile, Tile> prev = new();
        List<Tile> QContents = new List<Tile>();
        dist[source] = 0;
        PriorityQueue<Tile, int> Q = new PriorityQueue<Tile, int>();
        Q.Enqueue(source, dist[source]);
        QContents.Add(source);
        while (Q.Count > 0)
        {
            Tile u = Q.Dequeue();
            QContents.Remove(u);
            if (u == target)
                break;
            foreach (Tile v in u.neighbours)
            {
                var alt =  dist[u] + 1;
                if (alt < (dist.ContainsKey(v) ? dist[v] : int.MaxValue))
                {
                    dist[v] = alt;
                    prev[v] = u;
                    if (! QContents.Contains(v))
                    {
                        Q.Enqueue(v, alt);
                        QContents.Add(v);
                    }
                }
            }
        }
        List<Tile> path = new List<Tile>();
        if (prev.ContainsKey(target) || target == source)
        {
            while (target != null)
            {
                path.Add(target);
                target = prev.ContainsKey(target) ? prev[target] : null;
            }
        }
        return path;
    }

    public class Grid
    {
        public static Tile[][] grid;
        public static (int x, int y) gridSize { get; private set; }
        public static Tile Start { get; private set; }
        public static Tile Target { get; private set; }

        public static void Initialize(string[] input)
        {
            gridSize = (input[0].Length, input.Length);
            grid = new Tile[gridSize.y][];
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[y] = new Tile[gridSize.x];
                for (int x = 0; x < gridSize.x; x++)
                {
                    Tile newTile = new Tile(x, y, input[y][x]);
                    grid[y][x] = newTile;
                    if (newTile.heightLevel == 'S')
                    {
                        newTile.heightLevel = 'a';
                        Start = newTile;
                    } else if (newTile.heightLevel == 'E')
                    {
                        newTile.heightLevel = 'z';
                        Target = newTile;
                    }
                }
            }

            foreach (Tile[] row in grid)
            {
                foreach (Tile tile in row)
                {
                    tile.FindNeighbours();
                }
            }
        }

        public static Tile? GetTile(int x, int y)
        {
            if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
            {
                return grid[y][x];
            }
            return null;
        }

        public class Tile
        {
            public int x, y;
            public char heightLevel;
            public Tile[] neighbours;

            public Tile(int x, int y, char heightLevel)
            {
                this.x = x;
                this.y = y;
                neighbours = Array.Empty<Tile>();
                this.heightLevel = heightLevel;
            }

            internal void FindNeighbours()
            {
                List<Tile> tmpNeighbours = new List<Tile>();
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    Tile? neighbourInDir = GetNeighbourInDir(dir);
                    if (neighbourInDir != null)
                        tmpNeighbours.Add(neighbourInDir);
                }
                this.neighbours = tmpNeighbours.ToArray();
            }

            private Tile? GetNeighbourInDir(Direction dir)
            {
                (int offsetX, int offsetY) = DirToOffset(dir);
                (int newX, int newY) coords = (x + offsetX, y + offsetY);
                Tile? neighbouringTile = GetTile(coords.newX, coords.newY);
                if (neighbouringTile != null)
                {
                    if (neighbouringTile.heightLevel <= this.heightLevel + 1)
                        return neighbouringTile;
                }
                return null;
            }
        }
    }

    private static (int x, int y) DirToOffset(Direction dir)
    {
        return dir switch
        {
            Direction.N => (0, -1),
            Direction.E => (1, 0),
            Direction.S => (0, 1),
            Direction.W => (-1, 0),
        };
    }

    public enum Direction { N,E,S,W }

}