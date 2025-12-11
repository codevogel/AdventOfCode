using System.Diagnostics;

namespace day11
{
   public class Solver
   {
      static readonly Dictionary<Server, int> serverToIdMap = [];

      public static void Main()
      {
         List<string> input = ParseInput("input.txt");
         Stopwatch stopwatch = new();
         stopwatch.Start();

         Server? youServer = null;
         Server? svrServer = null;
         List<Server> servers =
         [
            .. input
               .Select((line, i) => (parts: line.Split(), id: i))
               .Select(partAndId =>
               {
                  string selfName = string.Join("", partAndId.parts[0].SkipLast(1));
                  string[] outputs = [.. partAndId.parts.Skip(1)];
                  Server server = new(partAndId.id, selfName, outputs);
                  serverToIdMap[server] = server.ID;
                  if (server.Name == "you")
                     youServer = server;
                  if (server.Name == "svr")
                     svrServer = server;
                  return server;
               }),
         ];

         // Create the outserver as it is not in the input list
         Server outServer = new(servers.Count, "out", []);
         servers.Add(outServer);
         serverToIdMap[outServer] = outServer.ID;

         // Generate a list of ID's that each server points to rather than using the names
         servers.ToList().ForEach(server => server.GenerateOutputs());

         Console.WriteLine($"A: {youServer!.GetNPathsTo(outServer!)}");
         stopwatch.Stop();
         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");

         stopwatch.Restart();
         Console.WriteLine($"B: {GetNProblemPathsFromAToB(svrServer!, outServer!)}");
         stopwatch.Stop();
         Console.WriteLine($"Solved in {stopwatch.ElapsedMilliseconds}ms");
      }

      private static long GetNProblemPathsFromAToB(Server a, Server b)
      {
         var memo = new Dictionary<(int ID, bool PassedDAC, bool passedFFT), long>();
         long count = a.GetNProblemPathsTo(b, visitedDAC: false, visitedFFT: false, memo);
         return count;
      }

      class Server(int id, string name, string[] outputs)
      {
         public int ID { get; } = id;
         public string Name { get; } = name;
         public string[] OutputsString { get; } = outputs;
         public Server[] Outputs { get; private set; } = [];

         public void GenerateOutputs()
         {
            Outputs =
            [
               .. OutputsString.Select(outString =>
                  serverToIdMap.Keys.First(server => server.Name == outString)
               ),
            ];
         }

         public int GetNPathsTo(Server targetServer)
         {
            // If we hit the target server, found a path
            if (targetServer.ID == ID)
               return 1;

            if (Outputs.Length == 0)
               // This shouldn't happen as only the target server (out) has 0 paths
               return 0;

            // Sum the paths that reach the targetServer
            int n = 0;
            foreach (Server output in Outputs)
               n += output.GetNPathsTo(targetServer);
            return n;
         }

         public long GetNProblemPathsTo(
            Server target,
            bool visitedDAC,
            bool visitedFFT,
            Dictionary<(int ID, bool PassedDAC, bool PassedFFT), long> totalMemo
         )
         {
            bool passedDAC = visitedDAC ? visitedDAC : Name == "dac";
            bool passedFFT = visitedFFT ? visitedFFT : Name == "fft";

            // If target is reached
            if (ID == target.ID)
               // Only return 1 when we passed both DAC and FFT
               return (passedDAC && passedFFT) ? 1 : 0;

            // Try to get the value from the current server given the current passed state
            // In that case, skip further processing
            (int ID, bool PassedDAC, bool PassedFFT) memoKey = (ID, passedDAC, passedFFT);
            if (totalMemo.TryGetValue(memoKey, out var cachedValue))
               return cachedValue;

            // Mark current server as being processed

            // Sum the paths that reach the target server that include both DAC and FFT
            long total = 0;
            foreach (var output in Outputs)
            {
               total += output.GetNProblemPathsTo(target, passedDAC, passedFFT, totalMemo);
            }

            // We're done visiting this server
            // Store the total so we dont have to calculate it again later
            totalMemo[memoKey] = total;
            return total;
         }
      }

      static List<string> ParseInput(string path) => [.. File.ReadLines(path)];
   }
}
