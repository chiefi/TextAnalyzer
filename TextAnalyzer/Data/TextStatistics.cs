using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer.Data
{
    public interface ITextStatistics
    {
        /// <summary>
        /// Returns a list of the most frequented words of the text.
        /// </summary>
        /// <param name="n">How many items of the list</param>
        /// <returns>A list representing the top n frequent words of the text.</returns>
        IList<IWordFrequency> GetTopWords(int n);

        /// <summary>
        /// Returns a list of the longest words of the text.
        /// </summary>
        /// <param name="n">How many items to return.</param>
        /// <returns>A list with the n longest words of the text.</returns>
        IList<string> GetLongestWords(int n);

        /// <summary>
        /// Total number of words in the text.
        /// </summary>
        long NumberOfWords { get; }
        
        /// <summary>
        /// Total number of line of the text.
        /// </summary>
        long NumberOfLines { get; set; }

        /// <summary>
        /// The source of the parsed document.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// All internal data of words and frequencies.
        /// </summary>
        /// <returns>A dictionary with all data</returns>
        IDictionary<string, int> GetFullData();
    }

    public class TextStatistics : ITextStatistics
    {
        private ConcurrentDictionary<string, int> lookup = new ConcurrentDictionary<string, int>();

        public long NumberOfWords => lookup.Sum(x => x.Value);
        public long NumberOfLines { get; set; }
        public string Source { get; set; }

        public IList<string> GetLongestWords(int n)
        {
            return lookup.OrderByDescending(x => x.Key.Length).ThenBy(x => x.Key).Take(n).Select(x => x.Key).ToList();
        }

        public IList<IWordFrequency> GetTopWords(int n)
        {
            return lookup.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Take(n).Select(x => new WordFrequency(x.Key, x.Value)).ToList<IWordFrequency>();
        }

        public void InsertWord(string word, int count = 1)
        {
            lookup.AddOrUpdate(word, count, (x, y) => y + count);
        }

        public IDictionary<string, int> GetFullData()
        {
            return lookup;
        }
    }
}
