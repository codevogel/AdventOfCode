namespace AdventOfCode
{
   public class Solver
   {
      static void Main(string[] args)
      {
         var inputPath = "input.txt";

         // Read the input file and parse the engravings
         var engravings = System.IO.File.ReadAllLines(inputPath)[0].Split(' ').Select(long.Parse).ToList();

         // Keep track of the number of stones in each pile.
         // Key: Engraving, Value: Number of stones
         Dictionary<long, long> stonePile = new Dictionary<long, long>();

         // For each engraving, increment the stone pile by 1
         foreach (var engraving in engravings)
         {
            IncrementOrAdd(stonePile, engraving, 1);
         }

         // Blink 75 times
         for (int i = 0; i < 75; i++)
         {
            // Create a new stone pile
            var newStonePile = new Dictionary<long, long>();

            // For each group in the current pile
            foreach (var stone in stonePile)
            {
               var engraving = stone.Key;
               var engravingStr = engraving.ToString();
               var num_stones = stone.Value;

               // If the engraving is 0, the new pile will have num_stones stones with engraving 1
               if (engraving == 0)
                  IncrementOrAdd(newStonePile, 1, num_stones);
               else if (engravingStr.Length % 2 == 0)
               {
                  // If the engraving has an even number of digits, split it in half and add the two halves to the new pile
                  // Each half will have num_stones stones
                  var leftHalf = engravingStr.Substring(0, engravingStr.Length / 2);
                  var rightHalf = engravingStr.Substring(engravingStr.Length / 2);
                  IncrementOrAdd(newStonePile, long.Parse(leftHalf), num_stones);
                  IncrementOrAdd(newStonePile, long.Parse(rightHalf), num_stones);
               }
               else
                  // Other rules don't apply, so multiply the engraving by 2024
                  IncrementOrAdd(newStonePile, engraving * 2024, num_stones);
            }
            // Replace the current pile with the new pile
            stonePile = newStonePile;
         }
         Console.WriteLine(stonePile.Values.Sum());
      }

      private static void IncrementOrAdd(Dictionary<long, long> stonePile, long engraving, long num_stones)
      {
         if (stonePile.ContainsKey(engraving))
            stonePile[engraving] += num_stones;
         else
            stonePile[engraving] = num_stones;
      }
   }
}
