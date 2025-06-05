﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextGenerator
{
    public abstract class TokenGenerator
    {
        protected readonly List<string> _tokens = new List<string>();
        protected readonly List<double> _weights = new List<double>();
        protected readonly List<double> _cumulativeWeights = new List<double>();
        protected double _totalWeight;
        protected readonly Random _random = new Random();

        public double TotalWeight => _totalWeight;
        
        public int TokenCount => _tokens.Count;

        public string GetRandomToken()
        {
            var randomValue = _random.Next(0, (int)_totalWeight);
            
            for (int i = 0; i < _cumulativeWeights.Count; i++)
            {
                if (randomValue <= _cumulativeWeights[i])
                {
                    return _tokens[i];
                }
            }
            
            return string.Empty;
        }

        public double GetTokenProbability(string token)
        {
            int index = _tokens.FindIndex(t => t == token);
            return index >= 0 ? _weights[index] : 0;
        }
    }

    public class CharacterGenerator : TokenGenerator
    {
        public CharacterGenerator(string dataFile)
        {
            string fullPath = GetFullPath(dataFile);
            
            foreach (string line in File.ReadLines(fullPath))
            {
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                
                _tokens.Add(parts[1]);
                double weight = double.Parse(parts[2]);
                _weights.Add(weight);
                
                _totalWeight += weight;
                _cumulativeWeights.Add(_totalWeight);
            }
        }
        
        private string GetFullPath(string relativePath)
        {
            return Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, 
                relativePath);
        }
    }

    public class WordGenerator : TokenGenerator
    {
        public WordGenerator(string dataFile)
        {
            string fullPath = GetFullPath(dataFile);
            
            foreach (string line in File.ReadLines(fullPath))
            {
                string[] parts = line.Replace('.', ',').Split(
                    new[] { ' ', '\t' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                
                _tokens.Add(parts[1]);
                double weight = double.Parse(parts[4]);
                _weights.Add(weight);
                
                _totalWeight += weight;
                _cumulativeWeights.Add(_totalWeight);
            }
        }
        
        private string GetFullPath(string relativePath)
        {
            return Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, 
                relativePath);
        }
    }

    public class GeneratorStatistics
    {
        public static void ProcessBigrams(string inputFile, string outputDirectory)
        {
            var generator = new CharacterGenerator(inputFile);
            var frequencyMap = new SortedDictionary<string, int>();
            var generatedText = new StringBuilder();
            
            for (int i = 0; i < 1000; i++)
            {
                string token = generator.GetRandomToken();
                generatedText.Append(token);
                
                if (frequencyMap.ContainsKey(token))
                    frequencyMap[token]++;
                else
                    frequencyMap[token] = 1;
                
                Console.Write(token);
            }
            
            Console.WriteLine();
            
            WriteResults(
                Path.Combine(outputDirectory, "gen-1.txt"),
                generatedText.ToString());
            
            WriteStatistics(
                Path.Combine(outputDirectory, "graph_bi_data.txt"),
                frequencyMap,
                generator);
        }

        public static void ProcessWords(string inputFile, string outputDirectory)
        {
            var generator = new WordGenerator(inputFile);
            var frequencyMap = new SortedDictionary<string, int>();
            var generatedText = new StringBuilder();
            
            for (int i = 0; i < 1000; i++)
            {
                string token = generator.GetRandomToken();
                generatedText.Append(token).Append(' ');
                
                if (frequencyMap.ContainsKey(token))
                    frequencyMap[token]++;
                else
                    frequencyMap[token] = 1;
                
                Console.Write(token + " ");
            }
            
            Console.WriteLine();
            
            WriteResults(
                Path.Combine(outputDirectory, "gen-2.txt"),
                generatedText.ToString());
            
            WriteStatistics(
                Path.Combine(outputDirectory, "graph_word_data.txt"),
                frequencyMap,
                generator);
        }

        private static void WriteResults(string filePath, string content)
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        private static void WriteStatistics(
            string filePath,
            SortedDictionary<string, int> frequencies,
            TokenGenerator generator)
        {
            var lines = frequencies.Select(pair => 
                $"{pair.Key} {pair.Value / 1000.0} {generator.GetTokenProbability(pair.Key) / generator.TotalWeight}");
            
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string resultsDirectory = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, 
                "Results");
            
            GeneratorStatistics.ProcessBigrams("bigrammweights.txt", resultsDirectory);
            GeneratorStatistics.ProcessWords("wordweights.txt", resultsDirectory);
        }
    }
}