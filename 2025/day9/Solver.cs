using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace day9
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");

         Stopwatch stopwatch = new();
         stopwatch.Start();

         var redTiles = input
            .Select(line => line.Split(','))
            .Select(parts => new Vector2i(
               int.Parse(parts[0].ToString()),
               int.Parse(parts[1].ToString())
            ));

         List<BigInteger> areas =
         [
            .. redTiles.SelectMany(
               (a, i) => redTiles.Skip(i + 1),
               (a, b) =>
               {
                  var area = (BigInteger.Abs(a.X - b.X) + 1) * (BigInteger.Abs(a.Y - b.Y) + 1);
                  return area;
               }
            ),
         ];
         Console.WriteLine($"A: {areas.Max()}");

         // Get all bounds
         List<(Vector2i start, Vector2i end)> bounds =
         [
            .. redTiles.Select(
               (tile, i) =>
               {
                  var next =
                     (i == redTiles.Count() - 1) ? redTiles.First() : redTiles.ElementAt(i + 1);
                  return (start: tile, end: next);
               }
            ),
         ];

         // All combos we should try
         List<(Vector2i a, Vector2i b)> combos =
         [
            .. redTiles.SelectMany((a, i) => redTiles.Skip(i + 1), (a, b) => (a, b)),
         ];

         // Get max of areas
         BigInteger maxAreaPartB = combos.Max(pair =>
         {
            var a = pair.a;
            var b = pair.b;

            // Calc the bounds of the rect formed by the red tiel
            int minX = Math.Min(a.X, b.X);
            int maxX = Math.Max(a.X, b.X);
            int minY = Math.Min(a.Y, b.Y);
            int maxY = Math.Max(a.Y, b.Y);

            var area = (BigInteger)(maxX - minX + 1) * (maxY - minY + 1);

            bool invalid = bounds.Any(bound =>
            {
               int boundMinX = Math.Min(bound.start.X, bound.end.X);
               int boundMaxX = Math.Max(bound.start.X, bound.end.X);
               int boundMinY = Math.Min(bound.start.Y, bound.end.Y);
               int boundMaxY = Math.Max(bound.start.Y, bound.end.Y);

               // Invalid when the rect intersects the bound
               return boundMinX < maxX && boundMaxX > minX && boundMinY < maxY && boundMaxY > minY;
            });

            return invalid ? BigInteger.Zero : area;
         });

         Console.WriteLine($"B: {maxAreaPartB}");
         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");
      }

      readonly struct Vector2i(int x, int y)
      {
         public int X { get; } = x;
         public int Y { get; } = y;

         public override string ToString()
         {
            var x = X;
            var y = Y;
            return $"({x},{y})";
         }
      }

      static List<string> ParseInput(string path) => [.. File.ReadLines(path)];
   }
}
