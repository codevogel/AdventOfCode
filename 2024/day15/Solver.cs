namespace AdventOfCode
{

   public class Solver
   {

      public static void Main(string[] args)
      {
         var inputPath = "input.txt";
         var lines = File.ReadAllLines(inputPath);

         // Parse the input
         var mapLines = lines.TakeWhile(line => !string.IsNullOrEmpty(line)).Select(line => line.ToCharArray()).ToArray();
         var moves = lines.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1).SelectMany(line => line).Select(
               move => move switch
               {
                  '^' => Move.Up,
                  'v' => Move.Down,
                  '<' => Move.Left,
                  '>' => Move.Right,
                  _ => throw new Exception("Invalid move")
               }).ToArray();

         // For part 1, we can use the map as is
         var smallMap = new Map(mapLines);
         foreach (var move in moves)
         {
            smallMap.MoveRobot(move);
         }
         Console.WriteLine("Part 1:");
         Console.WriteLine(smallMap.SumOfGPSCoordinates);

         // For part 2, we need to expand the map to account for wide objects
         var largeMapLines = mapLines.Select(line =>
         {
            var expandedLine = "";
            foreach (var c in line)
            {
               expandedLine += (c switch
               {
                  '#' => "##",
                  'O' => "[]",
                  '.' => "..",
                  '@' => "@.",
                  _ => throw new Exception("Invalid character")
               });
            }
            return expandedLine.ToCharArray();
         }).ToArray();

         // Create a new map with the large objects
         var largeMap = new Map(largeMapLines, largeObjects: true);
         foreach (var move in moves)
         {
            largeMap.MoveRobot(move);
         }

         Console.WriteLine("Part 2:");
         Console.WriteLine(largeMap.SumOfGPSCoordinates);
      }

      public enum Move { Up, Down, Left, Right }

      public class Map
      {
         public bool largeObjects = false;
         public Vector2i robotPosition;
         public char[][] content;

         public Map(char[][] content, bool largeObjects = false)
         {
            this.content = content.Select(line => line.ToArray()).ToArray();
            this.robotPosition = Coords.FirstOrDefault(coord => CharAt(coord) == '@');
            this.largeObjects = largeObjects;
         }

         public Vector2i[] Coords =>
            Enumerable.Range(0, content.Length).SelectMany(y =>
               Enumerable.Range(0, content[y].Length).Select(x => new Vector2i(x, y))).ToArray();


         public long SumOfGPSCoordinates =>
                     largeObjects ? GetSumOfGPSCoordinates('[') : GetSumOfGPSCoordinates('O');

         private long GetSumOfGPSCoordinates(char target)
         {
            return Coords.Where(coord => CharAt(coord) == target)
               .Select(coord => 100 * coord.y + coord.x).Sum();
         }

         public char CharAt(Vector2i position)
         {
            return content[position.y][position.x];
         }

         public void MoveRobot(Move move)
         {
            var direction = move switch
            {
               Move.Up => new Vector2i(0, -1),
               Move.Down => new Vector2i(0, 1),
               Move.Left => new Vector2i(-1, 0),
               Move.Right => new Vector2i(1, 0),
               _ => throw new Exception("Invalid move")
            };

            if (largeObjects)
               MoveRobotLargeObjects(direction);
            else
               MoveRobotSmallObjects(direction);
         }

         // Grab all potential box locations 
         // (i.e. all locations in the direction of the move until we hit a wall)
         public List<Vector2i> GetPotentialBoxLocations(Vector2i direction)
         {
            var potentialBoxLocations = new List<Vector2i>();

            var seekerPos = robotPosition + direction;
            while (CharAt(seekerPos) != '#')
            {
               potentialBoxLocations.Add(seekerPos);
               seekerPos += direction;
            }

            return potentialBoxLocations;
         }

         private void MoveRobotSmallObjects(Vector2i direction)
         {
            var potentialBoxLocations = GetPotentialBoxLocations(direction);

            // If there are no potential box locations, we can't move the robot
            if (potentialBoxLocations.Count() == 0) return;

            // If all potential box locations are boxes, we can't move the robot
            if (potentialBoxLocations.All(coord => CharAt(coord) == 'O')) return;

            // Grab all boxes that need to be moved
            // (i.e. all boxes from the robot's current position to the first non-box location)
            var boxesToMove = potentialBoxLocations.TakeWhile(coord => CharAt(coord) == 'O').ToArray();

            // Move the boxes
            for (int i = 0; i < boxesToMove.Length; i++)
            {
               var box = boxesToMove[i];
               if (i == 0)
               {
                  content[box.y][box.x] = '.';
               }
               content[box.y + direction.y][box.x + direction.x] = 'O';
            }

            // Move the robot
            content[robotPosition.y][robotPosition.x] = '.';
            var newRobotPosition = robotPosition + direction;
            content[newRobotPosition.y][newRobotPosition.x] = '@';
            robotPosition = newRobotPosition;
         }

         // Moves the robot in the direction specified
         private void MoveRobotLargeObjects(Vector2i direction)
         {
            // Move the robot in either the horizontal or vertical direction
            if (Math.Abs(direction.x) == 1)
               MoveRobotLargeObjectsHorizontal(direction);
            else
               MoveRobotLargeObjectsVertical(direction);
         }

         // Move the robot in the horizontal direction
         private void MoveRobotLargeObjectsHorizontal(Vector2i direction)
         {
            // Grab all potential box locations
            // (i.e. all locations in the direction of the move until we hit a wall)
            var potentialBoxLocations = GetPotentialBoxLocations(direction);

            // If there are no potential box locations, we can't move the robot
            if (potentialBoxLocations.Count() == 0) return;

            // If all potential box locations are boxes, we can't move the robot
            if (potentialBoxLocations.All(coord => CharAt(coord) == '[' || CharAt(coord) == ']')) return;

            // Grab all boxes that need to be moved
            // (i.e. all boxes from the robot's current position to the first non-box location)
            var boxesToMove = potentialBoxLocations.TakeWhile(coord => CharAt(coord) == '[' || CharAt(coord) == ']').ToArray();

            // Move the boxes
            char prevCharA = '!'; // Dummy initialization value, we don't care about the first value
            for (int i = 0; i < boxesToMove.Length; i += 2)
            {
               // Grab the two halves of the box
               var coordHalfA = boxesToMove[i];
               var coordHalfB = boxesToMove[i + 1];

               // Grab the characters at the two halves of the box
               var charHalfA = i == 0 ? CharAt(coordHalfA) : prevCharA;
               var charHalfB = CharAt(coordHalfB);

               // If this is the first box, we need to clear the location of the first half
               if (i == 0)
               {
                  content[coordHalfA.y][coordHalfA.x] = '.';
               }
               content[coordHalfB.y][coordHalfB.x] = charHalfA;
               // Store the character of the first half for the next iteration to prevent overwriting it
               prevCharA = CharAt(coordHalfA + direction);
               content[coordHalfB.y][coordHalfB.x + direction.x] = charHalfB;
            }

            // Move the robot
            content[robotPosition.y][robotPosition.x] = '.';
            var newRobotPosition = robotPosition + direction;
            content[newRobotPosition.y][newRobotPosition.x] = '@';
            robotPosition = newRobotPosition;
         }

         // Move the robot in the vertical direction
         private void MoveRobotLargeObjectsVertical(Vector2i direction)
         {
            // Grab all potential box locations
            // (i.e. all locations in the direction of the move until we hit a wall)
            var potentialBoxLocations = GetPotentialBoxLocations(direction);
            // If there are no potential box locations, we can't move the robot
            if (potentialBoxLocations.Count() == 0) return;

            // If all potential box locations are boxes, we can't move the robot
            if (potentialBoxLocations.All(coord => CharAt(coord) == '[' || CharAt(coord) == ']')) return;

            // Find all boxes that need to be moved
            var (boxesToMove, canMove) = FindBoxesToMoveVertical(direction);

            // If one of the boxes is stuck, we can't move the robot
            if (!canMove) return;

            // Move the boxes
            MoveBoxesInVerticalDirection(boxesToMove, direction);

            // Move the robot
            content[robotPosition.y][robotPosition.x] = '.';
            var newRobotPosition = robotPosition + direction;
            content[newRobotPosition.y][newRobotPosition.x] = '@';
            robotPosition = newRobotPosition;
         }

         private void MoveBoxesInVerticalDirection(List<(Vector2i left, Vector2i right)> boxesToMove, Vector2i direction)
         {
            // Store the characters so we can move the boxes without overwriting them
            var oldLocationCharacterMap = new Dictionary<Vector2i, char>();
            foreach (var box in boxesToMove)
            {
               oldLocationCharacterMap[box.left] = CharAt(box.left);
               oldLocationCharacterMap[box.right] = CharAt(box.right);
            }

            // Move each box
            var newCoords = new HashSet<Vector2i>();
            foreach (var box in boxesToMove)
            {
               // Calculate the new coordinates for this box
               var newCoordLeft = box.left + direction;
               var newCoordRight = box.right + direction;

               // Clear the location of the box if there wasn't a character moved here from the row below it
               if (!newCoords.Contains(box.left))
                  content[box.left.y][box.left.x] = '.';
               if (!newCoords.Contains(box.right))
                  content[box.right.y][box.right.x] = '.';

               // Move the box to the new position
               content[newCoordLeft.y][newCoordLeft.x] = oldLocationCharacterMap[box.left];
               content[newCoordRight.y][newCoordRight.x] = oldLocationCharacterMap[box.right];
               newCoords.Add(newCoordLeft);
               newCoords.Add(newCoordRight);
            }
         }

         // Finds all boxes that need to be moved in the direction specified
         private (List<(Vector2i left, Vector2i right)> boxesToMove, bool canMove) FindBoxesToMoveVertical(Vector2i direction)
         {
            // Find the first character directly in front of the robot
            var seekerPos = robotPosition + direction;
            var nextCharInDirection = CharAt(seekerPos);

            // Add the box that the character belongs to to the list of boxes we need to consider
            var currentBoxRow = new List<(Vector2i left, Vector2i right)>();
            if (nextCharInDirection == '[')
            {
               currentBoxRow.Add((left: seekerPos, right: seekerPos + new Vector2i(1, 0)));
            }
            else if (nextCharInDirection == ']')
            {
               currentBoxRow.Add((left: seekerPos + new Vector2i(-1, 0), right: seekerPos));
            }

            var boxesConsidered = new List<(Vector2i left, Vector2i right)>();
            boxesConsidered.AddRange(currentBoxRow);

            // While there are boxes in the row we are currently considering, 
            // and any of those boxes have a box in the row in front of them, 
            while (currentBoxRow.Count() > 0 && currentBoxRow.Any(box =>
            {
               var nextPosLeft = box.left + direction;
               var nextPosRight = box.right + direction;
               return CharAt(nextPosLeft) == '[' || CharAt(nextPosLeft) == ']'
               || CharAt(nextPosRight) == '[' || CharAt(nextPosRight) == ']';
            }))
            {
               // Find the boxes in the row in front of the current row
               var nextBoxRow = new List<(Vector2i left, Vector2i right)>();

               foreach (var box in currentBoxRow)
               {
                  var nextPosLeft = box.left + direction;
                  var nextPosRight = box.right + direction;

                  // If the left position of the current box has a left half of a box in front of it, 
                  // add that box to the list and continue to the next box
                  if (CharAt(nextPosLeft) == '[')
                  {
                     nextBoxRow.Add((left: nextPosLeft, right: nextPosRight));
                     continue;
                  }
                  // If the left position of the current box has a right half of a box in front of it,
                  // add that box to the list and continue checking the right half of the current box
                  else if (CharAt(nextPosLeft) == ']')
                  {
                     nextBoxRow.Add((left: nextPosLeft + new Vector2i(-1, 0), right: nextPosLeft));
                  }

                  // If the right position of the current box has a right half of a box in front of it,
                  // add that box to the list
                  if (CharAt(nextPosRight) == '[')
                  {
                     nextBoxRow.Add((left: nextPosRight, right: nextPosRight + new Vector2i(1, 0)));
                  }
               }
               // Add the boxes that we found to the list of boxes we need to consider
               boxesConsidered.AddRange(nextBoxRow);
               // Move to the next row
               currentBoxRow = nextBoxRow;
            }


            // We can move the robot if all boxes can move
            bool canMove = boxesConsidered.All(box =>
            {
               var nextPosLeft = box.left + direction;
               var nextPosRight = box.right + direction;
               return CharAt(nextPosLeft) != '#' && CharAt(nextPosRight) != '#';
            });
            return (boxesConsidered, canMove);
         }

         public record struct Vector2i(int x, int y)
         {
            public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.x + b.x, a.y + b.y);
            public override string ToString() => $"({x}, {y})";
         }
      }


   }
}
