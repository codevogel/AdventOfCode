using System.Diagnostics;
using System.Text;

class Program
{
	public static string LS = "$ ls";
	public static string CD = "$ cd";
	public static string BACK = "..";

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
		List<Directory> directories = new List<Directory>();
		Stack<Directory> directoryStack = new Stack<Directory>();

		while(input.Count() > 0)
		{
			string command = input.ElementAt(0);
			if (command.StartsWith(CD))
			{
				string dirName = command.Substring(CD.Length + 1);
				if (string.Equals(dirName, BACK))
				{
					// Go back one directory
					directoryStack.Pop();
				}
				else
				{
					// Add new directory
					Directory newDir = new Directory(dirName);
					directories.Add(newDir);

					// If stack is not empty
					if (directoryStack.Count > 0)
					{
						// Add new directory as sub directory of current directory
						directoryStack.Peek().SubDirectories[newDir.Name] = newDir;
					}
					// Push new directory onto the stack
					directoryStack.Push(newDir);
				}
				// Skip handled line
				input = input.Skip(1);
			}
			else if (command.StartsWith(LS))
			{
				var lsOutput = input.Skip(1).TakeWhile(line => !line.Contains('$'));
				var filesInThisDir = lsOutput.Where(line => char.IsDigit(line[0])).Select(line => line.Split(' '));
				// For all files in directory, add them as a subdirectory and set their file size as their name
				filesInThisDir.ToList().ForEach(file => directoryStack.Peek().SubDirectories[file[1]] = new Directory(file[0]));
				// Skip input to next command
				input = input.Skip(lsOutput.Count() + 1);
			}
		}
		// Solve A:
		Console.WriteLine("A" + directories.Select(directory => directory.FileSize).Where(fileSize => fileSize <= 100000).Sum());
		// Solve B:
		int availableSpace = 70000000 - directoryStack.Last().FileSize;
		int requiredSpace = 30000000;
		Console.WriteLine("B" + directories.Select(directory => directory.FileSize).Where(fileSize => availableSpace + fileSize >= requiredSpace).Min());
	}

	public class Directory
	{
		public string Name { get; set; }
		public Dictionary<string, Directory> SubDirectories { get; set; }
		public int FileSize { get => SubDirectories.Count == 0 ? int.Parse(Name) : SubDirectories.Values.Select(directory => directory.FileSize).Sum(); }
		public Directory(string name){ Name = name; SubDirectories = new(); }
	}

}