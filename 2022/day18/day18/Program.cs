class Program
{

    static void Main()
    {
        LavaScan lavaScan = new LavaScan(File.ReadAllLines("../../../../input.txt"));
    }

    class LavaScan
    {

        static (int x, int y, int z)[] neighbourOffsets = new (int x, int y, int z)[]
        {
                            (1,0,0), (-1,0,0),
                            (0,1,0), (0,-1,0),
                            (0,0,1), (0,0,-1),
        };

        public static Cube[,,] scan = new Cube[0,0,0];
        public static Cube[] cubesInScan = new Cube[0];
        static int scanSize = 0;

        public LavaScan(string[] input)
        {
            cubesInScan = input.Select(line => line.Split(','))
                .Select(parts => new Cube(x: int.Parse(parts[0]) + 1, y: int.Parse(parts[1]) + 1, z: int.Parse(parts[2]) + 1, isObsidian:true, isAir: false))
                .ToArray();
            scanSize = cubesInScan.Select(cube => new int[] { cube.x, cube.y, cube.z }.Max()).Max() + 2;
            scan = new Cube[scanSize, scanSize, scanSize];
            for (int x = 0; x < scanSize; x++)
            {
                for (int y = 0; y < scanSize; y++)
                {
                    for (int z = 0; z < scanSize; z++)
                    {
                        scan[x, y, z] = new Cube(x, y, z, false, false);
                    }
                }
            }
            cubesInScan.ToList().ForEach(cube => scan[cube.x, cube.y, cube.z].isObsidian = true);
            var ans = cubesInScan.Select(cube => cube.GetOpenSides(false)).Sum();

            var pos = (x: 0, y: 0, z: 0);
            scan[pos.x, pos.y, pos.z] = new Cube(pos.x, pos.y, pos.z, false, true);
            scan[pos.x, pos.y, pos.z].ExpandIntoAir();
            
            ans = cubesInScan.Select(cube => cube.GetOpenSides(true)).Sum();
            Console.WriteLine(ans);
        }

        public static Cube GetCubeAt(int x, int y, int z)
        {
            return scan[x, y, z];
        }

        public static bool InBounds(int num)
        {
            return num >= 0 && num < scanSize;
        }

        public class Cube
        {
            public int x, y, z;
            public bool isObsidian;
            public bool isAir;
            public Cube(int x, int y, int z, bool isObsidian, bool isAir)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.isObsidian = isObsidian;
                this.isAir = isAir;
            }
            public override string? ToString()
            {
                return String.Format("x: {0}, y: {1}, z: {2}", x, y, z);
            }

            public Cube[] GetInBoundsNeighbours()
            {
                return neighbourOffsets.Select(offset => (x: this.x + offset.x, y: this.y + offset.y, z: this.z + offset.z)).ToList()
                    .Where(offset => InBounds(offset.x) && InBounds(offset.y) && InBounds(offset.z))
                    .Select(offsetInBounds => GetCubeAt(offsetInBounds.x, offsetInBounds.y, offsetInBounds.z)).ToArray();
            }

            internal int GetOpenSides(bool airCheck)
            {
                return airCheck ? 
                    GetInBoundsNeighbours().Where(neighbour => neighbour.isAir).Count()
                    :
                    6 - GetInBoundsNeighbours().Where(neighbour => neighbour.isObsidian).Count();
            }

            public void ExpandIntoAir()
            {
                this.isAir = true;
                var airNeighbours = GetInBoundsNeighbours().Where(neighbour => !(neighbour.isObsidian) && !(neighbour.isAir));
                foreach (var neighbour in airNeighbours)
                {
                    neighbour.ExpandIntoAir();
                }
            }
        }
    }
}

