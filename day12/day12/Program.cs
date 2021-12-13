using System;
using System.Collections.Generic;
using System.Linq;

namespace day12
{
    class Program
    {

        static Dictionary<string, Node> nodeDict;

        static void Main(string[] args)
        {
            PartOne();

            PartTwo();
        }

        private static void PartOne()
        {
            nodeDict = ReadInput();
            int numPaths = GetNumberOfPathsTo(nodeDict["start"], nodeDict["end"], currentSum: 0, visited: new Stack<Node>());

            Console.WriteLine("Number of paths: " + numPaths);
        }
        private static void PartTwo()
        {
            nodeDict = ReadInput();
            int numPaths = GetNumberOfPathsToAlt(nodeDict["start"], nodeDict["end"], currentSum: 0, visited: new Stack<Node>());

            Console.WriteLine("Number of paths: " + numPaths);
        }

        private static int GetNumberOfPathsTo(Node currentNode, Node endNode, int currentSum, Stack<Node> visited)
        {
            visited.Push(currentNode);

            if (currentNode.ID == endNode.ID)
            {
                currentSum++;
            }
            else
            {
                foreach (Node connectingNode in currentNode.Connections)
                {
                    bool smallCave = char.IsLower(connectingNode.ID[0]);
                    if (smallCave && visited.Where(node => visited.Contains(connectingNode)).Count() > 1)
                    {
                        continue;
                    }
                    currentSum = GetNumberOfPathsTo(connectingNode, endNode, currentSum, new Stack<Node>(visited));
                }
            }
            return currentSum;
        }

        private static int GetNumberOfPathsToAlt(Node currentNode, Node endNode, int currentSum, Stack<Node> visited)
        {
            visited.Push(currentNode);

            var smallCaveVisitedTwice = visited
            .Where(x => x.ID != "start" && x.ID != "end" && char.IsLower(x.ID[0]))
            .GroupBy(x => x.ID)
            .FirstOrDefault(x => x.Count() == 2)
            ?.FirstOrDefault();

            if (currentNode.ID == endNode.ID)
            {
                currentSum++;
            }
            else
            {
                foreach (Node connectingNode in currentNode.Connections)
                {
                    bool smallCave = char.IsLower(connectingNode.ID[0]);
                    if (connectingNode.ID == "start" ||
                        (smallCaveVisitedTwice != null && smallCave && visited.Contains(connectingNode)))
                    {
                        continue;
                    }
                    currentSum = GetNumberOfPathsToAlt(connectingNode, endNode, currentSum, new Stack<Node>(visited));
                }
            }
            return currentSum;
        }

        private static Dictionary<string, Node> ReadInput()
        {
            Dictionary<string, Node> connections = new Dictionary<string, Node>();
            string[][] input = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day12\input.txt").Select(line => line.Trim('\n').Split('-')).ToArray();
            Array.ForEach(input, parts => FillDict(connections, parts));
            Array.ForEach(input, parts => AddConnections(connections, parts));
            return connections;
        }

        private static void AddConnections(Dictionary<string, Node> connections, string[] parts)
        {
            Node leftNode = connections[parts[0]];
            Node rightNode = connections[parts[1]];
            connections[leftNode.ID].Connections.Add(rightNode);
            connections[rightNode.ID].Connections.Add(leftNode);

        }

        private static void FillDict(Dictionary<string, Node> connections, string[] parts)
        {
            Node leftNode = new Node(parts[0]);
            Node rightNode = new Node(parts[1]);
            if (!connections.ContainsKey(leftNode.ID))
                connections[leftNode.ID] = leftNode;
            if (!connections.ContainsKey(rightNode.ID))
                connections[rightNode.ID] = rightNode;
        }
    }

    struct Node
    {
        public string ID { get; set; }

        public List<Node> Connections { get; set; }

        public Node(string id)
        {
            ID = id;
            Connections = new List<Node>();
        }
    }
}

