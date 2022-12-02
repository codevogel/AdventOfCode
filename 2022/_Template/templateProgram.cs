using System.Diagnostics;

class Program
{

	private static string[][] output;

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

	private static void ReadInput(string[] input, out string[][] output)
	{

	}

	private static void SolveA()
	{
		Console.WriteLine("Outcome A: ");
	}

	private static void SolveB()
	{
		Console.WriteLine("Outcome B: ");
	}
}