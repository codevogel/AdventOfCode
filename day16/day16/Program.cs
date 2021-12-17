using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day16
{
    class Program
    {

        static IEnumerable<char> bitStream;
        static void Main(string[] args)
        {
            bitStream = string.Join("", Convert.FromHexString(System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day16\input.txt")[0].Trim('\n')).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))).ToArray();
            Packet packet = new Packet(bitStream);

            Console.WriteLine("Sum of packet versions: " + GetVersionSum(packet));
            Console.WriteLine("Packet value: " + packet.value);
        }

        private static int GetVersionSum(Packet packet)
        {
            int sum = packet.version;
            foreach (Packet subpacket in packet.subpackets)
            {
                sum += GetVersionSum(subpacket);
            }
            return sum;
        }
    }

    internal class Packet
    {
        public int bytesRead;
        public List<Packet> subpackets;
        public int version;
        public TypeID typeID;
        public bool lengthType; // number of packets type when false
        public long value = long.MinValue;
        public IEnumerable<char> bitStream;

        public Packet(IEnumerable<char> _bitStream)
        {
            this.bitStream = _bitStream;
            subpackets = new();

            // Read header
            version = Convert.ToInt32(new string(ReadStream(3).ToArray()), 2);
            typeID = (TypeID)Convert.ToInt32(new string(ReadStream(3).ToArray()), 2);

            // Parse data
            if (typeID == TypeID.LITERAL)
            {
                StringBuilder result = new();
                bool readLastGroup = false;
                while (!readLastGroup)
                {
                    if (ReadStream(1).First() == '0')
                        readLastGroup = true;
                    result.Append(ReadStream(4).ToArray());
                }
                value = Convert.ToInt64(result.ToString(), 2);
            }
            else
            {
                if (ReadStream(1).First() == '0') // Extract subpackets by length
                {
                    int lengthOfSubpackets = Convert.ToInt32(new string(ReadStream(15).ToArray()), 2);
                    bytesRead += ExtractSubpacketsByLength(lengthOfSubpackets, bitStream);
                }
                else // Extract subpackets by number
                {
                    int numberOfSubpackets = Convert.ToInt32(new string(ReadStream(11).ToArray()), 2);
                    bytesRead += ExtractSubpacketsByNumber(numberOfSubpackets, bitStream);
                }

                // Get value based on subvalues
                var subValues = subpackets.Select(packet => packet.value);
                switch (typeID)
                {
                    case TypeID.SUM:
                        value = subValues.Sum();
                        break;
                    case TypeID.PRODUCT:
                        value = subValues.Aggregate((long)1, (x, y) => x * y);
                        break;
                    case TypeID.MINIMUM:
                        value = subValues.Min();
                        break;
                    case TypeID.MAXIMUM:
                        value = subValues.Max();
                        break;
                    case TypeID.GREATER_THAN:
                        value = subValues.First() > subValues.Last() ? 1 : 0;
                        break;
                    case TypeID.LESS_THAN:
                        value = subValues.First() < subValues.Last() ? 1 : 0;
                        break;
                    case TypeID.EQUALS:
                        value = subValues.First() == subValues.Last() ? 1 : 0;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private IEnumerable<char> ReadStream(int amount)
        {
            IEnumerable<char> result = bitStream.Take(amount);
            bytesRead += amount;
            bitStream = bitStream.Skip(amount);
            return result;
        }

        private int ExtractSubpacketsByNumber(int numberOfSubpackets, IEnumerable<char> bitStream)
        {
            int packetsAdded = 0;
            while (packetsAdded++ < numberOfSubpackets)
            {
                subpackets.Add(new Packet(bitStream));
                bitStream = bitStream.Skip(subpackets.Last().bytesRead);

            }

            return subpackets.Select(packet => packet.bytesRead).Sum();
        }

        private int ExtractSubpacketsByLength(int lengthOfSubpackets, IEnumerable<char> bitStream)
        {
            int bytesRead = 0;
            while (bytesRead < lengthOfSubpackets)
            {
                Packet subpacket = new Packet(bitStream);
                subpackets.Add(subpacket);

                bitStream = bitStream.Skip(subpacket.bytesRead);
                bytesRead += subpacket.bytesRead;
            }
            return bytesRead;
        }
    }

    enum TypeID
    {
        SUM = 0,
        PRODUCT = 1,
        MINIMUM = 2,
        MAXIMUM = 3,
        LITERAL = 4,
        GREATER_THAN = 5,
        LESS_THAN = 6,
        EQUALS = 7

    }
}

