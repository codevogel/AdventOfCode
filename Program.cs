namespace AdventOfCode
{
   public class Program
   {

      public static void Main(string[] args)
      {
         if (args.Length != 2)
         {
            Console.WriteLine("Usage: dotnet run <year> <day>");
            return;
         }

         var year = int.Parse(args[0]);
         var day = int.Parse(args[1]);

         Console.WriteLine($"Advent of Code {year} - Day {day}");

      }
   }

}
