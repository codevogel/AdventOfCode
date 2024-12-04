namespace AdventOfCode
{
   public class Solver
   {

      public static char[][] grid = new char[0][];

      private record struct Vector2I(int X, int Y)
      {
         public static Vector2I operator +(Vector2I a, Vector2I b) => new(a.X + b.X, a.Y + b.Y);
         public static Vector2I operator *(int scalar, Vector2I vector) => new(scalar * vector.X, scalar * vector.Y);
      }

      static void Main(string[] args)
      {
         string inputPath = "input.txt";
         grid = ParseInput(inputPath);

         Console.WriteLine(CrossWordSearch("XMAS"));
         Console.WriteLine(CrossMasSearch());
      }

      private static char[][] ParseInput(string inputPath)
      {
         var lines = File.ReadAllLines(inputPath);
         return lines.Select(line => line.ToCharArray()).ToArray();
      }

      private static int CrossWordSearch(string word)
         => grid.SelectMany((row, y) => row.Select((_, x) => OccurrencesAtPosition(word, new Vector2I(x, y)))).Sum();

      private static int OccurrencesAtPosition(string word, Vector2I startPosition)
      {
         List<Vector2I> searchDirections = new() { new(1, 0), new(-1, 0), new(0, 1), new(0, -1), new(1, 1), new(-1, -1), new(1, -1), new(-1, 1) };
         return searchDirections.Select(searchDirection => SearchForWord(word, startPosition, searchDirection) ? 1 : 0).Sum();
      }

      private static bool SearchForWord(string word, Vector2I startPosition, Vector2I direction)
         => word.Select((target_char, index) => (pos: startPosition + index * direction, target_char: target_char))
            .All(query => InBounds(grid, query.pos) && PositionHasChar(query.pos, query.target_char));

      private static bool PositionHasChar(Vector2I position, char targetChar) =>
         InBounds(grid, position) && grid[position.Y][position.X] == targetChar;

      private static int CrossMasSearch()
         => grid.SelectMany((row, y) => row.Select((c, x) => IsSAMorMASCrossing(new Vector2I(x, y)) ? 1 : 0)).Sum();

      private static bool IsSAMorMASCrossing(Vector2I pos)
         => PositionHasChar(pos, 'A')
            && IsSMOrMS(pos + new Vector2I(-1, -1), pos + new Vector2I(1, 1))
            && IsSMOrMS(pos + new Vector2I(-1, 1), pos + new Vector2I(1, -1));

      private static bool IsSMOrMS(Vector2I posA, Vector2I posB)
         => PositionHasChar(posA, 'S') && PositionHasChar(posB, 'M')
         || PositionHasChar(posA, 'M') && PositionHasChar(posB, 'S');

      private static bool InBounds(char[][] grid, Vector2I pos)
         => pos.Y >= 0 && pos.Y < grid.Length && pos.X >= 0 && pos.X < grid[pos.Y].Length;
   }
}

