using CommandLine;
using System.Collections.Generic;

namespace TextAnalyzer
{
    public class Options
    {
        [Option('s', "source", Required = true, HelpText = "Specify the texts to analyze.", Min = 1, Max = 1000)]
        public IEnumerable<string> Sources { get; set; }

        [Option('f', "frequency", Required = false, HelpText = "Specify the number of most frequent words that should be showed. Default is 20.")]
        public int CountForMostFrequentWords { get; set; } = 20;

        [Option('l', "longest", Required = false, HelpText = "Specify the number of longest words that should be showed. Default is 10.")]
        public int CountForLongestWords { get; set; } = 10;
    }
}
