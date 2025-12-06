using System.Numerics;

namespace day6
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");

         // Split the tokens on each line by whitespace
         string[][] tokens =
         [
            .. from string line in input
            select line.Split(null as char[], StringSplitOptions.RemoveEmptyEntries),
         ];

         string[][] numberStringsPerProblem = [.. tokens.Take(tokens.Length - 1)];

         // Parse the ops line
         Operation[] ops =
         [
            .. from string opString in tokens.Last()
            select opString switch
            {
               "+" => Operation.ADD,
               "*" => Operation.MULT,
               _ => throw new ArgumentException(),
            },
         ];

         // We parse the numbers for each problem
         BigInteger[][] problemNumColumns =
         [
            .. from int col in Enumerable.Range(0, numberStringsPerProblem.First().Length)
            select (
               from string[] numberStrings in numberStringsPerProblem
               select BigInteger.Parse(numberStrings[col])
            ).ToArray(),
         ];

         // Create problems (lol)
         Problem[] problemsA =
         [
            .. from int x in Enumerable.Range(0, problemNumColumns.Length)
            select new Problem(problemNumColumns[x], ops[x]),
         ];

         Console.WriteLine($"A: {Solve(problemsA)}");

         // We can solve part B by just rotating the input
         // 90 degrees to the left
         // If we then skip the last char on each line (the operator might be there)
         // we now have a list of strings, where each line holds a number or an empty line
         var rotatedInput = RotateInputLeft([.. input])
            .Select(line => string.Concat(line.SkipLast(1)))
            .ToArray();
         // Our ops just need to be reversed now
         ops = [.. ops.Reverse()];

         // Now we can build the new problems by parsing each line as a number for a problem
         // When we encounter an empty line, treat it as a separator which defines the 'end of a problem'.
         var problemsB = new List<Problem>();
         var problemNumsB = new List<BigInteger>();
         int i = 0;
         foreach (var s in rotatedInput)
         {
            // Line is empty so our problem is finished
            if (string.IsNullOrWhiteSpace(s))
            {
               problemsB.Add(new Problem([.. problemNumsB], ops[i++]));
               problemNumsB.Clear();
               continue;
            }
            problemNumsB.Add(BigInteger.Parse(s));
         }
         problemsB.Add(new Problem([.. problemNumsB], ops[i])); // Last problem doesn't have a whitespace seprator

         Console.WriteLine($"B: {Solve([.. problemsB])}");
      }

      private static BigInteger Solve(Problem[] problems)
      {
         return (from Problem prob in problems select prob.Sum).Aggregate(
            (total, next) => total + next
         );
      }

      private static string[] RotateInputLeft(string[] input)
      {
         int width = input[0].Length;
         int height = input.Length;

         char[][] newMatrix = new char[width][];
         for (int x = 0; x < width; x++)
         {
            newMatrix[x] = new char[height];
         }

         for (int oldY = 0; oldY < height; oldY++)
         {
            for (int oldX = 0; oldX < width; oldX++)
            {
               var newY = width - oldX - 1;
               var newX = oldY;
               newMatrix[newY][newX] = input[oldY][oldX];
            }
         }
         return [.. from row in newMatrix select string.Concat(row)];
      }

      public readonly struct Problem(BigInteger[] nums, Operation op)
      {
         public BigInteger[] Nums { get; } = nums;
         public Operation Op { get; } = op;

         public BigInteger Sum
         {
            get
            {
               var op = Op;
               return Nums.Aggregate(
                  (total, next) =>
                     op switch
                     {
                        Operation.ADD => total + next,
                        Operation.MULT => total * next,
                        _ => throw new ArgumentException(),
                     }
               );
            }
         }

         public override string ToString()
         {
            return $"{string.Join('\n', Nums)}\n{Op}";
         }
      }

      public enum Operation
      {
         ADD,
         MULT,
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
