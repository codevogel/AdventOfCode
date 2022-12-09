using System.Diagnostics;
using static Program.Knot;

class Program
{
	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		string[] input = File.ReadAllLines(@"../../../../input.txt");
		Solve(input, 2);
		Solve(input, 10);

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static void Solve(IEnumerable<string> input, int nKnots)
	{
		Knot head = new Knot(nKnots - 1);

		var commands = input.Select(line => line.Split(' ')).Select(part => (directionsMap[part[0][0]], int.Parse(part[1]))).ToArray();
		foreach ((Direction dir, int steps) in commands)
		{
			for (int i = 0; i < steps; i++)
			{
				head.MoveInDir(dir);
			}
		}
		// Make last tail head
		while (head.Tail != null) { head = head.Tail; }
		Console.WriteLine(head.Visited.Distinct().Count());
	}

	public class Knot
	{
		public Knot? Tail { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public List<(int x, int y)> Visited { get; private set; } = new();
		public Knot(int times = 0)
		{
			Visited.Add((X = 0, Y = 0));
			Tail = times > 0 ? new Knot(times - 1) : null;
		}

		public void MoveInDir(Direction dir)
		{
			var offset = coordinatesMap[dir];
			Visited.Add((X += offset.X, Y += offset.Y));
			if (Tail != null && !this.Touches(Tail))
			{
				Tail.MoveInDir(GetDir(Tail, this));
			}
		}

		private Direction GetDir(Knot from, Knot to)
		{
			Direction x = from.X == to.X ? Direction.NONE : (from.X < to.X ? Direction.R : Direction.L);
			Direction y = from.Y == to.Y ? Direction.NONE : (from.Y < to.Y ? Direction.U : Direction.D);
			return (Direction)(int)x + (int)y;
		}

		public bool Touches(Knot other)
		{
			for (int offsetX = -1; offsetX <= 1; offsetX++)
			{
				for (int offsetY = -1; offsetY <= 1; offsetY++)
				{
					if (this.X + offsetX == other.X && this.Y + offsetY == other.Y)
						return true;
				}
			}
			return false;
		}

		public enum Direction
		{
			U = 1, R = 2, D = 4, L = 8, UR = U + R, RD = R + D, DL = D + L, LU = L + U, NONE = 0
		}

		public static Dictionary<char, Direction> directionsMap = new Dictionary<char, Direction>()
		{
			{ 'U', Direction.U }, {'R', Direction.R}, {'D', Direction.D}, {'L', Direction.L}
		};

		public static Dictionary<Direction, (int X, int Y)> coordinatesMap = new Dictionary<Direction, (int X, int Y)>
		{ { Direction.U, (0,1) }, { Direction.R, (1,0) }, { Direction.D, (0,-1) }, { Direction.L, (-1, 0) },
		  { Direction.UR, (1,1) }, { Direction.RD, (1,-1) }, { Direction.DL, (-1,-1) }, { Direction.LU, (-1,1) }};

	}
}