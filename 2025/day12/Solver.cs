using System.Diagnostics;
using System.Text;

namespace day12
{
   public class Solver
   {
      public static void Main()
      {
         List<string> input = ParseInput("input.txt");
         Stopwatch stopwatch = new();
         stopwatch.Start();

         var shapeInput = input.Take(30);
         var packInput = input.Skip(30);

         List<Shape> shapes = [];

         int shapeIndex = 0;
         while (shapeInput.Any())
         {
            shapes.Add(new Shape(shapeIndex++, shapeInput.Take(5)));

            shapeInput = shapeInput.Skip(5);
         }

         PackInstruction[] packInstructions =
         [
            .. packInput
               .Select(line => line.Split(' '))
               .Select(parts =>
               {
                  var areaParts = string.Join("", parts.First().SkipLast(1)).Split('x');
                  var requiredShapesPart = parts.Skip(1);

                  (int x, int y) size = (x: int.Parse(areaParts[0]), int.Parse(areaParts[1]));
                  int[] requiredShapes = [.. requiredShapesPart.Select(int.Parse)];
                  return new PackInstruction(size, requiredShapes);
               }),
         ];

         var areasThatFit = packInstructions.Where(instruction =>
         {
            int area = instruction.Size.X * instruction.Size.Y;
            Console.WriteLine("Area: " + area);
            int[] shapeAreas =
            [
               .. instruction.RequiredShapes.Keys.Select(i =>
                  shapes[i].Area * instruction.RequiredShapes[i]
               ),
            ];
            shapeAreas.ToList().ForEach(Console.WriteLine);
            return shapeAreas.Sum() < area;
         });

         Console.WriteLine(areasThatFit.Count());

         stopwatch.Stop();
         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");
      }

      public class Shape(int index, IEnumerable<string> input)
      {
         public int Index { get; } = index;

         public int Area => contents.SelectMany(row => row).Count(b => b);

         public readonly bool[][] contents =
         [
            .. input
               .Skip(1)
               .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
               .Select(line => line.Select(c => c == '#').ToArray()),
         ];

         public override string ToString()
         {
            var sb = new StringBuilder();
            for (int y = 0; y < contents.Length; y++)
            {
               for (int x = 0; x < contents[0].Length; x++)
               {
                  sb.Append(contents[y][x] ? '#' : '.');
               }
               sb.Append('\n');
            }
            return sb.ToString();
         }
      }

      public class PackInstruction((int X, int Y) size, int[] requiredShapes)
      {
         public (int X, int Y) Size { get; } = size;
         public Dictionary<int, int> RequiredShapes { get; } =
            requiredShapes
               .Select((n, i) => (n, i))
               .ToDictionary(nAndIndex => nAndIndex.i, nAndIndex => nAndIndex.n);
      }

      static List<string> ParseInput(string path) => [.. File.ReadLines(path)];
   }
}
