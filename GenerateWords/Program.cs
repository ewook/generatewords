using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateWords
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxWordLength = 6;
            char[] chars = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'å', 'ä', 'ö' };

            foreach (string word in GenerateWords(chars, maxWordLength))
            {
                Console.WriteLine(word);
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
