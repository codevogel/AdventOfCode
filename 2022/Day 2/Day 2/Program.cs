using System.Diagnostics;

class Program
{

    public static Dictionary<string, RPS> rpsMap = new Dictionary<string, RPS>()
    {
        { "A", RPS.ROCK }, { "B", RPS.PAPER }, { "C", RPS.SCISSORS },
        { "X", RPS.ROCK }, { "Y", RPS.PAPER }, { "Z", RPS.SCISSORS }
    };

    public static Dictionary<string, Outcome> outcomeMap = new Dictionary<string, Outcome>()
    {
        { "X", Outcome.LOSS }, { "Y", Outcome.DRAW }, { "Z", Outcome.WIN }
    };

    public enum RPS { ROCK = 1, PAPER = 2, SCISSORS = 3 }
    public enum Outcome { WIN = 6, DRAW = 3, LOSS = 0 }

    static string[] input;
    
    static void Main(string[] args)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

        input = File.ReadAllLines(@"../../../../input.txt");

        SolveA();
		SolveB();

		stopwatch.Stop();
		Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
	}

    private static void SolveA()
    {
        // Split file into 2d Rock Paper Scissors Array where each element contains the choices of player 1, then player 2 respectively.
        RPS[][] outputA = input.Select(line => line.Split(' ')).Select(parts => new RPS[] { rpsMap[parts[0]], rpsMap[parts[1]] }).ToArray();
        // Map scores for each game then calculate the sum
        int score = outputA.ToList().Select(parts => PlayGameA(parts[0], parts[1])).Sum();
        Console.WriteLine("Outcome A: " + score);
    }

    private static void SolveB()
    {
        // Split file into a tuple array where each element contains the choices of player 1, then player 2's desired outcome respectively.
        (RPS rps, Outcome outcome)[] outputB = input.Select(line => line.Split(' ')).Select(parts => (rpsMap[parts[0]], outcomeMap[parts[1]])).ToArray();
        // Map scores for each game then calculate the sum
        int score = outputB.ToList().Select(tuple => PlayGameB(tuple.rps, tuple.outcome)).Sum();
        Console.WriteLine("Outcome B: " + score);
    }

    private static int GetScore(RPS style, Outcome outcome)
    {
        // return corresponding enum values added together
		return (int)style + (int)outcome;
    }

    private static int PlayGameA(RPS p1, RPS p2)
    {
		return p1 switch
        {
            RPS.ROCK => p2 switch
            {
                RPS.ROCK => GetScore(RPS.ROCK, Outcome.DRAW),
                RPS.PAPER => GetScore(RPS.PAPER, Outcome.WIN),
                RPS.SCISSORS => GetScore(RPS.SCISSORS, Outcome.LOSS),
                _ => throw new NotImplementedException()
            },
            RPS.PAPER => p2 switch
            {
                RPS.ROCK => GetScore(RPS.ROCK, Outcome.LOSS),
                RPS.PAPER => GetScore(RPS.PAPER, Outcome.DRAW),
                RPS.SCISSORS => GetScore(RPS.SCISSORS, Outcome.WIN),
                _ => throw new NotImplementedException()
            },
            RPS.SCISSORS => p2 switch
            {
                RPS.ROCK => GetScore(RPS.ROCK, Outcome.WIN),
                RPS.PAPER => GetScore(RPS.PAPER, Outcome.LOSS),
                RPS.SCISSORS => GetScore(RPS.SCISSORS, Outcome.DRAW),
                _ => throw new NotImplementedException()
            },
            _ => throw new NotImplementedException()
        };
    }

	private static int PlayGameB(RPS p1, Outcome desiredOutcome)
	{
		return p1 switch
        {
            RPS.ROCK => desiredOutcome switch
            {
                Outcome.WIN => GetScore(RPS.PAPER, Outcome.WIN),
                Outcome.DRAW => GetScore(RPS.ROCK, Outcome.DRAW),
                Outcome.LOSS => GetScore(RPS.SCISSORS, Outcome.LOSS),
                _ => throw new NotImplementedException()
            },
            RPS.PAPER => desiredOutcome switch
            {
                Outcome.WIN => GetScore(RPS.SCISSORS, Outcome.WIN),
                Outcome.DRAW => GetScore(RPS.PAPER, Outcome.DRAW),
                Outcome.LOSS => GetScore(RPS.ROCK, Outcome.LOSS),
                _ => throw new NotImplementedException()
            },
            RPS.SCISSORS => desiredOutcome switch
            {
                Outcome.WIN => GetScore(RPS.ROCK, Outcome.WIN),
                Outcome.DRAW => GetScore(RPS.SCISSORS, Outcome.DRAW),
                Outcome.LOSS => GetScore(RPS.PAPER, Outcome.LOSS),
                _ => throw new NotImplementedException()
            },
            _ => throw new NotImplementedException(),
        };
	}
}