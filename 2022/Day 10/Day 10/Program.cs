using System.Diagnostics;

class Program
{

    static int totalSignalStrength = 0; 
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] input = File.ReadAllLines(@"../../../../input.txt");
        Solve(input);

        stopwatch.Stop();
        Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    private static void Solve(IEnumerable<string> input)
    {
        (int xRegister, int cycleCount) data = (1,0);
        Operation[] operations = input.Select(line => ToOperation(line)).ToArray();
        foreach (Operation operation in operations)
        {
            data = UpdateData(data, operation.Apply(data.xRegister));
            if (data.cycleCount > 220)
            {
                break;
            }
        }

        Console.WriteLine("Total signal strength: " + totalSignalStrength);
    }

    private static (int xRegister, int cycleCount) UpdateData((int xRegister, int cycleCount) existingData, (int value, int cycleCount) newData)
    {
        int cycleCount = existingData.cycleCount;
        for (int i = 0; i < newData.cycleCount; i++)
        {
            if ((++cycleCount % 20) == 0 && (cycleCount % 40 != 0))
            {
                int signalStrength = cycleCount * existingData.xRegister;
                totalSignalStrength += signalStrength;
                Console.WriteLine("Cyclecount: " + cycleCount + "\tRegister: " + existingData.xRegister + "\tSignal Strength: " + signalStrength);
            }
        }
        return (newData.value, cycleCount);
    }

    public static Operation ToOperation(string line)
    {
        if (line.StartsWith('n'))
            return new NoOp();
        else 
            return new AddX(int.Parse(line.Split(' ')[1]));
    }

    public abstract class Operation
    {
        public int cycleCount = 1;

        public abstract (int value, int cycleCount) Apply(int onValue);
    }

    public class NoOp : Operation
    {
        public override (int value, int cycleCount) Apply(int onValue)
        {
            return (onValue, cycleCount);
        }
    }

    public class AddX : Operation
    {
        public int toAdd;

        public AddX(int toAdd)
        {
            this.toAdd = toAdd;
            this.cycleCount = 2;
        }

        public override (int value, int cycleCount) Apply(int onValue)
        {
            return (onValue + toAdd, cycleCount);
        }
    }
}