using System.Diagnostics;
using System.Numerics;
using System.Text;
using Microsoft.Z3;

namespace day10
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");

         var splitLines = input.Select(line => line.Split(' '));
         // Parse heaven
         var machines = splitLines.Select(parts => new Machine(
            diagram: string.Join(
               "",
               parts.First().Trim('[', ']').Select(c => c == '#' ? '1' : '0')
            ),
            wirings:
            [
               .. parts
                  .Skip(1)
                  .SkipLast(1)
                  .Select(s =>
                     s.Trim('(', ')')
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray()
                  ),
            ],
            joltage:
            [
               .. parts
                  .Last()
                  .Trim('{', '}')
                  .Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(int.Parse),
            ]
         ));

         Stopwatch stopwatch = new();
         stopwatch.Start();

         long totalFewestPresses = 0;
         int machineCount = 0;

         machines
            .ToList()
            .ForEach(machine =>
            {
               machineCount++;
               int fewestPresses = machine.SolveFewestPresses();
               totalFewestPresses += fewestPresses;
            });

         Console.WriteLine($"A: {totalFewestPresses}");

         long totalFewestPressesB = 0;
         machineCount = 0;

         machines
            .ToList()
            .ForEach(machine =>
            {
               machineCount++;
               int fewest = machine.SolveFewestPressesPartB();
               totalFewestPressesB += fewest;
            });

         Console.WriteLine($"B: {totalFewestPressesB}");

         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");
      }

      public class Machine
      {
         private string DiagramString { get; }
         private int[][] Wirings { get; }
         private int[] Joltage { get; }
         private string TargetState { get; }
         private string[] ButtonEffects { get; }

         public Machine(string diagram, int[][] wirings, int[] joltage)
         {
            DiagramString = diagram;
            Wirings = wirings;
            Joltage = joltage;

            TargetState = diagram;

            // We define each effect as a string of 1's and 0's that correspond
            // to the changes they will make in the lights
            ButtonEffects = new string[Wirings.Length];
            for (int i = 0; i < Wirings.Length; i++)
            {
               char[] effectChars = new char[diagram.Length];
               Array.Fill(effectChars, '0');

               foreach (int lightIndex in Wirings[i])
               {
                  effectChars[lightIndex] = '1';
               }
               ButtonEffects[i] = new string(effectChars);
            }
         }

         // This is a dumb way to do it as we can just XOR numbers,
         // but it ensures we don't have to deal with reversing the string
         // so the indexes align with the lights
         private static string Xor(string state, string effect)
         {
            char[] result = state.ToCharArray();
            for (int i = 0; i < result.Length; i++)
            {
               if (effect[i] == '1')
                  result[i] = result[i] == '0' ? '1' : '0';
            }
            return new string(result);
         }

         public int SolveFewestPresses()
         {
            // Initial state is all lights off
            string InitialState = new('0', DiagramString.Length);

            if (TargetState == InitialState)
               return 0;

            HashSet<string> visitedStates = [InitialState];
            Queue<string> currentGeneration = new();

            // In the first gen we just initialize the state
            // by XOR'ing the effect with the lights that are turned off
            // For each of the button presses this will generate a new
            // potential state
            int presses = 1;
            foreach (string effect in ButtonEffects)
            {
               string newState = Xor(InitialState, effect);

               // We check if any of them satisfy the target state
               // (aka the [.##.] thing)
               if (newState == TargetState)
                  return presses;

               // Else we add these as a new generation of states
               if (visitedStates.Add(newState))
               {
                  currentGeneration.Enqueue(newState);
               }
            }

            // Just keep doing this until we find the solution
            // We can be sure we won't get stuck in an inf loop here
            // as the puzzle is solveable.
            while (currentGeneration.Count > 0)
            {
               presses++;
               HashSet<string> nextGenerationSet = [];

               while (currentGeneration.Count > 0)
               {
                  string currentState = currentGeneration.Dequeue();

                  foreach (string effect in ButtonEffects)
                  {
                     string newState = Xor(currentState, effect);

                     if (newState == TargetState)
                        return presses;

                     if (visitedStates.Add(newState))
                     {
                        nextGenerationSet.Add(newState);
                     }
                  }
               }

               foreach (string state in nextGenerationSet)
               {
                  currentGeneration.Enqueue(state);
               }
            }
            // Won't reach this with the puzzle input.
            return -1;
         }

         // Part B is (after long consideration and a few hints)
         // an equation in the form of A*(effect)+B*effect) ... = (required joltage)
         // where each effect is a Vector containing all the effects (similar to part A)
         // and the required joltage is a vector too.
         // Then we should find the minimum sum of A, B ... etc to result the required joltage
         //
         // I used Z3 to solve it, which was a new library for me
         // It's documentation can be found here:
         // https://github.com/Z3Prover/z3/tree/master/examples/dotnet
         // Though the c# documentation is pretty awful, so I'd recommend looking at the main site
         // and the python examples instead
         public int SolveFewestPressesPartB()
         {
            Context context = new();

            // Create an optimization context as we need to find the minimal
            Optimize opt = context.MkOptimize();

            // Create variables for each of the wirings
            var presses = Wirings.Select((wiring, i) => context.MkIntConst($"x{i}")).ToArray();

            // For each of the press variables we say that they must be >= 0
            presses
               .Select(presses => context.MkGe(presses, context.MkInt(0)))
               .ToList()
               .ForEach(expr => opt.Add(expr));

            // For each of the counters, we create the equation as described above
            Enumerable
               .Range(0, Joltage.Length) // loop over all counters
               .Select(counter =>
                  context.MkEq(
                     // Build the SUM on the left side of the equation:
                     Wirings
                        .Select(
                           (wiring, buttonIndex) =>
                              // Here we basically build the vectors as per the equation
                              wiring.Contains(counter)
                                 ? (ArithExpr)presses[buttonIndex]
                                 : context.MkInt(0)
                        )
                        // Add each of the vars to the expression using + signs in between
                        .Aggregate((ArithExpr)context.MkInt(0), (a, b) => context.MkAdd(a, b)),
                     // Target joltage
                     context.MkInt(Joltage[counter])
                  )
               )
               .ToList()
               .ForEach(expr => opt.Add(expr));

            // Now we need to tell z3 that we want to solve for the minimum presses
            var totalPresses = presses.Aggregate(
               (ArithExpr)context.MkInt(0),
               (a, b) => context.MkAdd(a, b)
            );
            opt.MkMinimize(totalPresses);

            // We ask z3 to solve the equations
            if (opt.Check() != Status.SATISFIABLE)
               // This shouldn't happen because we can find an answer for all the equations
               // because its a puzzle
               // well. hopefully
               throw new Exception("No solution found");

            // Return the result
            return ((IntNum)opt.Model.Evaluate(totalPresses)).Int;
         }
      }

      static List<string> ParseInput(string path) => [.. File.ReadLines(path)];
   }
}
