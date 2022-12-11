using System.Diagnostics;
using static Program.Monkey;

class Program
{

    static Dictionary<int, Monkey> tribe = new();

    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] input = File.ReadAllLines(@"../../../../input.txt");
        Solve(input, true);
        Solve(input, false);

        stopwatch.Stop();
        Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    private static void Solve(string[] input, bool partOne)
    {
        while(input.Length > 0)
        {
            int id = int.Parse(String.Concat(input[0].Split(' ')[1].SkipLast(1)));
            tribe[id] = new Monkey(
                startingItems:  input[1].Split(": ")[1].Split(',').Select(digit => long.Parse(digit)).ToList(), 
                operation:      new Operation(input[2].Split(" = ")[1]), 
                divisibleBy:    int.Parse(input[3].Split(" by ")[1]), 
                throwToTrue:    int.Parse(input[4].Split(" monkey ")[1]), 
                throwToFalse:   int.Parse(input[5].Split(" monkey ")[1])
            );
            input = input.Skip(7).ToArray();
        }

        Monkey.LCM = LCM(tribe.Values.Select(monkey => monkey.divisibleBy).ToArray());
        
        int numRounds = partOne ? 20 : 10000;
        for (int round = 0; round < numRounds; round++)
        {
            for (int monkeyIndex = 0; monkeyIndex < tribe.Count; monkeyIndex++)
            {
                Monkey currentMonkey = tribe[monkeyIndex];
                currentMonkey.InspectItems(partOne);
            }
        }

        var mostActiveMonkeyInspections = tribe.Values.OrderBy(monkey => monkey.timesInspected).TakeLast(2).Select(monkey => monkey.timesInspected).ToList();
        Console.WriteLine(string.Format("Part {0}: Monkey Business after {1} rounds: \t{2}", partOne ? 1 : 2, numRounds, mostActiveMonkeyInspections[0] * mostActiveMonkeyInspections[1]));
    }

    public static long LCM(long[] ofNumbers) // Find LCM using GCD (source: https://iq.opengenus.org/lcm-of-array-of-numbers/ )
    {
        long ans = ofNumbers[0];
        for (int i = 1; i < ofNumbers.Length; i++)
        {
            ans = ofNumbers[i] * ans / GCD(ofNumbers[i], ans);
        }
        return ans;
    }

    private static long GCD(long a, long b) { return b == 0 ? a : GCD(b, a % b); }

    public class Monkey
    {
        public Queue<long> inventory = new();
        public Operation operation;
        public long timesInspected = 0;
        public long divisibleBy;
        public int throwToTrue, throwToFalse;

        public static long LCM = 0;

        public Monkey(List<long> startingItems, Operation operation, int divisibleBy, int throwToTrue, int throwToFalse)
        {
            startingItems.ForEach(item => inventory.Enqueue(item));
            this.operation = operation;
            this.divisibleBy = divisibleBy;
            this.throwToTrue = throwToTrue;
            this.throwToFalse = throwToFalse;
        }

        public void InspectItems(bool partOne = true)
        {
            while(inventory.Count > 0)
            {
                timesInspected++;
                long worryLevel = inventory.Dequeue();
                worryLevel = partOne ? operation.Apply(worryLevel) / 3 : operation.Apply(worryLevel) % LCM;
                tribe[worryLevel % divisibleBy == 0 ? throwToTrue : throwToFalse].inventory.Enqueue(worryLevel);
            }
        }

        public class Operation
        {
            public Operation(string inputString)
            {
                Type = inputString.Contains('*') ? '*' : '+';
                var arg = inputString.Split(Type)[1];
                Value = arg.StartsWith(" o") ? -1 : long.Parse(arg); 
            }
            char Type { get; set; }
            long Value { get; set; }
            public long Apply(long onValue) { return Type == '*' ? onValue * (Value < 0 ? onValue : Value) : onValue + (Value < 0 ? onValue : Value); }
        }
    }
}