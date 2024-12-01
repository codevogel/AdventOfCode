namespace AdventOfCode
{
   public class Solver
   {
      static void Main(string[] args)
      {
         string inputPath = "input.txt";

         var (left, right) = Parse(inputPath);
         var answerA = SolveA(left, right);
         Console.WriteLine(answerA);
         var answerB = SolveB(left, right);
         Console.WriteLine(answerB);
      }

      private static (int[] left, int[] right) Parse(string inputPath)
      {
         var lines = System.IO.File.ReadAllLines(inputPath);
         var separator = "   ";
         var split_lines = lines.Select(line => line.Split(separator));
         return (
            left: split_lines.Select(part => int.Parse(part[0])).ToArray(),
            right: split_lines.Select(part => int.Parse(part[1])).ToArray()
         );
      }

      private static int SolveA(int[] left, int[] right)
      {
         // Sort to match numbers
         left = left.OrderBy(number => number).ToArray();
         right = right.OrderBy(number => number).ToArray();
         // Get sum of absolute differences 
         return left.Zip(right, (leftNumber, rightNumber) => Math.Abs(leftNumber - rightNumber)).Sum();
      }

      private static int SolveB(int[] left, int[] right)
      {
         // Get sum of leftNumber * count of rightNumber where leftNumber == rightNumber
         return left.Select(leftNumber => leftNumber * right.Count(rightNumber => leftNumber == rightNumber)).Sum();
      }
   }
}
