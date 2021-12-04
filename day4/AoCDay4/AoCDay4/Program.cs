using System.Collections.Generic;
using System.Linq;

namespace AoCDay4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Convert input file to data
            string[] input = ReadInput(@"C:/workspace/AoC2021/day4/input.txt");
            Queue<int> ballQueue;
            List<Board> boards;
            ProcessInput(input, out boards, out ballQueue);


            // Keep track of last winning board and ball
            int lastWinningBall = -1;
            Board lastWinningBoard = null;

            bool ansFound = false;
            // While balls are in queue
            while (ballQueue.Count > 0)
            {
                // Dequeue ball
                int ball = ballQueue.Dequeue();
                System.Console.WriteLine("Drawed ball " + ball);

                // Check boards for bingo based on ball  
                foreach (Board board in boards)
                {
                    if (!board.Won)
                    {
                        board.MarkTile(ball);
                        if (board.Bingo)
                        {
                            // Store won board
                            board.Won = true;
                            ansFound = true;
                            lastWinningBoard = board;
                            lastWinningBall = ball;
                        }
                    }
                }
            }


            if (!ansFound)
            {
                System.Console.WriteLine("Didn't find any bingoes :(");
            }
            else
            {
                PrintBoardScore(lastWinningBoard, lastWinningBall);
            }
        }

        /// <summary>
        /// Prints the score of a board based on currentBall
        /// </summary>
        private static void PrintBoardScore(Board board, int currentBall)
        {

            int sum = 0;

            // Sum unmarked tile values
            foreach (Board.BoardTile tile in board.tiles)
            {
                if (!tile.Marked)
                {
                    sum += tile.Value;
                }
            }

            // Print sum
            System.Console.WriteLine("Answer: " + sum * currentBall);
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
        private static void ProcessInput(string[] input, out List<Board> boards, out Queue<int> ballQueue)
        {
            // First line is ball queue
            string[] ballQueueParts = input[0].Trim('\n').Split(',');
            ballQueue = new Queue<int>();
            for (int i = 0; i < ballQueueParts.Length; i++)
            {
                ballQueue.Enqueue(int.Parse(ballQueueParts[i]));
            }

            // Skip to first line of board
            input = input.Skip(2).ToArray();

            boards = new List<Board>();

            // While boards remain in the input array
            while (input.Length >= 6)
            {
                // Create a new board 
                string[] boardInput = input.Take(5).ToArray();
                boards.Add(new Board(boardInput));
                // Throw out created board data from input
                input = input.Skip(6).ToArray();
            }
        }

        class Board
        {

            public bool Won { get; set; }

            public BoardTile[,] tiles = new BoardTile[5, 5];

            /// <summary>
            /// Creates a board object by parsing its input lines
            /// </summary>
            public Board(string[] lines)
            {
                Won = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Replace("  ", " ").Trim('\n').Trim(' ').Split(' ');
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(parts[j]))
                            continue;
                        tiles[i, j] = new BoardTile(int.Parse(parts[j]), false);
                    }
                }
            }

            /// <summary>
            /// Marks a tile based on a drawn ball
            /// </summary>
            public void MarkTile(int ball)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        BoardTile tile = (tiles[i, j]);
                        if (tile.Value == ball)
                        {
                            tile.Marked = true;
                            tiles[i, j] = tile;
                            return;
                        }
                    }
                }
            }


            public bool Bingo
            {
                get
                {
                    for (int i = 0; i < 5; i++)
                    {
                        // Check rows and columns
                        if (CheckBingo(GetColumn(tiles, i)))
                            return true;
                        if (CheckBingo(GetRow(tiles, i)))
                            return true;

                    }
                    return false;
                }
            }

            /// <summary>
            /// Checks whether all tiles in the row or column are marked
            /// </summary>
            private bool CheckBingo(BoardTile[] rowOrColumn)
            {
                bool allMarked = true;
                foreach (BoardTile tile in rowOrColumn)
                {
                    if (!tile.Marked)
                    {
                        allMarked = false;
                    }
                }
                return allMarked;
            }

            /// <summary>
            /// Represents a board tile
            /// </summary>
            public struct BoardTile
            {
                public int Value { get; set; }
                public bool Marked { get; set; }

                public BoardTile(int value, bool marked)
                {
                    Value = value;
                    Marked = marked;
                }
            }

            /// <summary>
            /// Gets a BoardTile column from the 2d array
            /// </summary>
            public BoardTile[] GetColumn(BoardTile[,] matrix, int columnNumber)
            {
                return Enumerable.Range(0, matrix.GetLength(0))
                        .Select(x => matrix[x, columnNumber])
                        .ToArray();
            }

            /// <summary>
            /// Gets a BoardTile row from the 2d array
            /// </summary>
            public BoardTile[] GetRow(BoardTile[,] matrix, int rowNumber)
            {
                return Enumerable.Range(0, matrix.GetLength(1))
                        .Select(x => matrix[rowNumber, x])
                        .ToArray();
            }
        }
    }
}
