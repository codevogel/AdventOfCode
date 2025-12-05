using System.Diagnostics;
using System.Numerics;

namespace day5
{
   public class Solver
   {
      public static void Main()
      {
         var input = Parse_Input("input.txt");
         var rangeInput = input.TakeWhile(line => !string.IsNullOrEmpty(line));
         var ingredientInput = input.Skip(rangeInput.Count() + 1);

         RangeInclusive[] ranges =
         [
            .. rangeInput
               .Select(rangeString => rangeString.Split("-"))
               .Select(parts => new RangeInclusive(
                  BigInteger.Parse(parts[0]),
                  BigInteger.Parse(parts[1])
               )),
         ];

         BigInteger[] ingredients = [.. ingredientInput.Select(BigInteger.Parse)];

         Stopwatch stopwatch = new();
         stopwatch.Start();
         Console.WriteLine($"A: {SolveA(ingredients, ranges)}");
         Console.WriteLine($"B: {SolveB(ranges)}");
         stopwatch.Stop();

         Console.WriteLine($"Solved in: {stopwatch.ElapsedMilliseconds}ms");
      }

      private static BigInteger SolveB(RangeInclusive[] ranges)
      {
         Stack<RangeInclusive> rangesToCollapse = [];
         ranges.ToList().ForEach(rangesToCollapse.Push);

         List<RangeInclusive> collapsedRanges = CollapseRanges(rangesToCollapse);

         var sum = BigInteger.Zero;
         collapsedRanges.ForEach(r => sum += r.End - r.Start + 1);
         return sum;
      }

      private static List<RangeInclusive> CollapseRanges(Stack<RangeInclusive> rangesToCollapse)
      {
         List<RangeInclusive> collapsed = [];

         // While there are ranges left to consider...
         while (rangesToCollapse.Count > 0)
         {
            var current = rangesToCollapse.Pop();

            // Try to find a collapsed range that intersects with this one
            int index = collapsed.FindIndex(r => RangeInclusive.Collapse(current, r) != null);

            // None of the already-collapsed ranges insersect with this one
            // so we can add it as a separate range
            if (index < 0)
            {
               collapsed.Add(current);
               continue;
            }
            var merged = RangeInclusive.Collapse(current, collapsed[index]);

            // Remove the range that we previously thought was collapsed
            collapsed.RemoveAt(index);
            // And add th new one back onto the stack
            rangesToCollapse.Push(merged!);
         }
         return collapsed;
      }

      private static BigInteger SolveA(BigInteger[] ingredients, RangeInclusive[] ranges)
      {
         return ingredients.Count(i => ranges.Any(range => range.Includes(i)));
      }

      class RangeInclusive(BigInteger start, BigInteger end)
      {
         public BigInteger Start { get; set; } = start;
         public BigInteger End { get; set; } = end;

         public bool Includes(BigInteger num) => num >= Start && num <= End;

         public static RangeInclusive? Collapse(RangeInclusive a, RangeInclusive b)
         {
            // If matches don't intersect, return null
            if (a.End < b.Start || b.End < a.Start)
               return null;

            // else, return new resulting range
            return new RangeInclusive(
               BigInteger.Min(a.Start, b.Start),
               BigInteger.Max(a.End, b.End)
            );
         }

         public override string ToString()
         {
            return $"[{Start}..{End}]";
         }
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
