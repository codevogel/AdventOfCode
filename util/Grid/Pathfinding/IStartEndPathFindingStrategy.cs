using AdventOfCode.Util.Math;

namespace AdventOfCode.Util.Grid.Pathfinding
{
   /// <summary>
   /// The IStartEndPathFindingStrategy interface defines a path finding strategy for finding a path in a grid between two points.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public interface IStartEndPathFindingStrategy<T> : IPathFindingStrategy<T>
   {
      /// <summary>
      /// Find a path in a grid between two points.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <paramref name="start"/> The start position.
      /// <paramref name="end"/> The end position.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid, Vector2i start, Vector2i end);
   }
}
