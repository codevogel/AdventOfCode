using System.Diagnostics;

class Program
{

	static string messageCopy;

	static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		string message = File.ReadAllLines(@"../../../../input.txt")[0];
		Console.WriteLine("Outcome A:" + (message.Length - FilterPacket(message, 4).Length + 4));
		Console.WriteLine("Outcome B:" + (message.Length - FilterPacket(message, 14).Length + 14));

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

	private static string FilterPacket(string message, int length)
	{
		if (message.Substring(0,length).Distinct().Count() == length)
		{
			return message;
		}
		return FilterPacket(string.Concat(message.Skip(1)), length);
	}
}