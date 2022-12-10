using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{

	private static int[] output;

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

	private static void ReadInput(string[] input, out int[] output)
	{
		output = input.Select(line => string.IsNullOrEmpty(line) ? -1 : int.Parse(line)).ToArray();
	}

	private static void SolveA()
	{
		int[] calories = output;
		int currentCalories = 0, maxCalories = 0;
		for (int i = 0; i < calories.Length; i++)
		{

			if (calories[i] == -1)
			{
				if (currentCalories > maxCalories)
				{
					maxCalories = currentCalories;
				}
				currentCalories = 0;
			}
			else
			{
				currentCalories += calories[i];
			}
		}

		Console.WriteLine(maxCalories);
	}

	private static void SolveB()
	{
		int[] inCalories = output;
		int currentCalories = 0, maxCalories = 0;
		List<int> outCalories = new List<int>();
		for (int i = 0; i < inCalories.Length; i++)
		{

			if (inCalories[i] == -1)
			{
				outCalories.Add(currentCalories);
				currentCalories = 0;
			}
			else
			{
				currentCalories += inCalories[i];
			}
		}

		outCalories.Sort();
		outCalories.Reverse();

		Console.WriteLine(outCalories[0] + outCalories[1] + outCalories[2]);
	}

}
