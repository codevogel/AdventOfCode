namespace AdventOfCode.Util.Math
{
   /// <summary>
   /// The Vector2i struct represents a 2D vector with integer components.
   /// </summary>
   public record struct Vector2i(int x, int y)
   {
      public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
      public override string ToString() => $"({x}, {y})";
   }
}
