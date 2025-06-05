using TextGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text; // Добавлена эта строка

namespace GeneratorTests
{
    [TestClass]
    public class TokenGeneratorTests
    {
        private const string TestFilesDirectory = "TestData";

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(TestFilesDirectory);
            
            // Создаем тестовые файлы
            File.WriteAllText(Path.Combine(TestFilesDirectory, "bigrammweights.txt"), 
                "1 aa 10\n2 bb 20\n3 cc 30");
                
            File.WriteAllText(Path.Combine(TestFilesDirectory, "wordweights.txt"), 
                "1 word1 aa aa 10\n2 word2 bb bb 20\n3 word3 cc cc 30");
                
            File.WriteAllText(Path.Combine(TestFilesDirectory, "empty_file.txt"), "");
            
            File.WriteAllText(Path.Combine(TestFilesDirectory, "zero.txt"), "1 tt 0.0");
            
            File.WriteAllText(Path.Combine(TestFilesDirectory, "unicode.txt"), 
                "1 азбука aa aa 5.0", Encoding.UTF8);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(TestFilesDirectory))
            {
                Directory.Delete(TestFilesDirectory, true);
            }
        }

        [TestMethod]
        public void CharacterGenerator_ShouldReturnCorrectTokenCount()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "bigrammweights.txt"));
            Assert.AreEqual(3, generator.TokenCount);
        }

        [TestMethod]
        public void CharacterGenerator_ShouldReturnOnlyValidTokens()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "bigrammweights.txt"));
            var validTokens = new HashSet<string> { "aa", "bb", "cc" };

            var results = Enumerable.Range(0, 100)
                .Select(_ => generator.GetRandomToken())
                .ToList();

            CollectionAssert.AllItemsAreNotNull(results);
            Assert.IsTrue(results.All(validTokens.Contains));
        }

        [TestMethod]
        public void CharacterGenerator_ShouldRespectTokenWeights()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "bigrammweights.txt"));
            var stats = new Dictionary<string, int>();

            for (int i = 0; i < 10000; i++)
            {
                var token = generator.GetRandomToken();
                stats[token] = stats.GetValueOrDefault(token, 0) + 1;
            }

            Assert.IsTrue(stats["aa"] < stats["bb"]);
            Assert.IsTrue(stats["bb"] < stats["cc"]);
        }

        [TestMethod]
        public void CharacterGenerator_WithEmptyFile_ShouldHaveZeroTokens()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "empty_file.txt"));
            Assert.AreEqual(0, generator.TokenCount);
        }

        [TestMethod]
        public void CharacterGenerator_WithEmptyFile_ShouldReturnEmptyString()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "empty_file.txt"));
            Assert.AreEqual(string.Empty, generator.GetRandomToken());
        }

        [TestMethod]
        public void CharacterGenerator_WithZeroWeight_ShouldReturnEmptyString()
        {
            var generator = new CharacterGenerator(Path.Combine(TestFilesDirectory, "zero.txt"));
            Assert.AreEqual(string.Empty, generator.GetRandomToken());
        }

        [TestMethod]
        public void WordGenerator_ShouldReturnCorrectTokenCount()
        {
            var generator = new WordGenerator(Path.Combine(TestFilesDirectory, "wordweights.txt"));
            Assert.AreEqual(3, generator.TokenCount);
        }

        [TestMethod]
        public void WordGenerator_ShouldReturnOnlyValidTokens()
        {
            var generator = new WordGenerator(Path.Combine(TestFilesDirectory, "wordweights.txt"));
            var validTokens = new HashSet<string> { "word1", "word2", "word3" };

            var results = Enumerable.Range(0, 100)
                .Select(_ => generator.GetRandomToken())
                .ToList();

            CollectionAssert.AllItemsAreNotNull(results);
            Assert.IsTrue(results.All(validTokens.Contains));
        }

        [TestMethod]
        public void WordGenerator_ShouldHandleUnicodeTokens()
        {
            var generator = new WordGenerator(Path.Combine(TestFilesDirectory, "unicode.txt"));
            Assert.AreEqual("азбука", generator.GetRandomToken());
        }

        [TestMethod]
        public void SanityCheck_ShouldPassBasicMath()
        {
            Assert.AreEqual(2, 1 + 1);
        }
    }
}