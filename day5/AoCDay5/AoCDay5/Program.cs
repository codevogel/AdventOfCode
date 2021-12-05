using System;
using System.Collections.Generic;
using System.Drawing;

namespace AoCDay5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Convert input file to data
            string[] input = ReadInput(@"C:/workspace/AoC2021/day5/input.txt");


            List<Line> lines;
            ProcessInput(input, out lines);

            PartOne(lines);
        }

        private static void PartOne(List<Line> lines)
        {
            Dictionary<Point, int> overlapDict = new Dictionary<Point, int>();

            foreach (Line line in lines)
            {
                foreach (Point point in line.GetPointsInLine())
                {
                    if (overlapDict.ContainsKey(point))
                    {

                        overlapDict[point] += 1;
                    }
                    else
                    {
                        overlapDict.Add(point, 1);
                    }
                }
            }

            int count = 0;
            foreach (int value in overlapDict.Values)
            {
                if (value >= 2)
                    count++;
            }
            Console.WriteLine("Overlap occurs of at least two lines occurs on " + count + " points");
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
        private static void ProcessInput(string[] input, out List<Line> lines)
        {
            lines = new List<Line>();
            foreach (string inputLine in input)
            {
                string[] parts = inputLine.Split(" -> ");

                string[] start = parts[0].Split(",");
                Point startPoint = new Point(int.Parse(start[0]), int.Parse(start[1]));

                string[] end = parts[1].Split(",");
                Point endPoint = new Point(int.Parse(end[0]), int.Parse(end[1]));
                lines.Add(new Line(startPoint, endPoint));
            }
        }

        public struct Line
        {
            public Point start;
            public Point end;

            public Line(Point start, Point end)
            {
                this.start = start;
                this.end = end;
            }

            public bool IsHorizontalOrVertical()
            {
                return start.X == end.X || start.Y == end.Y;
            }
            public bool IsVertical()
            {
                return start.X == end.X;
            }

            public bool IsHorizontal()
            {
                return start.Y == end.Y;
            }

            public Point[] GetPointsInLine()
            {
                if (IsHorizontal())
                {
                    int startX = Math.Min(start.X, end.X);
                    int endX = Math.Max(start.X, end.X);
                    int dX = endX - startX;

                    Point[] points = new Point[dX + 1];
                    for (int x = startX; x <= endX; x++)
                    {
                        points[x - startX] = new Point(x, start.Y);
                    }
                    return points;
                }
                else if (IsVertical())
                {
                    int startY = Math.Min(start.Y, end.Y);
                    int endY = Math.Max(start.Y, end.Y);
                    int dY = endY - startY;

                    Point[] points = new Point[dY + 1];
                    for (int y = startY; y <= endY; y++)
                    {
                        points[y - startY] = new Point(start.X, y);
                    }
                    return points;
                }
                else
                {

                    int dirX = start.X - end.X > 0 ? -1 : 1;
                    int dirY = start.Y - end.Y > 0 ? -1 : 1;

                    List<Point> points = new List<Point>();

                    for (int i = 0; i <= Math.Abs(start.X - end.X); i++)
                    {
                        points.Add(new Point(start.X + i * dirX, start.Y + i * dirY));
                    }

                    return points.ToArray();
                }
                throw new NotImplementedException();
            }
        }
    }
}
