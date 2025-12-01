public class Solver
{

   private static List<(bool left, int amount)> rotations = new();

   public static void Main(string[] args)
   {
      List<string> result = Parse_Input("input.txt");

      rotations = result
          .Select(line => (left: line[0] == 'L', amount: int.Parse(line[1..])))
          .ToList();

      Console.WriteLine($"A: {SolveA()}");
      Console.WriteLine($"B: {SolveB()}");
   }

   private static int SolveA()
   {
      return CountClicks(false);
   }

   private static int SolveB()
   {
      return CountClicks(true);
   }

   private static int CountClicks(bool countIntermediateZeroes)
   {
      int dial = 50;
      int clicks = 0;
      foreach (var rotation in rotations)
      {
         var amount = rotation.amount;
         for (int i = 0; i < amount; i++)
         {
            dial = (dial + (rotation.left ? -1 : 1) + 100) % 100;
            if (countIntermediateZeroes && dial == 0)
               clicks++;
         }
         if (!countIntermediateZeroes && dial == 0)
            clicks++;
      }
      return clicks;

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
