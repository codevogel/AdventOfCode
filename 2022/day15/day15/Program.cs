
class Program
{
    public static void Main()
    {
        Dictionary<Point, (Point beacon, int distance)> sensorToBeaconMap = new();
        string[] input = File.ReadAllLines("../../../../input.txt");

        input.Select(line => line.Split(':')
                .Select(part => part.StartsWith('S') ?
                        part.Substring("Sensor at".Length) :
                        part.Substring(": closest beacon is at ".Length)
                )
                .Select(coordString => coordString.Split(',')
                    .Select(part => int.Parse(part.Split('=')[1])).ToArray()
                )
            ).ToList().ForEach(coordCombo => AddToMap(coordCombo.ToArray(), sensorToBeaconMap));

        var sensorsInRangeOfTargetLine = SensorsInRangeOfTargetLine(sensorToBeaconMap.Keys.ToArray(), 10);
        var tmp = sensorsInRangeOfTargetLine.Select(sensor => sensorToBeaconMap[sensor]);
    }

    private static Point[] SensorsInRangeOfTargetLine(Point[] sensors, int targetLineY)
    {
        List<Point> result = new List<Point>();
        foreach (Point sensor in sensors)
        {
            int yDistToBeacon = Math.Abs(targetLineY - sensor.Y);
            if (targetLineY <= sensor.Y + yDistToBeacon && targetLineY >= sensor.Y - yDistToBeacon)
            {
                result.Add(sensor);
            }
        }
        return result.ToArray();
    }

    private static void AddToMap(int[][] coordCombo, Dictionary<Point, (Point beacon, int distance)> sensorToBeaconMap)
    {
        Point a = new Point(coordCombo[0][0], coordCombo[0][1]);
        Point b = new Point(coordCombo[1][0], coordCombo[1][1]);
        sensorToBeaconMap.Add(a, (b, MHD(a, b)));
    }

    public static int MHD(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); 

    public static void ParseInput(string[] input)
    {

    }

    public struct Point { public int X { get; private set; } public int Y { get; private set; } 
        public Point(int x, int y) { X = x; Y = y; }
        public override string? ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }
    }

}

