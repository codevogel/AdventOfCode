using System.Diagnostics;
using System.Numerics;

namespace day2
{
   public class Solver
   {

      public static void Main(string[] args)
      {
         var input = Parse_Input("input.txt");
         var ranges = input[0]
             .Split(',')
             .Select(rangeString => rangeString.Split('-'))
             .Select(pair => new ProductRange(BigInteger.Parse(pair[0]), BigInteger.Parse(pair[1])));

         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();
         var (sumA, sumB) = ranges.Aggregate(
             (sumA: BigInteger.Zero, sumB: BigInteger.Zero),
             (acc, range) => (
                 sumA: acc.sumA + range.GetInvalidsA(),
                 sumB: acc.sumB + range.GetInvalidsB()
             )
         );
         stopwatch.Stop();

         Console.WriteLine($"A: {sumA}");
         Console.WriteLine($"B: {sumB}");
         Console.WriteLine($"Solved in: {stopwatch.ElapsedMilliseconds}ms");
      }

      readonly struct ProductRange(BigInteger start, BigInteger end)
      {
         public BigInteger Start { get; } = start;
         public BigInteger End { get; } = end;

         // Iterate over all the numbers in this ProductRange from Start to End (inclusive)
         public readonly IEnumerable<BigInteger> Range
         {
            get
            {
               for (BigInteger i = Start; i <= End; i++)
                  yield return i;
            }
         }

         public readonly BigInteger GetInvalidsA()
         {
            return Range
                .Select(num => num.ToString())
                // Skip evaluating single digit productIDs because they're always valid
                .Where(numString => numString.Length > 1)
                // Grab the numStrings where the first half matches the latter half
                .Where(numString =>
                {
                   int half = numString.Length / 2;
                   return numString[..half] == numString[half..];
                })
                // Calculate their sum
                .Select(BigInteger.Parse)
                .Aggregate(BigInteger.Zero, (acc, val) => acc + val);
         }


         public readonly BigInteger GetInvalidsB()
         {
            return Range
                .Select(num => num.ToString())
                // Skip evaluating single digit productIDs because they're always valid
                .Where(numString => numString.Length > 1)
                // Grab the numString with repeating sequences
                .Where(numString =>
                {
                   var haystack = numString;
                   // The needle can only be as long as half the string to be repetable
                   var maxNeedleLength = haystack.Length / 2 + 1;
                   // Iterate over the possible needle lengths
                   return Enumerable.Range(1, maxNeedleLength - 1)
                    // If any of them repeat, we know this is an invalid productID
                    .Any(needleLength =>
                    {
                       // We can skip evaluation of any haystacks that don't fit the needleLength perfectly
                       // because they would never be able to repeat
                       if (haystack.Length % needleLength != 0) return false;
                       // The needle is a substring of the haystack (productID)
                       string needle = haystack[..needleLength];
                       return StringIsRepeating(needle, haystack);
                    });
                })
                .Select(BigInteger.Parse)
                .Aggregate(BigInteger.Zero, (acc, val) => acc + val);
         }

         private static bool StringIsRepeating(string needle, string haystack)
         {
            // The string repeats if all chunks of needleLength equal the needle
            for (int chunkStartIndex = 0; chunkStartIndex < haystack.Length; chunkStartIndex += needle.Length)
            {
               var chunk = haystack[chunkStartIndex..(chunkStartIndex + needle.Length)];
               if (needle != chunk)
                  // One chunk mismatches, so we return early
                  return false;
            }
            return true;
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
