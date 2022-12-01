using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace day17
{
    class Program
    {

        static (int x, int y) startArea;
        static (int x, int y) stopArea;

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ReadInput(out startArea, out stopArea);
            Solve();

            stopwatch.Stop();
            System.Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
        }

        private static void Solve()
        {
            List<List<(int x, int y)>> trajectories = new();
            List<(int x, int y)> velocities = new();

            for (int x = 1; x <= stopArea.x; x++) // Velocities that are 0 or faster than stopArea.x will always miss
            {
                // Velocities that are lower than stopArea.y always miss. 
                // Highest absolute value of area y bounds is limit for y velocity 
                for (int y = stopArea.y; y <= Math.Max(Math.Abs(startArea.y), Math.Abs(stopArea.y)); y++)
                {
                    (bool hit, List<(int x, int y)> positions) trajectory = LaunchProbe((x, y));
                    if (trajectory.hit)
                    {
                        trajectories.Add(trajectory.positions);
                        velocities.Add((x, y));
                    }
                }
            }
            System.Console.WriteLine("Max trickshot y: " + trajectories.SelectMany(trajectory => trajectory.Select(pos => pos.y)).Max());
            System.Console.WriteLine("Distinct init velocities that hit: " + velocities.Distinct().Count());
        }

        static (bool hit, List<(int x, int y)> positions) LaunchProbe((int x, int y) velocity)
        {
            (int x, int y) position = (0, 0);
            List<(int x, int y)> positions = new();
            while (true)
            {
                positions.Add(position = (position.x + velocity.x, position.y + velocity.y));
                velocity = (ApplyDrag(velocity.x), velocity.y - 1);
                if (position.x >= startArea.x && position.x <= stopArea.x && position.y <= startArea.y && position.y >= stopArea.y)
                {
                    return (true, positions); // Hit in case in area
                }
                if (position.y < stopArea.y || position.x > stopArea.x)
                {
                    return (false, null); // Miss in case area has been passed
                }
            }
        }

        private static int ApplyDrag(int x)
        {
            if (x > 0)
                return x - 1;
            if (x < 0)
                return x + 1;
            return x;
        }

        private static void ReadInput(out (int x, int y) startArea, out (int x, int y) stopArea)
        {
            (int low, int high)[] coords = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day17\input.txt")[0].Trim('\n')
                            ["target area: ".Length..].Split(", ")
                            .Select(part => part[2..].Split("..")
                            .Select(coords => int.Parse(coords)).ToList())
                            .Select(area => (area.First(), area.Last())).ToArray();
            startArea = (coords[0].low, coords[1].high);
            stopArea = (coords[0].high, coords[1].low);
        }
    }
}
