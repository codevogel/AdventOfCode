using System.Diagnostics;

class Program
{

	private static (Range elfOne, Range elfTwo)[] output;

	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		ReadInput(File.ReadAllLines(@"../../../../input.txt"), out output);

		SolveA();
		SolveB();

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static void ReadInput(string[] input, out (Range elfOne, Range elfTwo)[] output)
	{
		output = input.Select(line => line.Split(','))
				.Select(parts => (left: parts[0].Split('-'),
								  right: parts[1].Split('-')))
				.Select(parts => (elfOne: new Range(int.Parse(parts.left[0]), int.Parse(parts.left[1])),
								  elfTwo: new Range(int.Parse(parts.right[0]), int.Parse(parts.right[1]))))
				.ToArray();
	}

	public struct Range
	{
		public int start; public int end;
		public Range(int start, int end) { this.start = start; this.end = end; }
		public bool Contains(Range range)
		{
			return this.start <= range.start && this.end >= range.end;
		}
		public bool Overlaps(Range range)
		{
			return (range.start >= this.start && range.start <= this.end) || (range.end >= this.start && range.end <= this.start);
		}
	}

	private static void SolveA()
	{
		var solution = output.Where(ranges => ranges.elfOne.Contains(ranges.elfTwo) || ranges.elfTwo.Contains(ranges.elfOne)).Count();
		Console.WriteLine("Outcome A: " + solution);
	}

	private static void SolveB()
	{
		var solution = output.Where(ranges => ranges.elfOne.Overlaps(ranges.elfTwo) || ranges.elfTwo.Overlaps(ranges.elfOne)).Count();
		Console.WriteLine("Outcome B: " + solution);
	}
}