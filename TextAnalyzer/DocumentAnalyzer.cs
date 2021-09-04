using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextAnalyzer.Data;
using TextAnalyzer.Parser;
using TextAnalyzer.Printer;
using TextAnalyzer.Reader;

namespace TextAnalyzer
{
    public interface IDocumentAnalyzer
    {
        Task AnalyzeAsync(IEnumerable<string> sources, int countForMostFrequentWords, int countForLongestWords);
    }

    public class DocumentAnalyzer : IDocumentAnalyzer
    {
        private readonly IDocumentReader reader;
        private readonly IDocumentParser parser;
        private readonly IResultPrinter printer;

        public DocumentAnalyzer(IDocumentReader reader,
            IDocumentParser parser,
            IResultPrinter printer)
        {
            this.reader = reader;
            this.parser = parser;
            this.printer = printer;
        }

        public async Task AnalyzeAsync(IEnumerable<string> sources, int countForMostFrequentWords, int countForLongestWords)
        {
            var results = await Task.WhenAll(sources.Select(source => Task.Run(() => DoAsync(source, reader, parser))));

            foreach (var result in results.Where(r => !r.Successful))
                printer.PrintFailure(result.Source, result.Error);

            foreach (var result in results.Where(r => r.Successful))
                printer.Print(result.Data, countForMostFrequentWords, countForLongestWords);

            var mergedResult = MergeResult(results.Where(r => r.Successful).Select(r => r.Data));

            printer.Print(mergedResult, countForMostFrequentWords, countForLongestWords);
        }

        private async Task<ParseResult> DoAsync(string source, IDocumentReader reader, IDocumentParser parser)
        {
            var text = await reader.GetTextAsync(source);
            if (!text.Successful)
                return ParseResult.Fail(source, text.Error);

            var data = await parser.ParseAsync(source, text.Data);

            return data;
        }

        private ITextStatistics MergeResult(IEnumerable<ITextStatistics> textStatistics)
        {
            var x = new TextStatistics
            {
                Source = "all",
                NumberOfLines = textStatistics.Sum(x => x.NumberOfLines)
            };

            foreach (var item in textStatistics.SelectMany(x => x.GetFullData()))
                x.InsertWord(item.Key, item.Value);

            return x;
        }
    }
}
