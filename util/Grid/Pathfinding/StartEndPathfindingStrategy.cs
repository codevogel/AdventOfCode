using AdventOfCode.Util.Math;

namespace AdventOfCode.Util.Grid.Pathfinding
{
   /// <summary>
   /// The StartEndPathFindingStrategy class is a base class for 
   /// path finding strategies that find a path between two points.
   /// <typeparamref name="T"/> The type of the grid contents.
   /// </summary>
   public abstract class StartEndPathFindingStrategy<T> : IStartEndPathFindingStrategy<T>
   {
      /// <summary> The start position. </summary>
      public Vector2i Start { get; set; }
      /// <summary> The end position. </summary>
      public Vector2i End { get; set; }

      public abstract IEnumerable<Vector2i> FindPath(Grid<T> grid, Vector2i start, Vector2i end);

      public IEnumerable<Vector2i> FindPath(Grid<T> grid)
      {
         return FindPath(grid, Start, End);
      }
   }
}
