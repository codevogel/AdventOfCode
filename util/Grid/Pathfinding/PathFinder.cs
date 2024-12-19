using AdventOfCode.Util.Math;

namespace AdventOfCode.Util.Grid.Pathfinding
{
   /// <summary>
   /// The PathFinder finds a path in a Grid using an IPathFindingStrategy. 
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public class PathFinder<T>
   {
      /// <summary> The path finding strategy. </summary>
      private IPathFindingStrategy<T> pathFindingStrategy;

      /// <summary>
      /// Initialize the PathFinder with a path finding strategy.
      /// <paramref name="pathFindingStrategy"/> The path finding strategy to use.
      /// </summary>
      public PathFinder(IPathFindingStrategy<T> pathFindingStrategy)
      {
         this.pathFindingStrategy = pathFindingStrategy;
      }

      /// <summary>
      /// Find a path in a grid.
      /// <paramref name="grid"/> The grid to find the path in.
      /// <returns>A list of coordinates representing the path.</returns>
      /// </summary>
      public IEnumerable<Vector2i> FindPath(Grid<T> grid)
      {
         return pathFindingStrategy.FindPath(grid);
      }
   }
}
