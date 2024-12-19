using AdventOfCode.Util.Math;

namespace AdventOfCode.Util.Grid.Pathfinding
{
   /// <summary>
   /// The IPathFindingStrategy interface defines a path finding strategy for finding a path in a grid.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public interface IPathFindingStrategy<T>
   {

      /// <summary>
      /// Find a path in a grid.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid);
   }
}
