using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace day11
{

    // By Kamiel de Visser
    class Program
    {

        static int[][] energyLevels;
        static int gridBoundsX, gridBoundsY;

        static void Main(string[] args)
        {
            Solve();
        }

        private static void Solve()
        {
            ReadInput(out energyLevels, out gridBoundsX, out gridBoundsY);
            int flashes = 0, numSteps = 1000;
            for (int i = 0; i < numSteps; i++)
            {
                // Increment grid
                flashes += FlashActiveSquids();
                energyLevels = energyLevels.Select(row => row.Select(num => num > 9 ? 0 : num).ToArray()).ToArray();
                if (energyLevels.All(row => row.All(num => num == 0)))
                {
                    Console.WriteLine("Step at which all octopi flash: " + (i + 1));
                    return;
                }

                if (i == 100 - 1)
                {
                    Console.WriteLine("Number of flashes at 100 steps: " + flashes);
                }
            }
            Console.WriteLine("No step at which all octopi flash in " + numSteps + " steps");
        }

        private static int FlashActiveSquids()
        {
            Queue<Point> activeSquids = IncrementSquids();
            int flashes = 0;
            while (activeSquids.Count > 0)
            {
                flashes++;
                Point activeSquid = activeSquids.Dequeue();
                foreach (Point neighbour in GetNeighbours(activeSquid.X, activeSquid.Y))
                {
                    if (energyLevels[neighbour.Y][neighbour.X]++ == 9)
                    {
                        activeSquids.Enqueue(neighbour);
                    }
                }
            }
            return flashes;
        }

        private static Queue<Point> IncrementSquids()
        {
            Queue<Point> activeSquids = new Queue<Point>();
            for (int x = 0; x < gridBoundsX; x++)
            {
                for (int y = 0; y < gridBoundsY; y++)
                {
                    if (energyLevels[y][x]++ == 9)
                    {
                        activeSquids.Enqueue(new Point(x, y));
                    }
                }
            }
            return activeSquids;
        }

        private static List<Point> GetNeighbours(int x, int y)
        {
            Point[] indexOffsets = { new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(-1, 0), new Point(1, 0), new Point(-1, 1), new Point(0, 1), new Point(1, 1) };
            return indexOffsets.Where(indexOffset => IsLegal(x + indexOffset.X, y + indexOffset.Y))
                           .Select(index => new Point(x + index.X, y + index.Y))
                           .ToList();
        }

        public static bool IsLegal(int x, int y)
        {
            return x >= 0 && x < gridBoundsX && y >= 0 && y < gridBoundsY;
        }

        private static void ReadInput(out int[][] grid, out int gridBoundsX, out int gridBoundsY)
        {
            string[] input = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day11\input.txt").Select(line => line.Trim('\n')).ToArray();
            gridBoundsX = input[0].Length;
            gridBoundsY = input.Length;
            grid = input.Select(y => y.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
        }
    }
}
