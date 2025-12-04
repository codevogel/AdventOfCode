using System.Diagnostics;

namespace day4
{
   public class Solver
   {
      public static void Main()
      {
         var input = Parse_Input("input.txt");

         bool[][] grid =
         [
            .. input.Select(line => line.ToCharArray().Select(c => c == '@').ToArray()),
         ];
         Stopwatch stopwatch = new();
         stopwatch.Start();
         Console.WriteLine($"A: {SolveA(grid)}");
         Console.WriteLine($"B: {SolveB(grid)}");
         stopwatch.Stop();

         Console.WriteLine($"Solved in: {stopwatch.ElapsedMilliseconds}ms");
      }

      public static int SolveA(bool[][] grid)
      {
         int width = grid[0].Length;
         int height = grid.Length;
         return (
            from y in Enumerable.Range(0, height)
            from x in Enumerable.Range(0, width)
            let isPaperStack = grid[y][x]
            where isPaperStack && FindNeighbouringPaperStacks(grid, (x, y), height, width) < 4
            select 1
         ).Count();
      }

      public static int SolveB(bool[][] grid)
      {
         var count = 0;
         while (true)
         {
            int width = grid[0].Length;
            int height = grid.Length;
            // Do the same thing as in A, but select the coords
            var coordsOfStacksToRemove =
               from y in Enumerable.Range(0, height)
               from x in Enumerable.Range(0, width)
               let isPaperStack = grid[y][x]
               where isPaperStack && FindNeighbouringPaperStacks(grid, (x, y), height, width) < 4
               select (x, y);

            // If we didnt find any coords of available paper stacks, we can stop
            var numAccessibleStacks = coordsOfStacksToRemove.Count();
            if (numAccessibleStacks == 0)
               break;
            // We count how many we found, and then remove the paper stacks
            count += numAccessibleStacks;
            coordsOfStacksToRemove.ToList().ForEach(coord => grid[coord.y][coord.x] = false);
         }
         return count;
      }

      private static int FindNeighbouringPaperStacks(
         bool[][] grid,
         (int x, int y) coords,
         int height,
         int width
      )
      {
         return (
            from dy in Enumerable.Range(-1, 3)
            from dx in Enumerable.Range(-1, 3)
            // Skip self
            where !(dx == 0 && dy == 0)
            // Grab only valid neighbouring coords
            let neighbourX = coords.x + dx
            let neighbourY = coords.y + dy
            where neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height
            // Grab only neighbours that are paper stacks (true)
            where grid[neighbourY][neighbourX]
            select 1
         ).Count();
      }

      static List<string> Parse_Input(string path)
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
