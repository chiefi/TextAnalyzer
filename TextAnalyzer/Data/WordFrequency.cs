namespace TextAnalyzer.Data
{
    /// <summary>
    /// Represents a word and its frequency.
    /// </summary>
    public interface IWordFrequency
    {
        /// <summary>
        /// The word.
        /// </summary>
        /// <returns>The word as a string.</returns>
        string Word { get; set; }

        /// <summary>
        /// The frequency.
        /// </summary>
        /// <returns>A long representing the frequency of the word.</returns>
        long Frequency { get; set; }
    }

    public class WordFrequency : IWordFrequency
    {
        public WordFrequency(string word, long frequency)
        {
            Word = word;
            Frequency = frequency;
        }

        public string Word { get; set; }
        public long Frequency { get; set; }
    }
}
