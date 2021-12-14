using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace day14
{
    class Program
    {

        static string polymer;
        static Dictionary<string, string> insertionDict;

        static Stopwatch stopwatch;

        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        private static void PartOne()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            ReadInput(out polymer, out insertionDict);

            for (int i = 0; i < 10; i++)
            {
                polymer = StepP1(polymer);
            }

            System.Console.WriteLine("Most minus least common element: " + MostMinusLeast(polymer));

            stopwatch.Stop();
            System.Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds.ToString() + " ms");
        }

        private static string StepP1(string polymer)
        {
            StringBuilder newPolymer = new StringBuilder().Append(polymer[0]);
            for (int i = 1; i < polymer.Length; i++)
            {
                char prevChar = polymer[i - 1], currentChar = polymer[i];
                string pair = new StringBuilder().Append(prevChar).Append(currentChar).ToString();
                if (insertionDict.ContainsKey(pair))
                {
                    newPolymer.Append(insertionDict[pair]);
                }
                newPolymer.Append(currentChar);
            }
            return newPolymer.ToString();
        }

        private static void PartTwo()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            ReadInput(out polymer, out insertionDict);

            Dictionary<string, long> occurances = GetOccurances(polymer);
            for (int i = 0; i < 40; i++)
            {

                occurances = GetOccurances(occurances);
            }
            System.Console.WriteLine(GetAnswer(occurances));

            stopwatch.Stop();
            System.Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds.ToString() + " ms");
        }


        private static string GetAnswer(Dictionary<string, long> occurances)
        {
            Dictionary<char, long> occurencesPerCharacter = new Dictionary<char, long>();


            foreach (string key in occurances.Keys)
            {
                foreach (char c in key)
                {
                    if (!occurencesPerCharacter.ContainsKey(c))
                    {
                        occurencesPerCharacter[c] = occurances[key];
                        continue;
                    }
                    occurencesPerCharacter[c] += occurances[key];
                }
            }

            occurencesPerCharacter[polymer[0]]++;
            occurencesPerCharacter[polymer[polymer.Length - 1]]++;

            foreach (char key in occurencesPerCharacter.Keys)
            {
                occurencesPerCharacter[key] = occurencesPerCharacter[key] / 2;
            }


            long max = occurencesPerCharacter.Values.Max();
            long min = occurencesPerCharacter.Values.Min();

            return "Most minus least common element: " + (max - min).ToString();
        }

        private static Dictionary<string, long> GetOccurances(Dictionary<string, long> occurances)
        {
            Dictionary<string, long> newOccurences = new();
            // For each type of occurence
            foreach (string key in occurances.Keys)
            {
                // Get character to be inserted for this pair
                char insertedChar = insertionDict[key][0];
                // Generate new pairs
                string[] newPairs = {
                      new StringBuilder(2).Append(key[0]).Append(insertedChar).ToString(),
                      new StringBuilder(2).Append(insertedChar).Append(key[1]).ToString()
                };

                // For each new pair
                foreach (string pair in newPairs)
                {
                    // If pair that we should count
                    if (insertionDict.ContainsKey(pair))
                    {
                        if (!newOccurences.ContainsKey(pair))
                        {
                            newOccurences[pair] = occurances[key];
                            continue;
                        }
                        newOccurences[pair] += occurances[key];
                    }
                }
            }
            return newOccurences;
        }

        private static Dictionary<string, long> GetOccurances(string polymer)
        {
            Dictionary<string, long> occurances = new Dictionary<string, long>();
            for (int i = 1; i < polymer.Length; i++)
            {
                string pair = polymer.Substring(i - 1, 2);
                if (insertionDict.ContainsKey(pair))
                {
                    if (!occurances.ContainsKey(pair))
                    {
                        occurances[pair] = 1;
                        continue;
                    }
                    occurances[pair]++;
                }
            }
            return occurances;
        }

        private static string MostMinusLeast(string polymer)
        {
            IEnumerable<(char key, int count)> numPerChar = polymer.GroupBy(c => c).Select(group => (group.Key, group.Count()));
            int most = numPerChar.Max(x => x.count);
            int least = numPerChar.Min(x => x.count);
            return (most - least).ToString();
        }



        private static void ReadInput(out string polymer, out Dictionary<string, string> insertionDict)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\workspace\AoC2021\day14\input.txt");
            polymer = lines[0].Trim('\n');
            insertionDict = lines.Skip(2).Select(line => line.Trim('\n').Split(" -> ")).ToDictionary(parts => parts[0], parts => parts[1]);
        }
    }
}
