using System;
using System.Collections.Generic;
using System.Linq;

// By Kamiel de Visser

namespace Day10
{
    class Program
    {

        static char[] openers = { '(', '[', '{', '<' };
        static char[] closers = { ')', ']', '}', '>' };
        static int[] scoreTable = { 3, 57, 1197, 25137 };

        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines(@"C:/workspace/AoC2021/day10/input.txt").Select(x => x.Trim('\n')).ToArray();
            PartOne(input);
            PartTwo(input);
        }

        private static void PartTwo(string[] input)
        {
            long[] scores = input.Where(x => IsLegal(x) == 0).ToArray().Select(x => CompleteLegalLine(x)).OrderBy(x => x).ToArray();
            Console.WriteLine("Middle score: " + scores[scores.Length / 2]);
        }

        private static void PartOne(string[] input)
        {
            Console.WriteLine("Total syntax error: " + input.Select(x => IsLegal(x)).Sum());
        }


        // Returns 0 if legal, syntax error score if not
        private static int IsLegal(string line)
        {
            Stack<char> openStack = new Stack<char>();

            foreach (char c in line)
            {
                if (openers.Contains(c))
                {
                    openStack.Push(c);
                }
                else
                {
                    // If current char does not correspond to popped open character
                    if (Array.IndexOf(closers, c) != Array.IndexOf(openers, openStack.Pop()))
                    {
                        // Return syntax error score
                        return scoreTable[Array.IndexOf(closers, c)];
                    }
                }
            }
            return 0;
        }

        // Returns autocomplete score for line
        private static long CompleteLegalLine(string line)
        {
            Stack<char> openStack = new Stack<char>();

            foreach (char c in line)
            {
                if (openers.Contains(c))
                {
                    openStack.Push(c);
                }
                else // closing character
                {
                    // Legal line so assume it closed correctly
                    openStack.Pop();
                }
            }
            long score = 0;
            // For each leftover unclosed character
            foreach (char c in openStack)
            {
                score *= 5;
                score += Array.IndexOf(openers, c) + 1;
            }
            return score;
        }
    }
}
