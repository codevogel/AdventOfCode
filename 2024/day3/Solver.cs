using System.Text.RegularExpressions;

namespace AdventOfCode
{
   public class Solver
   {
      static void Main(string[] args)
      {
         string inputPath = "input.txt";
         var instruction_set = Parse(inputPath, false);
         var answerA = instruction_set.Select(instruction => instruction.a * instruction.b).Sum();
         Console.WriteLine(answerA);
         var instruction_set_b = Parse(inputPath, true);
         var answerB = instruction_set_b.Select(instruction => instruction.a * instruction.b).Sum();
         Console.WriteLine(answerB);
      }

      static Instruction[] Parse(string inputPath, bool extra_instructions)
      {
         string[] lines = System.IO.File.ReadAllLines(inputPath);
         // Aggregate all lines into a single string
         string aggregate = lines.Aggregate((a, b) => a + b);
         // Recursively extract instructions from the aggregate string
         return ExtractInstructions(aggregate, new List<Instruction>(), extra_instructions).ToArray();
      }

      static List<Instruction> ExtractInstructions(string content, List<Instruction> instructions, bool extra_instructions, bool skipping = false)
      {
         var regex_mul = new Regex(@"^mul\(\d*,\d*\)");
         var regex_do = new Regex(@"^do\(\)");
         var regex_dont = new Regex(@"^don't\(\)");

         // While there is still content to parse
         while (content.Length != 0)
         {
            // If looking for do and don't instructions
            if (extra_instructions)
            {
               // If start of content matches do() or don't(), skip 1 character, mark skipping accordingly, and continue parsing
               if (regex_do.IsMatch(content))
               {
                  content = content.Substring(1);
                  return ExtractInstructions(content, instructions, extra_instructions, false);
               }
               if (regex_dont.IsMatch(content))
               {
                  content = content.Substring(1);
                  return ExtractInstructions(content, instructions, extra_instructions, true);
               }
            }
            // If we are not skipping any instructions, and the instruction is a mul instruction, break out of the loop
            if (!skipping && regex_mul.IsMatch(content))
            {
               break;
            }
            // If we did not find any instructions, keep looking
            content = content.Substring(1);
         }
         // If there is no content left, return the instructions
         if (content.Length == 0)
         {
            return instructions;
         }

         // We found a mul() instruction
         var instruction_string = regex_mul.Match(content).Value;
         // Skip the content forward by the length of the instruction
         content = content.Substring(instruction_string.Length);
         // Parse the instruction and add it to the list
         var split_str = instruction_string.Substring(startIndex: "mul(".Length, length: instruction_string.Length - "mul()".Length).Split(",");
         var instruction = new Instruction("*", a: int.Parse(split_str[0]), b: int.Parse(split_str[1]));
         instructions.Add(instruction);
         // Keep parsing
         return ExtractInstructions(content, instructions, extra_instructions, skipping);
      }

   }

   public struct Instruction
   {
      public string op; // This was in case part two required different operations
      public int a, b;

      public Instruction(string op, int a, int b)
      {
         this.op = op;
         this.a = a;
         this.b = b;
      }
   }

}
