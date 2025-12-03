using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace day3
{
   public class Solver
   {

      public static void Main(string[] args)
      {
         var input = Parse_Input("input.txt");


         List<int[]> banks = input.Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToList();
         Stopwatch stopwatch = new();
         stopwatch.Start();
         Console.WriteLine($"A: {SolveA(banks)}");
         Console.WriteLine($"B: {SolveB(banks)}");
         stopwatch.Stop();

         Console.WriteLine($"Solved in: {stopwatch.ElapsedMilliseconds}ms");
      }

      private static int SolveA(List<int[]> banks)
      {
         return banks.Select(FindLargestJoltage).Sum();
      }

      private static int FindLargestJoltage(int[] bank)
      {
         return bank.Select((rating, i) =>
         {
            // skip at end because next joltage is
            // only one digit
            if (i == bank.Length - 1)
               return "0";
            // consider the rest of the bank
            // and take its max rating
            var rest = bank[(i + 1)..];
            return $"{rating}{rest.Max()}";
         })
         .Select(int.Parse)
         .Max();
      }

      private static BigInteger SolveB(List<int[]> banks)
      {
         var largestJoltages = banks.Select(FindLargestJoltageB);

         var sum = BigInteger.Zero;
         largestJoltages.ToList().ForEach(
               largestJoltage =>
               {
                  sum += largestJoltage;
               });
         return sum;
      }

      private static BigInteger FindLargestJoltageB(int[] bank)
      {
         Stack<int> stack = new();

         for (int i = 0; i < bank.Length; i++)
         {
            int ratingCandidate = bank[i];

            while (stack.Count > 0 && // stack not empty
                   stack.Peek() < ratingCandidate && // candidate is better than current rating 
                   (stack.Count - 1 + (bank.Length - i)) >= 12) // enough digits leftto add to 12 
            {
               // discard rating 
               stack.Pop();
            }

            if (stack.Count < 12)
            {
               stack.Push(ratingCandidate);
            }
         }

         int[] result = stack.Reverse().ToArray();

         StringBuilder sb = new();
         foreach (var d in result)
            sb.Append(d);

         return BigInteger.Parse(sb.ToString());
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
