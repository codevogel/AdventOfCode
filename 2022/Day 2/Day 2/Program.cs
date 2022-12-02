using System.Diagnostics;

class Program
{

	private static string[][] output;
	private static int score;

	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		ReadInput(File.ReadAllLines(@"../../../../input.txt"), out output);

		SolveA();
		SolveB();

		stopwatch.Stop();
		System.Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static void ReadInput(string[] input, out string[][] output)
	{
		output = input.Select(line => line.Split(' ')).ToArray();
	}

    private static void PlayGameA(string p1, string p2)
    {
        switch (p1)
        {
			case "A":
                switch (p2)
                {
					case "X":
						score += 1 + 3;
						break;
					case "Y":
						score += 2 + 6;
						break;
					case "Z":
						score += 3 + 0;
						break;
                }
				break;
			case "B":
				switch (p2)
				{
					case "X":
						score += 1 + 0;
						break;
					case "Y":
						score += 2 + 3;
						break;
					case "Z":
						score += 3 + 6;
						break;
				}
				break;
			case "C":
				switch (p2)
				{
					case "X":
						score += 1 + 6;
						break;
					case "Y":
						score += 2 + 0;
						break;
					case "Z":
						score += 3 + 3;
						break;
				}
				break;
		}
    }

	private static void PlayGameB(string p1, string p2)
	{
		switch (p1)
		{
			case "A":
				switch (p2)
				{
					case "X":
						score += 3 + 0;
						break;
					case "Y":
						score += 1 + 3;
						break;
					case "Z":
						score += 2 + 6;
						break;
				}
				break;
			case "B":
				switch (p2)
				{
					case "X":
						score += 1 + 0;
						break;
					case "Y":
						score += 2 + 3;
						break;
					case "Z":
						score += 3 + 6;
						break;
				}
				break;
			case "C":
				switch (p2)
				{
					case "X":
						score += 2 + 0;
						break;
					case "Y":
						score += 3 + 3;
						break;
					case "Z":
						score += 1 + 6;
						break;
				}
				break;
		}
	}

	private static void SolveA()
	{
		score = 0;
		output.ToList().ForEach(parts => PlayGameA(parts[0], parts[1]));
		Console.WriteLine("Outcome A: " + score);
	}

	private static void SolveB()
	{
		score = 0;
		output.ToList().ForEach(parts => PlayGameB(parts[0], parts[1]));
		Console.WriteLine("Outcome B: " + score);
	}
}