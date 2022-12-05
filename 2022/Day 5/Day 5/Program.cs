using System.Diagnostics;

class Program
{

	private static Stack<char>[] outputStacks;
	private static (int amount, int from, int to)[] instructions;

	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		ReadInput(File.ReadAllLines(@"../../../../input.txt"), out outputStacks, out instructions);
		Console.WriteLine("Outcome A: " + CompleteInstructionsA(outputStacks.Select(stack => new Stack<char>(stack)).ToArray()));
		Console.WriteLine("Outcome B: " + CompleteInstructionsB(outputStacks.Select(stack => new Stack<char>(stack)).ToArray()));

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static void ReadInput(string[] input, out Stack<char>[] outputStacks, out (int amount, int from, int to)[] instructions)
	{
		var stackTextColumns = input.TakeWhile(line => ! string.IsNullOrEmpty(line)).Select(line => Split(line).ToArray()).SkipLast(1).Reverse().ToArray();
		
		// Initialize stacks
		List<Stack<char>> stacks = new List<Stack<char>>();
		for (int i = 0; i < stackTextColumns[0].Length; i++)
		{
			stacks.Add(new Stack<char>());
		}
		// For each line in input
        foreach (string[] parts in stackTextColumns)
        {
			// Iterate over each column
			int stackIndex = 0;
			foreach(string part in parts)
            {
				// Add to stack corresponding to the current column if non-whitespace string
				if (!string.IsNullOrWhiteSpace(part))
					stacks[stackIndex].Push(part[1]);
				stackIndex++;
			}
		}
		outputStacks = stacks.ToArray();

		instructions = input.SkipWhile(line => !line.StartsWith('m'))
			.Select(line => line.Split(' '))
			.Select(parts => (int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1)).ToArray();
	}

    private static IEnumerable<string> Split(string line)
    {
		while (line.Length > 0)
		{
			yield return new string(line.Take(3).ToArray());
			line = new string(line.Skip(4).ToArray());
		}
	}

    private static string CompleteInstructionsA(Stack<char>[] stacks)
    {
        foreach ((int amount, int from, int to) instruction in instructions)
        {
            for (int i = 0; i < instruction.amount; i++)
            {
				// Pick up crate from one stack and place on another
				stacks[instruction.to].Push(stacks[instruction.from].Pop());
            }
        }

		return string.Concat(stacks.Select(stack => stack.Peek()));
	}

	private static string CompleteInstructionsB(Stack<char>[] stacks)
	{
		foreach ((int amount, int from, int to) instruction in instructions)
		{
			// Create stack of picked up crates
			Stack<char> cratesPickedUp = new Stack<char>();
			for (int i = 0; i < instruction.amount; i++)
			{
				cratesPickedUp.Push(stacks[instruction.from].Pop());
			}
			// Reverse that stack
			cratesPickedUp = new Stack<char>(cratesPickedUp.Reverse());
			while (cratesPickedUp.Count > 0)
			{
				// Then place crates on the destionation stack
				stacks[instruction.to].Push(cratesPickedUp.Pop());
			}
		}

		return string.Concat(stacks.Select(stack => stack.Peek()));
	}
}