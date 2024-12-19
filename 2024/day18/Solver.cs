namespace AdventOfCode
{
   public class Solver
   {
      public static void Main(string[] args)
      {
         var inputPath = "input.txt";
         var lines = File.ReadAllLines(inputPath);

         List<Vector2i> coords = File.ReadAllLines(inputPath)
            .Select(line => line.Split(','))
            .Select(parts => new Vector2i(int.Parse(parts[0]), int.Parse(parts[1])))
            .ToList();
         var grid = new Grid<bool>(new Vector2i(70 + 1, 70 + 1), false);

         // Let the the first kB bits fall into place 
         for (int i = 0; i < 1024; i++)
         {
            grid[coords[i]] = true;
         }

         // Find shortest path from (0, 0) to (70, 70)
         var pathFinder = new PathFinder<bool>(
               new DijkstraPathFindingStrategy<bool>(
                  Start: new Vector2i(0, 0),
                  End: new Vector2i(70, 70),
                  isWalkable: cell => !cell
               )
            );
         var path = pathFinder.FindPath(grid);
         // Print the number of steps (nodes in path - 1)
         Console.WriteLine($"Ans A: {path.Count() - 1}");

         // For each of the remaining bits, let it fall into place and check if the path is still valid
         for (int i = 1024; i < coords.Count(); i++)
         {
            grid[coords[i]] = true;
            // If the returned path is empty, no path can be found, and thus we have found the answer
            if (pathFinder.FindPath(grid).Count() == 0)
            {
               Console.WriteLine($"Ans B: {coords[i]}");
               break;
            }
         }

      }
   }

   /// <summary>
   /// Grid class for representing a 2D grid of type <typeparamref name="T"/>.
   /// <typeparamref name="T"/> The type of the grid.
   /// </summary>
   public class Grid<T>
   {
      /// <summary> The content of the grid </summary>
      private T[][] _content;

      /// <summary> 
      /// The size of the grid.
      /// <remarks>Assumes that the grid is rectangular.</remarks>
      /// <returns>The size of the grid.</returns>
      /// </summary>
      public Vector2i Size =>
         new Vector2i(_content.Length, _content[0].Length);

      /// <summary>
      /// Initialize grid with data.
      /// <paramref name="data"/> The data of the grid.
      /// </summary>
      public Grid(T[][] data)
      {
         _content = data;
      }

      /// <summary>
      /// Initialize grid with size and default value. 
      /// <paramref name="size"/> The size of the grid.
      /// <paramref name="defaultValue"/> The default value of the grid.
      /// </summary>
      public Grid(Vector2i size, T defaultValue)
      {
         _content = Enumerable.Range(0, size.y).Select(_ => Enumerable.Repeat(defaultValue, size.x).ToArray()).ToArray();
      }

      /// <summary>
      /// Get or set the value at a specific position.
      /// <throws>Throws <see cref="IndexOutOfRangeException"/> if the position is out of bounds.</throws>
      /// </summary>
      public T this[Vector2i pos]
      {
         get
         {
            if (IsInBounds(pos))
               return _content[pos.y][pos.x];
            throw new IndexOutOfRangeException();
         }
         set
         {
            if (IsInBounds(pos))
               _content[pos.y][pos.x] = value;
            throw new IndexOutOfRangeException();
         }
      }

      /// <summary>
      /// Get all coordinates in the grid.
      /// <returns>A list of all coordinates in the grid.</returns>
      /// </summary>
      public List<Vector2i> Coords =>
         Enumerable.Range(0, Size.y)
            .SelectMany(y => Enumerable.Range(0, Size.x).Select(x => new Vector2i(x, y)))
            .ToList();

      /// <summary>
      /// Check if a position is in bounds.
      /// <paramref name="pos"/> The position to check.
      /// <returns>True if the position is in bounds, otherwise false.</returns>
      /// </summary>
      public bool IsInBounds(Vector2i pos)
      {
         return pos.x >= 0 && pos.x < Size.x && pos.y >= 0 && pos.y < Size.y;
      }

      /// <summary>
      /// Get the coordinates of the neighbours of a position.
      /// <paramref name="vertex"/> The position to get the neighbours of.
      /// <returns>A list of the coordinates of the neighbours of the position.</returns>
      /// </summary>
      public List<Vector2i> GetInBoundsNeighbourCoords(Vector2i vertex)
      {
         var offsets = new Vector2i[] {
            new (0, 1), new (1, 0), new (0, -1), new (-1, 0)
         };
         return offsets.Select(offset => vertex + offset)
            .Where(IsInBounds)
            .ToList();
      }

      /// <summary>
      /// Get the neighbours of a position.
      /// <paramref name="vertex"/> The position to get the neighbours of.
      /// <returns>A list of the neighbours of the position.</returns>
      /// </summary>
      public List<T> GetNeighbours(Vector2i vertex)
      {
         return GetInBoundsNeighbourCoords(vertex)
            .Select(pos => this[pos])
            .ToList();
      }

      /// <summary>
      /// Convert the grid to a string.
      /// <paramref name="cellToString"/> A function to convert a cell of type <typeparamref name="T"/> to a string. 
      /// <returns>A string representation of the grid.</returns>
      /// </summary>
      public string ToString(Func<T, string> cellToString)
      {
         return string.Join("\n", _content.Select(row => string.Join("", row.Select(cellToString))));
      }
   }

   /// <summary>
   /// The PathFinder finds a path in a Grid using an IPathFindingStrategy. 
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public class PathFinder<T>
   {
      /// <summary> The path finding strategy. </summary>
      private IPathFindingStrategy<T> pathFindingStrategy;

      /// <summary>
      /// Initialize the PathFinder with a path finding strategy.
      /// <paramref name="pathFindingStrategy"/> The path finding strategy to use.
      /// </summary>
      public PathFinder(IPathFindingStrategy<T> pathFindingStrategy)
      {
         this.pathFindingStrategy = pathFindingStrategy;
      }

      /// <summary>
      /// Find a path in a grid.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid)
      {
         return pathFindingStrategy.FindPath(grid);
      }
   }

   /// <summary>
   /// The IPathFindingStrategy interface defines a path finding strategy for finding a path in a grid.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public interface IPathFindingStrategy<T>
   {

      /// <summary>
      /// Find a path in a grid.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid);
   }

   /// <summary>
   /// The IStartEndPathFindingStrategy interface defines a path finding strategy for finding a path in a grid between two points.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public interface IStartEndPathFindingStrategy<T> : IPathFindingStrategy<T>
   {
      /// <summary>
      /// Find a path in a grid between two points.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <paramref name="start"/> The start position.
      /// <paramref name="end"/> The end position.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid, Vector2i start, Vector2i end);
   }

   /// <summary>
   /// The StartEndPathFindingStrategy class is a base class for 
   /// path finding strategies that find a path between two points.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public abstract class StartEndPathFindingStrategy<T> : IStartEndPathFindingStrategy<T>
   {
      /// <summary> The start position. </summary>
      public Vector2i Start { get; set; }
      /// <summary> The end position. </summary>
      public Vector2i End { get; set; }

      public abstract IEnumerable<Vector2i> FindPath(Grid<T> grid, Vector2i start, Vector2i end);

      public IEnumerable<Vector2i> FindPath(Grid<T> grid)
      {
         return FindPath(grid, Start, End);
      }
   }


   /// <summary>
   /// The DijkstraPathFindingStrategy class is a path finding strategy 
   /// that finds the shortest path between two points using Dijkstra's algorithm.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public class DijkstraPathFindingStrategy<T> : StartEndPathFindingStrategy<T>
   {
      /// <summary> A function to check if a cell is walkable. </summary>
      private readonly Func<T, bool> _isWalkable;

      /// <summary>
      /// Initialize the DijkstraPathFindingStrategy with a start position, an end position, and a function to check if a cell is walkable.
      /// <paramref name="Start"/> The start position.
      /// <paramref name="End"/> The end position.
      /// <paramref name="isWalkable"/> A function to check if a cell is walkable.
      /// </summary>
      public DijkstraPathFindingStrategy(Vector2i Start, Vector2i End, Func<T, bool> isWalkable)
      {
         _isWalkable = isWalkable;
         this.Start = Start;
         this.End = End;
      }

      /// <summary>
      /// Find a path in a grid between two points using Dijkstra's algorithm.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <paramref name="start"/> The start position.
      /// <paramref name="end"/> The end position.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public override IEnumerable<Vector2i> FindPath(Grid<T> grid, Vector2i start, Vector2i end)
      {
         var priorityQueue = new PriorityQueue<Vector2i, int>();
         var previous = new Dictionary<Vector2i, Vector2i>();
         var distances = new Dictionary<Vector2i, int>();

         // Initialize distances to 'infinity'
         grid.Coords.ForEach(pos => distances[pos] = int.MaxValue);

         // Set start distance to 0 and add to queue
         distances[start] = 0;
         priorityQueue.Enqueue(start, 0);

         // While the queue is not empty
         while (priorityQueue.Count > 0)
         {
            // Consider the vertex with the smallest distance
            var currentVertex = priorityQueue.Dequeue();

            // If we've reached the end, break
            if (currentVertex.Equals(end))
               break;

            // Check all neighbors

            grid.GetInBoundsNeighbourCoords(currentVertex).ForEach(neighbor =>
            {
               // Skip if the neighbor is not walkable
               if (!_isWalkable(grid[neighbor]))
                  return;

               // Calculate new distance (in this case, each step costs 1)
               var alternative = distances[currentVertex] + 1;

               // If we found a better path, update it
               if (alternative < distances[neighbor])
               {
                  distances[neighbor] = alternative;
                  previous[neighbor] = currentVertex;

                  // Add to priority queue with new priority
                  priorityQueue.Enqueue(neighbor, alternative);
               }
            });
         }

         // If previous does not contain the end, we did not find a path
         if (!previous.ContainsKey(end))
            return new List<Vector2i>();

         // Reconstruct path by traversing backwards
         var path = new List<Vector2i>();
         var current = end;
         while (!current.Equals(start))
         {
            path.Add(current);
            current = previous[current];
         }
         path.Add(start);
         path.Reverse();
         return path;
      }
   }

   /// <summary>
   /// The Vector2i struct represents a 2D vector with integer components.
   /// </summary>
   public record struct Vector2i(int x, int y)
   {
      public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
      public override string ToString() => $"({x}, {y})";
   }
}
