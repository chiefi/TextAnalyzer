using System;
using System.Linq;
using TextAnalyzer.Data;

namespace TextAnalyzer.Printer
{
    public interface IResultPrinter
    {
        void Print(ITextStatistics textStatistics, int countForMostFrequentWords, int countForLongestWords);
        void PrintFailure(string name, string error);
    }

    public class ResultPrinter : IResultPrinter
    {
        public void Print(ITextStatistics textStatistics, int countForMostFrequentWords, int countForLongestWords)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("- Statistics for ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{textStatistics.Source}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n-- Most common words:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Join('\n', textStatistics.GetTopWords(countForMostFrequentWords).Select(FormatWordWithFrequency)));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n-- Longest words:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Join('\n', textStatistics.GetLongestWords(countForLongestWords).Select(FormatWordWithLength)));
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine("################################################################################");
        }

        public void PrintFailure(string name, string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Failure to analyze ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{name} - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
        }

        private string FormatWordWithFrequency(IWordFrequency wordFrequency)
        {
            return $"({wordFrequency.Frequency})\t{wordFrequency.Word}";
        }

        private string FormatWordWithLength(string word)
        {
            return $"({word.Length})\t{word}";
        }
    }
}
