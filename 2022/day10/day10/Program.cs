using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string[] input = File.ReadAllLines(@"../../../../input.txt");
        Console.WriteLine("Total signal strength: " + new VideoSystem(program: input).totalSignalStrength);

        stopwatch.Stop();
        Console.WriteLine("Found solution in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    class VideoSystem
    {
        private Sprite sprite;
        public int totalSignalStrength { get; private set; } = 0; 
        private int cycleCount = 0, x = 1;
        public const int LINESIZE = 40;
        
        public VideoSystem(IEnumerable<string> program)
        {
            sprite = new Sprite();
            sprite.SetPosition(1);
            foreach (string line in program)
            {
                if (line.StartsWith('n')) // noOp
                    Cycle(add: 0);
                else // AddX
                {
                    Cycle(add: 0);
                    Cycle(add: int.Parse(line.Split(' ')[1]));
                    sprite.SetPosition(x);
                }
                if (cycleCount > 240) { break; }
            }
            Console.Write('\n');
        }

        public void Cycle(int add)
        {
            // First draw the sprite and determine signal strength
            sprite.Draw(cycleCount);
            if (++cycleCount % 20 == 0 && cycleCount % LINESIZE != 0)
                totalSignalStrength += x * cycleCount;
            // Then add value at end of the cycle
            x += add;
        }

        public class Sprite
        {
            private int[] covers = Array.Empty<int>();
            public void SetPosition(int x) { covers = new int[] { x - 1, x, x + 1 }; }
            internal void Draw(int crtMarker)
            {
                if (crtMarker % 40 == 0 && crtMarker != 0)
                {
                    Console.Write('\n');
                }
                Console.Write(covers.Contains(crtMarker % VideoSystem.LINESIZE) ? '#' : '.');
            }
        }
    }
}