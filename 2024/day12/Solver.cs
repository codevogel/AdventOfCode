
namespace AdventOfCode
{
   public class Solver
   {
      static List<Region> regions = new();
      static HashSet<Vector2i> visited = new();

      public static void Main(string[] args)
      {
         string inputPath = "input.txt";

         // Read the input file
         char[][] map = File.ReadAllLines(inputPath).Select(line => line.ToCharArray()).ToArray();

         var coords = from y in Enumerable.Range(0, map.Length)
                      from x in Enumerable.Range(0, map[y].Length)
                      select new Vector2i(x, y);

         foreach (var coord in coords)
         {
            // Skip if already visited
            if (visited.Contains(coord))
               continue;

            var c = map[coord.y][coord.x];

            // Create a new region for this plant type
            var region = new Region(plant: c);
            regions.Add(region);

            // Process this region
            ProcessRegion(map, coord, region);
         }

         // Calculate total price
         int totalPrice = regions.Sum(region => region.Area * region.Perimeter);

         Console.WriteLine(totalPrice);

         int bulkPrice = regions.Sum(region => region.Area * region.CountSides());

         Console.WriteLine(bulkPrice);
      }

      private static void ProcessRegion(char[][] map, Vector2i startCoord, Region region)
      {
         Queue<Vector2i> queue = new();
         queue.Enqueue(startCoord);

         while (queue.Count > 0)
         {
            var coord = queue.Dequeue();

            // Skip if already visited or not matching plant type
            if (visited.Contains(coord) || map[coord.y][coord.x] != region.plant)
               continue;

            // Mark as visited and increment area
            visited.Add(coord);
            region.Coords.Add(coord);

            var offsets = new Vector2i[]
            { new (0, -1), new (1, 0), new (0, 1), new (-1, 0) };

            foreach (var offset in offsets)
            {
               var neighbourCoord = coord + offset;

               // Check if neighbor is out of bounds or a different plant type
               if (!IsInBounds(neighbourCoord, map) ||
                   map[neighbourCoord.y][neighbourCoord.x] != region.plant)
               {
                  region.Perimeter++;
               }
               else if (!visited.Contains(neighbourCoord))
               {
                  queue.Enqueue(neighbourCoord);
               }
            }
         }
      }

      private static bool IsInBounds(Vector2i coord, char[][] map)
      {
         return coord.x >= 0 && coord.x < map[0].Length &&
                coord.y >= 0 && coord.y < map.Length;
      }
   }

   class Region
   {
      public char plant;
      public List<Vector2i> Coords = new();

      public int Area => Coords.Count;
      public int Perimeter;
      public Region(char plant) => this.plant = plant;

      public int CountSides()
      {
         return -1;
      }
   }

   record struct Vector2i(int x, int y)
   {
      public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
   }
}
