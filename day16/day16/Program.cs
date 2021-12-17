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
        public bool lengthType;
        public long value = long.MinValue;

        public Packet(IEnumerable<char> bitStream)
        {
            subpackets = new();

            version = Convert.ToInt32(new string(bitStream.Take(3).ToArray()), 2);
            typeID = (TypeID)Convert.ToInt32(new string(bitStream.Skip(3).Take(3).ToArray()), 2);

            bitStream = bitStream.Skip(6);
            bytesRead += 6;


            if (typeID == TypeID.LITERAL)
            {
                (long literal, int bytesRead) tmp = GetLiteral(bitStream);
                bytesRead += tmp.bytesRead;
                value = tmp.literal;
            }
            else
            {
                lengthType = bitStream.First() == '0';
                bitStream = bitStream.Skip(1);
                bytesRead += 1;

                if (lengthType)
                {
                    int lengthOfSubpackets = Convert.ToInt32(new string(bitStream.Take(15).ToArray()), 2);
                    bitStream = bitStream.Skip(15);
                    bytesRead += 15;

                    bytesRead += ExtractSubpacketsByLength(lengthOfSubpackets, bitStream);
                }
                else
                {
                    int numberOfSubpackets = Convert.ToInt32(new string(bitStream.Take(11).ToArray()), 2);
                    bitStream = bitStream.Skip(11);
                    bytesRead += 11;

                    bytesRead += ExtractSubpacketsByNumber(numberOfSubpackets, bitStream);
                }

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

        private (long literal, int bytesRead) GetLiteral(IEnumerable<char> data)
        {
            StringBuilder result = new();
            int bytesRead = 0;
            bool lastGroup = false;
            while (!lastGroup)
            {
                if (data.First() == '0')
                    lastGroup = true;
                result.Append(data.Skip(1).Take(4).ToArray());
                bytesRead += 5;
                data = data.Skip(5);
            }
            return (Convert.ToInt64(result.ToString(), 2), bytesRead);
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

