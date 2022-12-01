using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day20
{
    class Program
    {

        static string enhancementAlgorithm;

        static void Main(string[] args)
        {
            Image image;
            ReadInput(out enhancementAlgorithm, out image);
            for (int i = 0; i < 25; i++)
            {
                image.EnhanceImage('.');
                image.EnhanceImage('#');
            }
            Console.WriteLine("Lit pixels in image: " + image.contents.Select(line => line.Count(c => c == '#')).Sum());
        }


        private static void ReadInput(out string enhancementAlgorithm, out Image image)
        {
            var trimmedInput = System.IO.File.ReadAllLines(@"C:/workspace/AOC2021/day20/input.txt").Select(line => line.Trim('\n'));
            enhancementAlgorithm = trimmedInput.First();      // Take algorithm
            image = new Image(trimmedInput.Skip(2).ToList()); // Skip algorithm and blank line
        }

        class Image
        {
            public List<string> contents;

            public Image(List<string> contents)
            {
                this.contents = contents;
            }

            public int Width { get { return contents[0].Length; } }

            public int Height { get { return contents.Count; } }

            private void EnlargeImageBy(char c, int times)
            {
                string toInsert = string.Concat(Enumerable.Repeat(c, times));
                contents = contents.Select(line => line.Insert(0, toInsert) + toInsert).ToList();
                string newLine = string.Concat(Enumerable.Repeat(c, Width));
                for (int i = 0; i < times; i++)
                {
                    contents.Insert(0, newLine);
                    contents.Add(newLine);

                }
            }

            public void EnhanceImage(char c)
            {
                int padding = 2;
                int border = padding - 1;
                EnlargeImageBy(c, padding);
                Image enhancedImage = new Image(new List<string>(contents));

                for (int y = border; y < Height - border; y++)
                {
                    for (int x = padding - border; x < Width - border; x++)
                    {
                        string enhancedPixel = GetEnhancedPixel(GetEnhancementIndex(x, y));
                        enhancedImage.contents[y] = enhancedImage.contents[y].Remove(x, 1);
                        enhancedImage.contents[y] = enhancedImage.contents[y].Insert(x, enhancedPixel);
                    }
                }
                enhancedImage.ShrinkImageBy(border);
                contents = enhancedImage.contents;
            }

            public void ShrinkImageBy(int times)
            {
                for (int i = 0; i < times; i++)
                {
                    contents.RemoveAt(0);
                    contents.RemoveAt(contents.Count - 1);
                }
                contents = contents.Select(line => new string(line.Skip(times).SkipLast(times).ToArray())).ToList();
            }

            private string GetEnhancedPixel(int index)
            {
                return enhancementAlgorithm.ElementAt(index).ToString();
            }

            private int GetEnhancementIndex(int inX, int inY)
            {
                StringBuilder binaryString = new();
                for (int y = inY - 1; y <= inY + 1; y++)
                {
                    for (int x = inX - 1; x <= inX + 1; x++)
                    {
                        binaryString.Append(contents[y][x] == '#' ? 1 : 0);
                    }
                }
                return Convert.ToInt32(binaryString.ToString(), 2);

            }

            public override string ToString() // Prints image as a grid
            {
                StringBuilder result = new StringBuilder();
                for (int y = 0; y < Height; y++)
                {

                    for (int x = 0; x < Width; x++)
                    {
                        result.Append(contents[y][x]);
                    }
                    result.Append('\n');
                }
                return result.ToString();
            }
        }

    }
}
