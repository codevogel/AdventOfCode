using AdventOfCode.Util.Math;

namespace AdventOfCode.Util.Grid
{
   /// <summary>
   /// Grid class for representing a 2D grid of type <typeparamref name="T"/>.
   /// <typeparamref name="T"/> The type of the grid.
   /// </summary>
   public class Grid<T>
   {
      /// <summary> The content of the grid </summary>
      private T[][] _content;

      /// <summary> 
      /// The size of the grid.
      /// <remarks>Assumes that the grid is rectangular.</remarks>
      /// <returns>The size of the grid.</returns>
      /// </summary>
      public Vector2i Size =>
         new Vector2i(_content.Length, _content[0].Length);

      /// <summary>
      /// Initialize grid with data.
      /// <paramref name="data"/> The data of the grid.
      /// </summary>
      public Grid(T[][] data)
      {
         _content = data;
      }

      /// <summary>
      /// Initialize grid with size and default value. 
      /// <paramref name="size"/> The size of the grid.
      /// <paramref name="defaultValue"/> The default value of the grid.
      /// </summary>
      public Grid(Vector2i size, T defaultValue)
      {
         _content = Enumerable.Range(0, size.y).Select(_ => Enumerable.Repeat(defaultValue, size.x).ToArray()).ToArray();
      }

      /// <summary>
      /// Get or set the value at a specific position.
      /// <throws>Throws <see cref="IndexOutOfRangeException"/> if the position is out of bounds.</throws>
      /// </summary>
      public T this[Vector2i pos]
      {
         get
         {
            if (IsInBounds(pos))
               return _content[pos.y][pos.x];
            throw new IndexOutOfRangeException();
         }
         set
         {
            if (IsInBounds(pos))
               _content[pos.y][pos.x] = value;
            throw new IndexOutOfRangeException();
         }
      }

      /// <summary>
      /// Get all coordinates in the grid.
      /// <returns>A list of all coordinates in the grid.</returns>
      /// </summary>
      public List<Vector2i> Coords =>
         Enumerable.Range(0, Size.y)
            .SelectMany(y => Enumerable.Range(0, Size.x).Select(x => new Vector2i(x, y)))
            .ToList();

      /// <summary>
      /// Check if a position is in bounds.
      /// <paramref name="pos"/> The position to check.
      /// <returns>True if the position is in bounds, otherwise false.</returns>
      /// </summary>
      public bool IsInBounds(Vector2i pos)
      {
         return pos.x >= 0 && pos.x < Size.x && pos.y >= 0 && pos.y < Size.y;
      }

      /// <summary>
      /// Get the coordinates of the neighbours of a position.
      /// <paramref name="vertex"/> The position to get the neighbours of.
      /// <returns>A list of the coordinates of the neighbours of the position.</returns>
      /// </summary>
      public List<Vector2i> GetInBoundsNeighbourCoords(Vector2i vertex)
      {
         var offsets = new Vector2i[] {
            new (0, 1), new (1, 0), new (0, -1), new (-1, 0)
         };
         return offsets.Select(offset => vertex + offset)
            .Where(IsInBounds)
            .ToList();
      }

      /// <summary>
      /// Get the neighbours of a position.
      /// <paramref name="vertex"/> The position to get the neighbours of.
      /// <returns>A list of the neighbours of the position.</returns>
      /// </summary>
      public List<T> GetNeighbours(Vector2i vertex)
      {
         return GetInBoundsNeighbourCoords(vertex)
            .Select(pos => this[pos])
            .ToList();
      }

      /// <summary>
      /// Convert the grid to a string.
      /// <paramref name="cellToString"/> A function to convert a cell of type <typeparamref name="T"/> to a string. 
      /// <returns>A string representation of the grid.</returns>
      /// </summary>
      public string ToString(Func<T, string> cellToString)
      {
         return string.Join("\n", _content.Select(row => string.Join("", row.Select(cellToString))));
      }
   }

}
