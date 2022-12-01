// By Kamiel de Visser

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace Day9
{
    class Program
    {

        static int gridBoundsX, gridBoundsY;
        static int[,] grid;

        static void Main(string[] args)
        {
            string[] input = ReadInput(@"C:/workspace/AoC2021/day9/input.txt");
            ProcessInput(input, out grid, out gridBoundsX, out gridBoundsY);

            System.Console.WriteLine(PartOne(grid));
            System.Console.WriteLine(PartTwo(grid));
        }

        private static string PartOne(int[,] grid)
        {
            // Find lowest points in grid
            List<Point> listOfLowest = GetListOfLowest(grid);

            int risk = 0;
            // Foreach lowest point find corresponding value and increment riskfactor
            foreach (Point lowest in listOfLowest)
            {
                risk += 1 + grid[lowest.X, lowest.Y];
            }

            return "Risk factor: " + risk;
        }

        /// <summary>
        /// Gets a list of the locations in the grid that are lower than any of its adjacent locations
        /// </summary>
        private static List<Point> GetListOfLowest(int[,] grid)
        {
            List<Point> listOfLowest = new List<Point>();

            // For every point in the grid
            for (int x = 0; x < gridBoundsX; x++)
            {
                for (int y = 0; y < gridBoundsY; y++)
                {
                    // Mark lowest as true
                    bool lowest = true;
                    // Find neighbours
                    List<Point> neighbours = GetNeighbours(x, y);
                    // For all neighbours
                    foreach (Point point in neighbours)
                    {
                        // If point is higher than neighbours
                        if (grid[x, y] >= grid[point.X, point.Y])
                        {
                            // Mark lowest as false
                            lowest = false;
                        }
                    }
                    // If still lowest
                    if (lowest)
                    {
                        // Add to list
                        listOfLowest.Add(new Point(x, y));
                    }
                }
            }

            return listOfLowest;
        }

        private static string PartTwo(int[,] grid)
        {
            // Get lowest points
            List<Point> listOfLowest = GetListOfLowest(grid);


            // Find basins and note their sizes
            List<int> maxBasinSizes = new List<int>();
            foreach (Point lowest in listOfLowest)
            {
                maxBasinSizes.Add(GetBasin(lowest).Count);
            }

            // Remove min basins untill 3 are left over
            while (maxBasinSizes.Count > 3)
            {
                maxBasinSizes.Remove(maxBasinSizes.Min());
            }

            // Multiply sizes together
            int sum = 1;
            for (int i = 0; i < 3; i++)
            {
                sum *= maxBasinSizes[i];
            }

            return "Sizes of three largest basins multiplied: " + sum;
        }

        /// <summary>
        /// Gets a basin given startindex
        /// </summary>
        private static List<Point> GetBasin(Point startIndex)
        {
            // Keeps track of current basin contents
            List<Point> basin = new List<Point>();
            // Potential neighbours
            List<Point> startNeighbours = GetNeighbours(startIndex.X, startIndex.Y, true);

            return FillBasin(basin, startNeighbours);
        }

        /// <summary>
        /// Recursive function that fills a basin
        /// </summary>
        private static List<Point> FillBasin(List<Point> basin, List<Point> availableNeighbours)
        {
            // Return if no more neighbours
            if (availableNeighbours.Count == 0)
            {
                return basin;
            }

            // New neighbours to track
            List<Point> newlyFoundNeighbours = new List<Point>();

            // For each neighbur
            foreach (Point neighbour in availableNeighbours)
            {
                // Get their neighbours
                List<Point> newNeighbours = GetNeighbours(neighbour.X, neighbour.Y, true);
                // For each of their neighbours
                foreach (Point newNeighbour in newNeighbours)
                {
                    // If neighbour is already contained in basin, skip
                    if (basin.Contains(newNeighbour))
                    {
                        continue;
                    }
                    // Add new neighbour
                    basin.Add(newNeighbour);
                    newlyFoundNeighbours.Add(newNeighbour);
                }
            }
            // Fill basin with potential neighbours
            return FillBasin(basin, newlyFoundNeighbours);
        }

        /// <summary>
        /// Gets the neighbours from a point on the grid
        /// </summary>
        private static List<Point> GetNeighbours(int x, int y, bool basin = false)
        {
            // Potential locations
            Point[] neighbourIndeces = { new Point(x, y - 1), new Point(x + 1, y), new Point(x, y + 1), new Point(x - 1, y) };


            List<Point> neighbours = new List<Point>();
            foreach (Point neighbour in neighbourIndeces)
            {
                // Skip cases that exceed grid bounds
                if (neighbour.X < 0 || neighbour.X >= gridBoundsX || neighbour.Y < 0 || neighbour.Y >= gridBoundsY)
                {
                    continue;
                }
                // If looking for basin neighbours
                if (basin)
                {
                    // Also skip neighbours that are 9
                    if (grid[neighbour.X, neighbour.Y] == 9)
                    {
                        continue;
                    }
                }
                // Else add neighbour
                neighbours.Add(neighbour);
            }
            return neighbours;
        }

        /// <summary>
        /// Parse input file into string array
        /// </summary>
        private static string[] ReadInput(string filePath)
        {
            string[] input = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Trim('\n');
            }
            return input;
        }


        /// <summary>
        /// Load data from string array
        /// </summary>
        private static void ProcessInput(string[] input, out int[,] grid, out int gridBoundsX, out int gridBoundsY)
        {
            grid = new int[input[0].Length, input.Length];

            gridBoundsX = grid.GetLength(0);
            gridBoundsY = grid.GetLength(1);

            for (int y = 0; y < input.GetLength(0); y++)
            {
                for (int x = 0; x < input[0].Length; x++)
                {
                    grid[x, y] = int.Parse("" + input[y][x]);
                }
            }
        }
    }
}


