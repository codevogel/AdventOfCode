

namespace AdventOfCode
{
   public class Solver
   {
      static void Main(string[] args)
      {
         string inputPath = "input.txt";
         int[][] reports = Parse(inputPath);
         int resultA = SolveA(reports);
         System.Console.WriteLine(resultA);
         int resultB = SolveB(reports);
         System.Console.WriteLine(resultB);
      }

      private static int[][] Parse(string inputPath)
      {
         var lines = System.IO.File.ReadAllLines(inputPath);
         var separator = " ";
         var split_lines = lines.Select(line => line.Split(separator));
         return split_lines.Select(line => line.Select(int.Parse).ToArray()).ToArray();
      }

      private static int SolveA(int[][] reports)
      {
         return reports.Where(report => IsSafe(report)).Count();
      }

      private static int SolveB(int[][] reports)
      {
         return reports.Where(report => IsSafeEnough(report)).Count();
      }

      private static bool IsSafe(int[] report)
      {
         var deltas = report.Zip(report.Skip(1), (a, b) => a - b);

         return deltas.All(delta => delta >= 1 && delta <= 3) || deltas.All(delta => delta <= -1 && delta >= -3);
      }

      private static bool IsSafeEnough(int[] report)
      {
         return Enumerable.Range(0, report.Length)
        .Any(indexToRemove =>
            IsSafe(report.Where((_, index) => index != indexToRemove).ToArray())
        );
      }
   }
}
