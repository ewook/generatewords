using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateWords
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxWordLength = 0;
            int maxLineCountPerFile = 0;

            if (args.Length != 2)
            {
                Console.WriteLine("Insufficient arguments.");
                Console.WriteLine("Please specify the following arguments:");
                Console.WriteLine("--max_word_length=<int>\tMaximum word length to generate.");
                Console.WriteLine("--max_line_count_per_file=<int>\tMaximum number of lines per file.");
                Console.WriteLine();
                Console.WriteLine("GenerateWords.exe --max_word_length=4 --max_line_count_per_file=1000000");
                Environment.Exit(-1);
            }

            foreach(string argument in args)
            {
                if (argument.StartsWith("--max_word_length="))
                {
                    string[] splitArg = argument.Split(new char[] { '=' });
                    if(!Int32.TryParse(splitArg[1],out maxWordLength))
                    {
                        Console.WriteLine("--max_word_length={0} is an invalid integer.", splitArg[1]);
                        Environment.Exit(-1);
                    }
                    continue;
                }
                if (argument.StartsWith("--max_line_count_per_file="))
                {
                    string[] splitArg = argument.Split(new char[] { '=' });
                    if (!Int32.TryParse(splitArg[1], out maxLineCountPerFile))
                    {
                        Console.WriteLine("--max_line_count_per_file={0} is an invalid integer.", splitArg[1]);
                        Environment.Exit(-1);
                    }
                    continue;
                }
            }

            if(maxWordLength == 0 || maxLineCountPerFile == 0)
            {
                Console.WriteLine("Parsing error. Bad coder.");
                Environment.Exit(-1);
            }

            Run(maxWordLength, maxLineCountPerFile);
        }

        static void Run(int maxWordLength, int maxLineCountPerFile)
        {
            int lineCountPerFile = 0;
            int fileWrittenCount = 0;
            char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'å', 'ä', 'ö' };

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            string nameOfFileToWrite = null;

            foreach (string word in GenerateWords(chars, maxWordLength))
            {
                if (lineCountPerFile == maxLineCountPerFile)
                {
                    streamWriter.Flush();
                    nameOfFileToWrite = string.Format("words_{0}.txt", fileWrittenCount);
                    using (FileStream fileToWrite = new FileStream(nameOfFileToWrite, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                    {
                        memoryStream.Position = 0;
                        memoryStream.CopyTo(fileToWrite);
                    }
                    fileWrittenCount++;
                    lineCountPerFile = 0;
                    memoryStream = new MemoryStream();
                    streamWriter = new StreamWriter(memoryStream);
                }
                streamWriter.WriteLine(word);
                lineCountPerFile++;
            }

            // write the remaining words
            streamWriter.Flush();
            nameOfFileToWrite = string.Format("words_{0}.txt", fileWrittenCount);
            using (FileStream fileToWrite = new FileStream(nameOfFileToWrite, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                memoryStream.Position = 0;
                memoryStream.CopyTo(fileToWrite);
            }
        }

        /// <summary>
        /// Thanks to https://stackoverflow.com/questions/24756924/how-to-generate-all-possible-words
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<String> GenerateWords(IEnumerable<char> characters, int length)
        {
            if (length > 0)
            {
                foreach (char c in characters)
                {
                    foreach (string suffix in GenerateWords(characters, length - 1))
                    {
                        yield return c + suffix;
                    }
                }
            }
            else
            {
                yield return string.Empty;
            }
        }
    }
}
