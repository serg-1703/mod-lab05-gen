﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace generator
{
    public abstract class Generator
    {
        protected List<string> tokens = new List<string>();
        protected List<double> weights = new List<double>();
        protected List<double> upper_bounds = new List<double>();
        protected double sum;
        protected Random random = new Random();

        public double Sum { get { return sum; } }
        public string getToken()
        {
            var rnd = random.Next(0, (Int32)sum);
            for (int i = 0; i < upper_bounds.Count; i++)
            {
                if (rnd <= upper_bounds[i])
                {
                    return tokens[i];
                }
            }
            return "";
        }

        public int getSize()
        {
            return tokens.Count;
        }
        public double getTokenWeight(string sym) => weights[tokens.FindIndex(x => x == sym)];
    }
    public class CharGenerator : Generator
    {
        public CharGenerator(string filename)
        {
            string path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, filename);
            using (StreamReader reader = new StreamReader(path))
            {
                string input;
                string[] line;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    tokens.Add(line[i + 1]);
                    weights.Add(Double.Parse(line[2]));
                    sum += Double.Parse(line[2]);
                    upper_bounds.Add(sum);
                }
            }
        }
    }
    public class WordGenerator : Generator
    {
        public WordGenerator(string filename)
        {
            string path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, filename);
            using (StreamReader reader = new StreamReader(path))
            {
                string input;
                string[] line;
                while ((input = reader.ReadLine()) != null)
                {
                    line = input.Replace('.', ',').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    tokens.Add(line[1]);
                    weights.Add(Double.Parse(line[4]));
                    sum += Double.Parse(line[4]);
                    upper_bounds.Add(sum);
                }
            }
        }
    }
    class Program
    {
        static void BigramProcessing(string filename, string pathRes)
        {
            CharGenerator gen = new CharGenerator(filename);
            SortedDictionary<string, int> stat = new SortedDictionary<string, int>();
            string result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = gen.getToken();
                result += ch;
                if (stat.ContainsKey(ch))
                    stat[ch]++;
                else
                    stat.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');

            using (StreamWriter writer = new StreamWriter(pathRes + "gen-1.txt", false, Encoding.UTF8))
            {
                writer.WriteLine(result);
            }

            using (StreamWriter writer = new StreamWriter((pathRes + "graph_bi_data.txt"), true, Encoding.UTF8))
            {
                foreach (KeyValuePair<string, int> entry in stat)
                {
                    writer.WriteLine(entry.Key + " " + (entry.Value / 1000.0).ToString() + " " +
                        ((Double)gen.getTokenWeight(entry.Key) / gen.Sum).ToString());
                }
            }
        }

        static void WordProcessing(string filename, string pathRes)
        {
            WordGenerator genWord = new WordGenerator(filename);
            SortedDictionary<string, int> statWord = new SortedDictionary<string, int>();
            string result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = genWord.getToken();
                result += ch + " ";
                if (statWord.ContainsKey(ch))
                    statWord[ch]++;
                else
                    statWord.Add(ch, 1); Console.Write(ch + " ");
            }
            Console.Write('\n');
            using (StreamWriter writer = new StreamWriter((pathRes + "gen-2.txt"), false, Encoding.UTF8))
            {
                writer.WriteLine(result);
            }

            using (StreamWriter writer = new StreamWriter((pathRes + "graph_word_data.txt"), true, Encoding.UTF8))
            {
                foreach (KeyValuePair<string, int> entry in statWord)
                {
                    writer.WriteLine(entry.Key + " " + (entry.Value / 1000.0).ToString() + " " +
                        ((Double)genWord.getTokenWeight(entry.Key) / genWord.Sum).ToString());
                }
            }

        }

        static void Main(string[] args)
        {
            string pathRes = Path.Combine(Directory.GetParent(
                Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "Results/");
            BigramProcessing("bigrammweights.txt", pathRes);
            WordProcessing("wordweights.txt", pathRes);
        }
    }
}