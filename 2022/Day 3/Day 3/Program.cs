using System.Diagnostics;

static class Program
{

	private static string[][] outputA;
	private static string[][] outputB;

	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		ReadInput(File.ReadAllLines(@"../../../../input.txt"), out outputA, out outputB);

		Console.WriteLine("Outcome A: " +
			outputA.Select(compartment => GetCommonChar(compartment[0], compartment[1])).Select(c => char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1).Sum());
		Console.WriteLine("Outcome B: " +
			outputB.Select(rucksack => GetCommonChar(rucksack[0], rucksack[1], rucksack[2])).Select(c => char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1).Sum());

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static void ReadInput(string[] input, out string[][] outputA, out string[][] outputB)
	{
		outputA = input.Select(line => new string[] { line.Substring(0, line.Length / 2), line.Substring(line.Length / 2) }).ToArray();
		outputB = Chunk(input, 3).Select(chunk => chunk.ToArray()).ToArray();
	}

	/// <summary>
	/// Break a list of items into chunks of a specific size (from https://stackoverflow.com/a/6362642/12094608 )
	/// </summary>
	public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
	{
		while (source.Any())
		{
			yield return source.Take(chunksize);
			source = source.Skip(chunksize);
		}
	}

	private static char GetCommonChar(string x, string y, string z = "" )
	{
		for (char c = 'A'; c <= 'z'; c++) // beware: does some unnessecary symbol char checks
		{
			if (x.Contains(c) && y.Contains(c))
				if (string.IsNullOrEmpty(z))
					return c;
				else if (z.Contains(c))
					return c;
		}
		return '0';
	}
}