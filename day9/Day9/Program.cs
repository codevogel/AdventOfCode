// By Kamiel de Visser

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = ReadInput(@"C:/workspace/AoC2021/day9/input.txt");
            List<int> nums;
            ProcessInput(input, out nums);

            PartTwo(nums);
        }

        private static void PartOne(List<int> fishies)
        {
            // For each day...
            for (int days = 1; days <= 80; days++)
            {
                // For each fish...
                for (int fishIndex = 0; fishIndex < fishies.Count; fishIndex++)
                {
                    // If fish will reproduce
                    if (fishies[fishIndex] - 1 < 0)
                    {
                        // Reproduce
                        fishies.Add(9);
                        fishies[fishIndex] = 6;
                    }
                    else
                    {
                        // Just grow old :(
                        fishies[fishIndex] -= 1;
                    }
                }
            }
        }

        private static void PartTwo(List<int> fishies)
        {
            // Use dict composed of  <days to reproduce, amount of fish with that 'age'>
            // Use longs to prevent integer overflow
            Dictionary<long, long> fishDict = new Dictionary<long, long>();

            // For each fish...
            for (int i = 0; i < fishies.Count; i++)
            {
                // Place age of fish as key
                // Increment amount of fish per fish with that age
                AddOrIncrementDict(fishDict, fishies[i], 1);
            }


            // For each day...
            for (int days = 1; days <= 256; days++)
            {
                // Create dict for next day
                Dictionary<long, long> nextDayDict = new Dictionary<long, long>();

                // For each age group of fish
                foreach (long dayToReproduce in fishDict.Keys.ToArray())
                {
                    // Get amount of fish in age group
                    long amountOfFishThisAge = fishDict[dayToReproduce];
                    // Get next age
                    long nextAge = dayToReproduce - 1;

                    // If next age just grows old
                    if (nextAge >= 0)
                    {
                        // Grow old
                        AddOrIncrementDict(nextDayDict, nextAge, amountOfFishThisAge);
                    }
                    else
                    {
                        // Reproduce
                        AddOrIncrementDict(nextDayDict, 8, amountOfFishThisAge);
                        AddOrIncrementDict(nextDayDict, 6, amountOfFishThisAge);
                    }
                }

                // Reassign fishdict to dict of next day
                fishDict = nextDayDict;
            }

            // Sum the fish
            long sum = 0;
            foreach (long amountOfFish in fishDict.Values)
            {
                sum += amountOfFish;
            }
            System.Console.WriteLine("Fish after days: " + sum);
        }

        private static void AddOrIncrementDict(Dictionary<long, long> dict, long key, long amount)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += amount;
            }
            else
            {
                dict.Add(key, amount);
            }
        }


        /// <summary>
        /// Parse input file into string array
        /// </summary>
        private static string[] ReadInput(string filePath)
        {
            string[] input = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = input[i].Trim('\n');
            }
            return input;
        }


        /// <summary>
        /// Load data from string array
        /// </summary>
        private static void ProcessInput(string[] input, out List<int> nums)
        {
            nums = new List<int>();
            foreach (string inputLine in input)
            {
                if (string.IsNullOrEmpty(inputLine))
                {
                    continue;
                }
                else
                {
                    string[] numberStrings = inputLine.Split(',');

                    foreach (string numberString in numberStrings)
                    {
                        nums.Add(int.Parse(numberString));
                    }
                }
            }
        }
    }
