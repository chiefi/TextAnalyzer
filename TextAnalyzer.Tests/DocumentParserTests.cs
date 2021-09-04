using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TextAnalyzer.Parser;

namespace TextAnalyzer.Tests
{
    [TestClass]
    public class DocumentParserTests
    {
        [TestMethod]
        public void TestParseEmptyDocument()
        {
            var filterService = new FilterService();
            var parser = new DocumentParser(filterService);

            string[] text = null;
            var task = parser.ParseAsync("Fail", text);
            task.Wait();
            Assert.IsFalse(task.Result.Successful);

            string[] text2 = new string[0];
            var task2 = parser.ParseAsync("Empty", text2);
            task2.Wait();
            Assert.IsFalse(task2.Result.Successful);
        }

        [TestMethod]
        public void TestParseSampleDocument()
        {
            var filterService = new FilterService();
            var parser = new DocumentParser(filterService);

            var source = "Texts/Sample.txt";
            var text = File.ReadAllLines(source);

            var task = parser.ParseAsync(source, text);
            task.Wait();
            var result = task.Result;
            Assert.IsTrue(result.Successful);
            Assert.AreEqual(29, result.Data.NumberOfLines);
            Assert.AreEqual(239, result.Data.NumberOfWords);

            var longestWords = result.Data.GetLongestWords(5);
            Assert.AreEqual("protuberant", longestWords[0]);
            Assert.AreEqual("12.06.2016", longestWords[1]);
            Assert.AreEqual("everything", longestWords[2]);
            Assert.AreEqual("up-to-date", longestWords[3]);
            Assert.AreEqual("evidently", longestWords[4]);

            var mostFrequentWords = result.Data.GetTopWords(5);
            Assert.AreEqual("the", mostFrequentWords[0].Word);
            Assert.AreEqual(15, mostFrequentWords[0].Frequency);
            Assert.AreEqual("i", mostFrequentWords[1].Word);
            Assert.AreEqual(9, mostFrequentWords[1].Frequency);
            Assert.AreEqual("and", mostFrequentWords[2].Word);
            Assert.AreEqual(8, mostFrequentWords[2].Frequency);
            Assert.AreEqual("a", mostFrequentWords[3].Word);
            Assert.AreEqual(7, mostFrequentWords[3].Frequency);
            Assert.AreEqual("of", mostFrequentWords[4].Word);
            Assert.AreEqual(7, mostFrequentWords[4].Frequency);
        }

        [TestMethod]
        public void TestParseFullDocument()
        {
            var filterService = new FilterService();
            var parser = new DocumentParser(filterService);

            var source = "Texts/45839.txt";
            var text = File.ReadAllLines(source);

            var task = parser.ParseAsync(source, text);
            task.Wait();
            var result = task.Result;
            Assert.IsTrue(result.Successful);
            Assert.AreEqual(15722, result.Data.NumberOfLines);
            Assert.AreEqual(164829, result.Data.NumberOfWords);

            var longestWords = result.Data.GetLongestWords(5);
            Assert.AreEqual("two-pages-to-the-week-with-sunday-squeezed-in-a-corner", longestWords[0]);
            Assert.AreEqual("mentally-accomplished", longestWords[1]);
            Assert.AreEqual("brother-professional", longestWords[2]);
            Assert.AreEqual("splendidly-coloured", longestWords[3]);
            Assert.AreEqual("burdon-sanderson's", longestWords[4]);

            var mostFrequentWords = result.Data.GetTopWords(5);
            Assert.AreEqual("the", mostFrequentWords[0].Word);
            Assert.AreEqual(8070, mostFrequentWords[0].Frequency);
            Assert.AreEqual("and", mostFrequentWords[1].Word);
            Assert.AreEqual(5979, mostFrequentWords[1].Frequency);
            Assert.AreEqual("i", mostFrequentWords[2].Word);
            Assert.AreEqual(4793, mostFrequentWords[2].Frequency);
            Assert.AreEqual("to", mostFrequentWords[3].Word);
            Assert.AreEqual(4547, mostFrequentWords[3].Frequency);
            Assert.AreEqual("of", mostFrequentWords[4].Word);
            Assert.AreEqual(3740, mostFrequentWords[4].Frequency);
        }

        [TestMethod]
        public void TestRowFilter()
        {
            var filterService = new FilterService();


            var words = new [] { "jesus", "lives", "on", "the", "street" };
            Assert.IsTrue(IsEqual(words, filterService.FilterRow("Jesus lives on the street.")));
            Assert.IsTrue(IsEqual(words, filterService.FilterRow("Jesus lives on the street!")));
            Assert.IsTrue(IsEqual(words, filterService.FilterRow("Jesus lives, on the street.")));
            Assert.IsTrue(IsEqual(words, filterService.FilterRow("Jesus lives on the (street).")));

            var expected1 = new[] { "hitherto", "i", "had", "noticed", "the", "backs", "of", "his", "hands", "as", "they", "lay", "on", "his", "knees" };
            var actual1 = filterService.FilterRow("Hitherto I had noticed the backs of his hands as they lay on his knees");
            Assert.IsTrue(IsEqual(expected1, actual1));

            var expected2 = new[] { "in", "the", "firelight", "and", "they", "had", "seemed", "rather", "white", "and", "fine", "but", "seeing" };
            var actual2 = filterService.FilterRow("in the firelight, and they had seemed rather white and fine; but seeing");
            Assert.IsTrue(IsEqual(expected2, actual2));

            var expected3 = new[] { "coarse", "broad", "with", "squat", "fingers", "strange", "to", "say", "there", "were", "hairs" };
            var actual3 = filterService.FilterRow("coarse--broad, with squat fingers. Strange to say, there were hairs");
            Assert.IsTrue(IsEqual(expected3, actual3));

            var expected4 = new[] { "the", "fireplace", "we", "were", "both", "silent", "for", "a", "while", "and", "as", "i", "looked", "towards" };
            var actual4 = filterService.FilterRow("the fireplace. We were both silent for a while; and as I looked towards");
            Assert.IsTrue(IsEqual(expected4, actual4));

            var expected5 = new[] { "from", "down", "below", "in", "the", "valley", "the", "howling", "of", "many", "wolves", "the", "count's" };
            var actual5 = filterService.FilterRow("from down below in the valley, the howling of many wolves. The Count's");
            Assert.IsTrue(IsEqual(expected5, actual5));

            var expected6 = new[] { "over", "30,000", "people", "cheered", "at", "9.39am", "on", "the", "12.06.2016" };
            var actual6 = filterService.FilterRow("Over 30,000 people cheered at 9.39am on the 12.06.2016.");
            Assert.IsTrue(IsEqual(expected6, actual6));

            var expected7 = new[] { "on", "12th", "july", "55%", "or", "55", "percent", "was", "up-to-date" };
            var actual7 = filterService.FilterRow("On 12th july 55% or 55 percent was up-to-date.");
            Assert.IsTrue(IsEqual(expected7, actual7));
        }

        private bool IsEqual(string[] first, string[] second)
        {
            return first.SequenceEqual(second);
        }
    }
}
