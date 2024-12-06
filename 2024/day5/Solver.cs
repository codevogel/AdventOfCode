

namespace AdventOfCode
{
   public class Solver
   {

      static void Main(string[] args)
      {
         string inputPath = "input.txt";
         var (orderingRules, updates) = Parse(inputPath);

         var updatesInCorrectOrder = updates.Where(update => IsInCorrectOrder(update, orderingRules)).ToArray();
         var middlePageNumbers = updatesInCorrectOrder.Select(update => update[update.Length / 2]).ToArray();
         Console.WriteLine(middlePageNumbers.Sum());

         var updatesInWrongOrder = updates.Where(update => !IsInCorrectOrder(update, orderingRules)).ToArray();
         var fixedUpdates = updatesInWrongOrder.Select(update => FixOrder(update, orderingRules)).ToArray();
         var fixedMiddlePageNumbers = fixedUpdates.Select(update => update[update.Length / 2]).ToArray();
         Console.WriteLine(fixedMiddlePageNumbers.Sum());
      }

      static (Dictionary<int, HashSet<int>> ordering_rules, int[][] updates) Parse(string inputPath)
      {
         string[] lines = File.ReadAllLines(inputPath);

         // Parse the ordering rules into a dictionary where the key is the page number and all page numbers that depend on it are stored in a hash set.
         var ruleLines = lines.TakeWhile(line => !string.IsNullOrEmpty(line));
         var orderingRules = new Dictionary<int, HashSet<int>>();
         ruleLines.Select(line => line.Split("|").Select(part => int.Parse(part.Trim())))
            .ToList().ForEach(nums =>
            {
               var (firstNum, secondNum) = (nums.First(), nums.Last());
               if (orderingRules.ContainsKey(nums.First()))
               {
                  orderingRules[firstNum].Add(secondNum);
                  return;
               }
               orderingRules[firstNum] = new HashSet<int>();
               orderingRules[firstNum].Add(secondNum);
            }
         );

         // Parse the updates 
         var updateLines = lines.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);
         var updates = updateLines.Select(line => line.Split(",").Select(part => int.Parse(part.Trim())).ToArray()).ToArray();
         return (orderingRules, updates);
      }

      private static bool IsInCorrectOrder(int[] update, Dictionary<int, HashSet<int>> orderingRules)
      {
         // Keep track of visited pages.
         HashSet<int> visited = new HashSet<int>();
         // The update is in correct order if all pages precede all pages that depend on them.
         return update.All(pageNumber =>
         {
            // A page is in correct order if it has no ordering rule.
            // If it has an ordering rule, it is in correct order if all preceding pages do not depend on it. 
            var correct = orderingRules.ContainsKey(pageNumber) ? !visited.Any(visitedPageNumber => orderingRules[pageNumber].Contains(visitedPageNumber)) : true;
            visited.Add(pageNumber);
            return correct;
         });
      }

      private static int[] FixOrder(int[] update, Dictionary<int, HashSet<int>> orderingRules)
      {
         // We sort the pages in the update by the number of pages that depend on them.
         return update
             .OrderBy(page => update.Count(earlier =>
                 orderingRules.ContainsKey(page) &&
                 orderingRules[page].Contains(earlier)))
             .ToArray();
      }

   }
}
