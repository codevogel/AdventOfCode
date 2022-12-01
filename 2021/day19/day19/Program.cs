using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace day19
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Scanner> scanners = ReadInput(@"C:/workspace/AOC2021/day19/input.txt");
        }

        private static List<Scanner> ReadInput(string filepath)
        {
            var input = System.IO.File.ReadAllLines(filepath).Select(line => line.Trim('\n')).Skip(1);
            List<Scanner> scanners = new();
            int id = 0;
            while (input.Count() > 0)
            {
                var positionData = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line));
                var positions = positionData.Select(position => position.Split(',').Select(number => int.Parse(number))).ToArray()
                                            .Select(position => new Vector3(position.ElementAt(0), position.ElementAt(1), position.ElementAt(2))).ToList();
                scanners.Add(new Scanner(id++, positions));
                input = input.Skip(positionData.Count() + 2);
            }
            return scanners;
        }
    }

    class Scanner
    {
        public Scanner(int id, List<Vector3> positions)
        {
            ID = id;
            Positions = positions;
            int a = 0;



            List<Dictionary<int, Vector3>> positionsPerDir = new();
            List<Vector3> coordsInDir = new();

            coordsInDir.Add(Vector3.Reflect(new Vector3(positions[0].X, positions[0].Y, positions[0].Z), new Vector3(1, 0, 0)));
            coordsInDir.Add(Vector3.Reflect(Positions[0], new Vector3(-1, 0, 0)));
            coordsInDir.Add(Vector3.Reflect(Positions[0], new Vector3(0, 1, 0)));
            coordsInDir.Add(Vector3.Reflect(Positions[0], new Vector3(0, -1, 0)));
            coordsInDir.Add(Vector3.Reflect(Positions[0], new Vector3(0, 0, 1)));
            coordsInDir.Add(Vector3.Reflect(Positions[0], new Vector3(0, 0, -1)));


            for (int i = 0; i < 3; i++)
            {
            }


            System.Console.WriteLine(Vector3.Reflect(Positions[0], new Vector3(-1, 0, 0)));

        }

        public int ID { get; private set; }
        public List<Vector3> Positions { get; set; }

        public List<List<Vector3>> AllPositions { get; set; }
    }
}
