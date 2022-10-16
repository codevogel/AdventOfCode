using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace day15
{
    class Program
    {

        static Node[][] grid;

        static void Main(string[] args)
        {
            ReadInput(out grid);

        }

        private static List<Node> GetNeighbours(int x, int y)
        {
            Point[] indexOffsets = { new Point(0, -1), new Point(-1, 0), new Point(1, 0), new Point(0, 1) };
            return indexOffsets.Where(indexOffset => IsLegal(x + indexOffset.X, y + indexOffset.Y))
                            .Select(index => grid[y + index.Y][x + index.X])
                            .ToList();
        }

        public static bool IsLegal(int x, int y)
        {
            return x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length;
        }

        private static void ReadInput(out Node[][] grid)
        {
            string[] input = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day15\input.txt").Select(line => line.Trim('\n')).ToArray();
            int i = 1;
            grid = Enumerable.Range(0, input.Length).Select(y => Enumerable.Range(0, input[0].Length).Select(x => new Node(x, y, int.Parse(input[y][x].ToString()), i++)).ToArray()).ToArray();
        }


        class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Risk { get; set; }
            public int Distance { get; set; } = int.MaxValue;
            public bool Visited { get; set; } = false;

            public Node(int x, int y, int risk)
            {
                X = x;
                Y = y;
                Risk = risk;
            }
        }
    }
}
