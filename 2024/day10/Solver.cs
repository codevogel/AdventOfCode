namespace AdventOfCode
{
   class Solver
   {
      static int[][] map = new int[0][];

      static void Main(string[] args)
      {
         string inputPath = "input.txt";

         // Read the input file into a topological map
         map = File.ReadAllLines(inputPath)
            .Select(line => line.Select(c => int.Parse(c.ToString())).ToArray())
            .ToArray();

         // Get all coordinates in the map as a flat list
         var coords = from y in Enumerable.Range(0, map.Length)
                      from x in Enumerable.Range(0, map[y].Length)
                      select new Vector2i(x, y);

         // Get all trailheads (positions where the height is 0)
         var trailheads = coords.Where(c => map[c.y][c.x] == 0);

         // Calculate the score and rating of each trailhead
         var sumOfScores = trailheads.Select(startPosition => CalculateScoreOrRating(startPosition, true)).Sum();
         Console.WriteLine(sumOfScores);
         var sumOfRatings = trailheads.Select(startPosition => CalculateScoreOrRating(startPosition, false)).Sum();
         Console.WriteLine(sumOfRatings);
      }

      private static int CalculateScoreOrRating(Vector2i trailhead, bool calculatingScore)
      {
         // We only care about distinct results, so we use a HashSet to store them 
         var results = new HashSet<string>();
         ExploreTrails(trailhead, 0, new HashSet<Vector2i>(), new List<Vector2i>(), results, calculatingScore);
         return results.Count;
      }

      // Depth-first search to explore all possible trails
      private static void ExploreTrails(Vector2i current, int currentHeight, HashSet<Vector2i> visited,
                              List<Vector2i> currentTrail, HashSet<string> results, bool calculatingScore)
      {
         // If already visited, are out of bounds, or the height increase was not expected, return
         if (visited.Contains(current) || !IsInBounds(current) || map[current.y][current.x] != currentHeight)
            return;

         // Mark current position  as visited
         visited.Add(current);
         currentTrail.Add(current);

         // If we reached a summit, we have a valid trail, and we can add it to the results
         if (currentHeight == 9)
         {
            if (calculatingScore)
               // When considering score, add the position of this summit
               results.Add($"{current.x},{current.y}");
            else
            {
               // When considering rating, add the path to this summit
               var trailKey = currentTrail.Aggregate("", (acc, pos) => acc + $"{pos.x},{pos.y}");
               results.Add(trailKey);
            }
         }
         else
         {
            // Keep exploring! 
            foreach (var neighbor in GetInBoundsNeighbourCoords(current))
            {
               ExploreTrails(neighbor, currentHeight + 1, visited, new List<Vector2i>(currentTrail), results, calculatingScore);
            }
         }

         // Clean up to allow for backtracking 
         visited.Remove(current);
         currentTrail.RemoveAt(currentTrail.Count - 1);
      }

      private static Vector2i[] GetInBoundsNeighbourCoords(Vector2i position)
      {
         var offsets = new Vector2i[] { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
         return offsets.Select(offset => position + offset).Where(p => IsInBounds(p)).ToArray();
      }

      private static bool IsInBounds(Vector2i pos)
      {
         return pos.x >= 0 && pos.x < map[0].Length && pos.y >= 0 && pos.y < map.Length;
      }
   }

   record struct Vector2i(int x, int y)
   {
      public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
   }
}
