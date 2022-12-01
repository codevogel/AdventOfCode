using System;
using System.Collections.Generic;
using System.Linq;

namespace day13
{

    // By Kamiel de Visser 

    class Program
    {

        static char[,] paper;

        static void Main(string[] args)
        {
            Solve();
        }

        private static void Solve()
        {
            (int x, int y)[] points;
            (bool x, int coord)[] instructions;
            ReadInput(out points, out instructions, out paper);
            Console.WriteLine("Ans part 1:\n");
            foreach ((bool x, int coord) instruction in instructions)
            {
                Fold(instruction);
                if (instruction == instructions[0])
                    Console.WriteLine("Num dots after fold 1: " + paper.Cast<char>().Where(c => c.Equals('#')).Count());
            }
            Console.WriteLine("\nAns part 2:\n");
        }

        private static void Fold((bool foldOverX, int coord) instruction)
        {
            Queue<char[]> firstHalf = new(), secondHalf = new();
            if (instruction.foldOverX)
            {
                // top of fold
                for (int x = instruction.coord - 1; x >= 0; x--)
                {
                    firstHalf.Enqueue(GetCol(paper, x));
                }
                // bottom of fold
                for (int x = instruction.coord + 1; x < paper.GetLength(0); x++)
                {
                    secondHalf.Enqueue(GetCol(paper, x));
                }
                paper = MergeRows(secondHalf, firstHalf, instruction.foldOverX);
                return;
            }

            // left of fold
            for (int y = instruction.coord - 1; y >= 0; y--)
            {
                firstHalf.Enqueue(GetRow(paper, y));
            }
            // right of fold
            for (int y = instruction.coord + 1; y < paper.GetLength(1); y++)
            {
                secondHalf.Enqueue(GetRow(paper, y));
            }

            paper = MergeRows(secondHalf, firstHalf, instruction.foldOverX);
        }

        private static char[,] MergeRows(Queue<char[]> firstHalf, Queue<char[]> secondHalf, bool foldOverX)
        {
            Stack<char[]> newRows = new();
            while (firstHalf.Count > 0)
            {
                char[] rowBot = firstHalf.Dequeue();
                char[] rowTop = secondHalf.Dequeue();
                newRows.Push(MergeRow(rowBot, rowTop));
            }

            List<char[]> newPaper = new List<char[]>();
            while (newRows.Count > 0)
            {
                newPaper.Add(newRows.Pop());
            }

            char[,] output = ConvertTo2DArray(newPaper);
            if (foldOverX)
            {
                output = TurnArrayClockwise(output);
            }
            return output;
        }

        private static char[,] TurnArrayClockwise(char[,] output)
        {
            List<char[]> newRows = new();
            for (int x = 0; x < output.GetLength(0); x++)
            {
                newRows.Add(GetCol(output, x));
            }
            return ConvertTo2DArray(newRows);

        }

        private static char[,] ConvertTo2DArray(List<char[]> newRows)
        {
            char[,] output = new char[newRows[0].Length, newRows.Count];
            for (int y = 0; y < output.GetLength(1); y++)
            {
                for (int x = 0; x < output.GetLength(0); x++)
                {
                    output[x, y] = newRows[y][x];
                }
            }
            return output;
        }

        private static char[] MergeRow(char[] rowBot, char[] rowTop)
        {
            string row = "";
            for (int i = 0; i < rowBot.Length; i++)
            {
                bool filled = rowBot[i].Equals('#') || rowTop[i].Equals('#');
                row += filled ? '#' : '.';
            }
            return row.ToArray();
        }

        private static char[] GetRow(char[,] grid, int y)
        {
            return Enumerable.Range(0, grid.GetLength(0)).Select(i => grid[i, y]).ToArray();
        }

        private static char[] GetCol(char[,] grid, int x)
        {
            return Enumerable.Range(0, grid.GetLength(1)).Select(i => grid[x, i]).ToArray();
        }

        private static void PrintPaper()
        {
            for (int y = 0; y < paper.GetLength(1); y++)
            {
                for (int x = 0; x < paper.GetLength(0); x++)
                {
                    System.Console.Write(paper[x, y] + " ");
                }
                System.Console.Write("\n");
            }
            System.Console.WriteLine("\n");
        }

        private static void ReadInput(out (int x, int y)[] points, out (bool, int)[] instructions, out char[,] paper)
        {
            string[] input = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day13\input.txt").Select(line => line.Trim('\n')).ToArray();
            string[] inputCoords = input.Where(line => line.Count() > 0 && char.IsDigit(line[0])).ToArray();
            points = inputCoords.Select(coord => ToPoint(coord.Split(','))).ToArray();
            string[][] inputInstructions = input.Where(line => !string.IsNullOrWhiteSpace(line) && char.IsLetter(line[0])).Select(instruction => instruction.Split(' ')[2].Split('=')).ToArray();
            instructions = inputInstructions.Select(instruction => (instruction[0].Equals("x"), int.Parse(instruction[1]))).ToArray();

            int maxY = points.Max(point => point.y) + 1, maxX = points.Max(point => point.x) + 1;
            paper = new char[maxX, maxY];
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (points.Contains((x, y)))
                    {
                        paper[x, y] = '#';
                        continue;
                    }
                    paper[x, y] = '.';
                }
            }
        }

        private static (int, int) ToPoint(string[] coord)
        {
            return (int.Parse(coord[0]), int.Parse(coord[1]));
        }
    }
}
