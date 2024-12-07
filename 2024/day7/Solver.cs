namespace AdventOfCode
{
   public class Solver
   {
      enum Operator { None, Add, Multiply, Concatenate }

      static void Main(string[] args)
      {
         var equations = ParseInput("input.txt");

         // Filter out the equations that are possible to solve
         var possibleEquations = equations.Where(equation => IsPossible(equation, allowConcatenate: false)).ToArray();
         Console.WriteLine($"Part 1: {possibleEquations.Select(equation => equation.result).Sum()}");

         // Filter out the equations that are possible to solve, allowing concatenation
         possibleEquations = equations.Where(equation => IsPossible(equation, allowConcatenate: true)).ToArray();
         Console.WriteLine($"Part 2: {possibleEquations.Select(equation => equation.result).Sum()}");
      }

      // Parse the input file into an array of tuples, where each tuple contains the result and the test values
      private static (long result, long[] testValues)[] ParseInput(string inputPath)
      {
         var lines = File.ReadAllLines(inputPath);
         return lines.Select(line => line.Split(":"))
              .Select(parts => (result: long.Parse(parts[0]), testValues: parts[1].Trim().Split(" ").Select(long.Parse).ToArray())).ToArray();
      }

      // Check if it is possible to get the result using the test values and the operators
      private static bool IsPossible((long result, long[] testValues) equation, bool allowConcatenate)
      {
         var numOperators = equation.testValues.Length - 1;
         return IsPossible(equation.testValues, equation.result, new Operator[numOperators], 0, allowConcatenate);
      }

      // Recursive function to check if it is possible to get the result using the test values by trying all permutations of the operators
      private static bool IsPossible(long[] testValues, long result, Operator[] operators, long index, bool allowConcatenate)
      {
         // If we have filled all operators, check if the result is correct
         if (index == operators.Length)
            return CalculateResult(testValues, operators) == result;

         // If we have yet to fill all operators, permutate over possible operators, and recurse
         operators[index] = Operator.Add;
         if (IsPossible(testValues, result, operators, index + 1, allowConcatenate))
            return true;
         operators[index] = Operator.Multiply;
         if (IsPossible(testValues, result, operators, index + 1, allowConcatenate))
            return true;
         if (allowConcatenate)
         {
            operators[index] = Operator.Concatenate;
            if (IsPossible(testValues, result, operators, index + 1, allowConcatenate))
               return true;
         }

         // Nothing worked. Must be impossible to get the result with the current operators
         return false;
      }

      // Calculate the result using the test values and the operators
      private static long CalculateResult(long[] testValues, Operator[] operators)
      {
         var result = testValues[0];
         for (long i = 0; i < operators.Length; i++)
         {
            result = operators[i] switch
            {
               Operator.Add => result += testValues[i + 1],
               Operator.Multiply => result *= testValues[i + 1],
               Operator.Concatenate => long.Parse(result.ToString() + testValues[i + 1].ToString()),
               _ => throw new InvalidOperationException("Invalid operator")
            };
         }
         return result;
      }
   }
}
