using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;



namespace CountingWords
{
    class Program
    {
        static void Main(string[] args)
        {
            //files paths are given as command line parameters
            string path_input = args[0];
            string path_exclude = args[1];

            //Get a list of words which have to be excluded
            List<string> WordsToExclude = GetWordsFromExcludeList(path_exclude).ToList();

            //Get final text (with excluded words)
            string text = ExcludeWordsFromInputFile(WordsToExclude, path_input);

            //count words
            CountWordsStartingEachAlphabetLetter(text);

            Console.WriteLine("Output file is saved on Desktop");

        }



        public static IList<string> GetWordsFromExcludeList(string path_exclude)
        {
            List<string> WordsToExclude = new List<string>();

            using (StreamReader Reader = new StreamReader(path_exclude))
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append(Reader.ReadToEnd());
                {
                    //Words are case insensitive, punctuation should be ignored (period, comma etc.) 
                    string text = Regex.Replace(Sb.ToString(), @"[^\w\d\s]", " ").ToLower();

                    //split text into words and add into list
                    char[] delims = new[] { '\r', '\n', ' ' };
                    string[] words = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                    WordsToExclude.AddRange(words);

                    return WordsToExclude.Distinct().ToList();
                }
            }
        }

     


        public static string ExcludeWordsFromInputFile(List<string> WordsToExclude, string path_input)
        {
            using (StreamReader Reader = new StreamReader(path_input))
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append(Reader.ReadToEnd());
                {
                    //read text from input file
                    // Words are case insensitive, punctuation should be ignored(period, comma etc.)
                    string text = Sb.ToString().ToLower();
                    text = Regex.Replace(text.ToString(), @"[^\w\d\s]", " ");

                    //remove words from Task 2_exclude.txt file
                    foreach (string ExcludedWord in WordsToExclude)
                    {
                        text = Regex.Replace(text, @"\b" + Regex.Escape(ExcludedWord) + @"\b", "");
                    }

                     return text;
                }
            }
        }

        public static void CountWordsStartingEachAlphabetLetter(string text)
        {
            //split text into words
            char[] delims = new[] { '\r', '\n', ' ' };
            string[] words = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            //set chars for alphabet
            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            //set location to save output file
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filepath = path + "\\Task 2_output.txt";

            //Count the words starting with each letter of the alphabet
            foreach (var letter in alphabet)
            {

                var word = words.Where(x =>
                               x.StartsWith(letter.ToString(), StringComparison.OrdinalIgnoreCase))
                               .OrderBy(x => x);

                //write result in text file
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
                {
                    file.WriteLine(letter + " " + word.Count());
                    file.WriteLine(String.Join(" ", word));

                }

            }
           
        }
       
    }
}






