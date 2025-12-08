using System.Diagnostics;
using System.Numerics;

namespace day8
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");

         Stopwatch stopwatch = new();
         stopwatch.Start();

         // Parse vectors
         List<Vector3> junctionBoxes =
         [
            .. input
               .Select(line => line.Split(',').Select(int.Parse).ToArray())
               .Select(parts => new Vector3(parts[0], parts[1], parts[2])),
         ];

         // Get closest pairs
         List<(Vector3 A, Vector3 B, double Delta)> closestBoxes = FindClosestBoxes(junctionBoxes);

         // For the first 1000 closest pairs, find the circuits
         // In this case, the circuits won't be closed
         List<HashSet<Vector3>> circuits = FindCircuits(
            [.. closestBoxes.TakeLast(1000)], // Since we ordered by descending order, take the last rather than the first
            junctionBoxes
         ).circuits;

         // Sum the three largest found circuits
         BigInteger sumA = circuits
            .Select(circuit => circuit.Count)
            .OrderDescending()
            .Take(3)
            .Aggregate(BigInteger.One, (curr, total) => curr * total);
         Console.WriteLine($"A: {sumA}");

         // Find the first pair that closes the circuit
         var (A, B, Delta) = FindCircuits([.. closestBoxes], junctionBoxes).lastPair;
         // Sum the X coordinates of that pair
         BigInteger sumB = (BigInteger)A.X * (BigInteger)B.X;
         Console.WriteLine($"B: {sumB}");

         stopwatch.Stop();

         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");
      }

      private static (
         List<HashSet<Vector3>> circuits,
         (Vector3 A, Vector3 B, double Delta) lastPair
      ) FindCircuits(
         List<(Vector3 A, Vector3 B, double Delta)> closestBoxPairs,
         List<Vector3> junctionBoxes
      )
      {
         Stack<(Vector3 A, Vector3 B, double Delta)> closestPairsStack = [];
         closestBoxPairs.ForEach(closestPairsStack.Push);

         // Initialize the state by adding all junctionBoxes into a separate hashset
         List<HashSet<Vector3>> circuits =
         [
            .. junctionBoxes.Select(box =>
            {
               HashSet<Vector3> set = [box];
               return set;
            }),
         ];

         // Store the last pair for part B
         (Vector3 A, Vector3 B, double Delta) lastPair = (Vector3.Zero, Vector3.Zero, -1);
         // While there are still pairs
         while (closestPairsStack.Count > 0)
         {
            // Consider the one at the top of the stack
            lastPair = closestPairsStack.Pop();
            // Find the circuit A and B belongs to
            // (because we initialize them all to a stack, we will always find a result here)
            var indexOfSetA = circuits.FindIndex(set => set.Contains(lastPair.A));
            var indexOfSetB = circuits.FindIndex(set => set.Contains(lastPair.B));

            // If they belong to the same hashset, they are in the same circuit
            if (indexOfSetA == indexOfSetB)
               // So we do nothing
               continue;

            // Else, we make a union of the circuits
            var setA = circuits[indexOfSetA];
            var setB = circuits[indexOfSetB];
            var union = setA.Union(setB).ToHashSet();
            // Then replace the separate circuits by the new union
            circuits.Remove(setA);
            circuits.Remove(setB);
            circuits.Add(union);
            // When the resulting union is the only circuit left, we have a closed circuit
            if (circuits.Count == 1)
               // We only reach this in part B, so it's safe to break here
               break;
         }
         // Finally, return the resulting circuits and the last pair
         return (circuits, lastPair);
      }

      private static List<(Vector3 A, Vector3 B, double Delta)> FindClosestBoxes(
         List<Vector3> junctionBoxes
      )
      {
         return
         [
            .. junctionBoxes
               .SelectMany(
                  (a, i) => junctionBoxes.Skip(i + 1),
                  (a, b) =>
                     (
                        A: a,
                        B: b,
                        // We can skip the sqrt here as it has no significance
                        // on the resulting distance order
                        Delta: Math.Pow(a.X - b.X, 2)
                           + Math.Pow(a.Y - b.Y, 2)
                           + Math.Pow(a.Z - b.Z, 2)
                     )
               )
               // Order by descending as we will push these to a stack
               .OrderByDescending(pair => pair.Delta),
         ];
      }

      static List<string> ParseInput(string path) => [.. File.ReadLines(path)];
   }
}
