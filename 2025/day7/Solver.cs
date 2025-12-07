using System.Numerics;

namespace day7
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");

         char[][] grid = [.. from row in input select row.ToCharArray()];
         int height = grid.Length;
         int width = grid[0].Length;

         Vector2 start = new(grid.First().IndexOf('S'), 0);

         List<Vector2> splitters =
         [
            .. from y in Enumerable.Range(0, height)
            from x in Enumerable.Range(0, width)
            where grid[y][x] == '^'
            select new Vector2(x, y),
         ];

         Console.WriteLine(SolveA(splitters, start, height));
         Console.WriteLine(SolveB(splitters, start, height));
      }

      private static int SolveA(List<Vector2> splitters, Vector2 start, int height)
      {
         int splits = 0;
         HashSet<Vector2> beams = [start];
         for (int y = 0; y < height; y++)
         {
            // The hash set will take care of beams combining into one beam
            // when being split on the same cell
            HashSet<Vector2> nextBeams = [];
            beams
               .ToList()
               .ForEach(beam =>
               {
                  // Go down
                  var next = new Vector2(beam.X, beam.Y + 1);
                  // If hit a splitter, branch left and right, and count the split
                  if (splitters.Contains(next))
                  {
                     nextBeams.Add(new Vector2(next.X - 1, next.Y));
                     nextBeams.Add(new Vector2(next.X + 1, next.Y));
                     splits++;
                     return;
                  }
                  // Else just go down
                  nextBeams.Add(next);
               });
            // Reassign the resulting distinct beams as the new hashset
            beams = nextBeams;
         }
         return splits;
      }

      private static BigInteger SolveB(List<Vector2> splitters, Vector2 start, int height)
      {
         return CountPaths([.. splitters], start, height, []);
      }

      private static BigInteger CountPaths(
         HashSet<Vector2> splitters,
         Vector2 beamPos,
         int height,
         Dictionary<Vector2, BigInteger> memo
      )
      {
         // If we've already been here, we can just return the memo'ed amount
         // Because the result is deterministic
         if (memo.TryGetValue(beamPos, out var cached))
            return cached;

         Vector2 next = new(beamPos.X, beamPos.Y + 1);

         // If we've exited the grid, we store that this defines a single timeline
         if (next.Y >= height)
            return memo[beamPos] = 1;

         BigInteger total = BigInteger.Zero;

         if (splitters.Contains(next))
         {
            // Increment the total by the amount of branching paths from here
            total += CountPaths(splitters, new(next.X - 1, next.Y), height, memo);
            total += CountPaths(splitters, new(next.X + 1, next.Y), height, memo);
         }
         else
         {
            // No branching, but still increment the total by the amount of paths that we end up with
            // when going straight down
            total = CountPaths(splitters, next, height, memo);
         }

         // Memo the result in case we end up here in a different timeline,
         // as the result will be the same from that point.
         // We use this at the top at memo.TryGetValue
         memo[beamPos] = total;
         return total;
      }

      static List<string> ParseInput(string path)
      {
         List<string> result = [];
         var file_contents = File.ReadLines(path);
         foreach (var line in file_contents)
         {
            result.Add(line);
         }
         return result;
      }
   }
}
