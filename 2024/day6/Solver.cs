namespace AdventOfCode
{
   public class Solver
   {

      static void Main(string[] args)
      {
         string inputPath = "input.txt";
         var guardStart = (-1, -1);
         // Parse the map. Replace the guard with an empty cell, and store its position.
         var map = File.ReadAllLines(inputPath).Select((line, y) => line.ToCharArray().Select((c, x) =>
         {
            if (c == '^')
            {
               guardStart = (x, y);
               return '.';
            }
            return c;
         }).ToArray()).ToArray();

         // Part A: Simulate the route and count the number of unique cells visited
         var route = SimulateRoute(map.ToArray(), guardStart).resultingRoute;
         Console.WriteLine(route.Count());

         // Part B: For each position on the route, simulate the route but now put a wall on each position. 
         // If the resulting route is infinite, note it down.
         int numInfs = 0;
         foreach (var pos in route.Distinct())
         {
            if (pos == guardStart) { continue; }
            if (map[pos.y][pos.x] == '#') { continue; }
            map[pos.y][pos.x] = '#';
            if (SimulateRoute(map, guardStart).infinite)
            {
               numInfs++;
            }
            map[pos.y][pos.x] = '.';
         }
         Console.WriteLine(numInfs);
      }

      static (List<(int x, int y)> resultingRoute, bool infinite) SimulateRoute(char[][] map, (int x, int y) start)
      {
         var route = new List<(int x, int y)>();
         var visited = new HashSet<((int x, int y) pos, (int x, int y) dir)>();
         var current_pos = start;
         var current_dir = (x: 0, y: -1);

         while (IsInBounds(map, current_pos))
         {
            // We are entering an infinite loop if we have already visited current position in current direction
            var state = ((current_pos.x, current_pos.y), (current_dir.x, current_dir.y));
            if (visited.Contains(state))
            {
               return (route.Distinct().ToList(), true);
            }

            // Store current position in route and mark it as visited
            route.Add(current_pos);
            visited.Add(state);

            // Try to walk
            var next_pos = (x: current_pos.x + current_dir.x, y: current_pos.y + current_dir.y);
            // If we hit a wall, turn right
            if (IsInBounds(map, next_pos) && map[next_pos.y][next_pos.x] == '#')
            {
               current_dir = TurnRight(current_dir);
               continue;
            }
            // If we can walk, do it
            current_pos = next_pos;
         }
         return (route.Distinct().ToList(), false);
      }

      static (int x, int y) TurnRight((int x, int y) dir)
      {
         switch (dir)
         {
            case (0, -1): return (1, 0);
            case (1, 0): return (0, 1);
            case (0, 1): return (-1, 0);
            case (-1, 0): return (0, -1);
         }
         throw new Exception("Invalid direction");
      }

      static bool IsInBounds(char[][] map, (int x, int y) pos)
      {
         return pos.x >= 0 && pos.x < map[0].Length && pos.y >= 0 && pos.y < map.Length;
      }
   }
}
