using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TextAnalyzer.Data;

namespace TextAnalyzer.Parser
{
    public interface IDocumentParser
    {
        Task<ParseResult> ParseAsync(string source, string[] text);
    }

    public class DocumentParser : IDocumentParser
    {
        private const int CHUNK_SIZE = 2000;

        private readonly IFilterService filterService;

        public DocumentParser(IFilterService filterService)
        {
            this.filterService = filterService;
        }

        public async Task<ParseResult> ParseAsync(string source, string[] text)
        {
            if (text == null || text.Length == 0)
                return ParseResult.Fail(source, "No text to parse");

            Log.Logger.Debug("Parsing {0}...", source);
            Stopwatch sw = Stopwatch.StartNew(); 
            
            var result = new TextStatistics
            {
                NumberOfLines = text.Length,
                Source = source
            };

            var tasks = new List<Task>();

            for (int i = 0; i < text.Length; i += CHUNK_SIZE)
            {
                ArraySegment<string> segment = new ArraySegment<string>(text, i, i+ CHUNK_SIZE < text.Length ? CHUNK_SIZE : text.Length - i);
                tasks.Add(Task.Run(() => ProcessChunk(segment, result)));
            }

            await Task.WhenAll(tasks);

            Log.Logger.Debug("Parsed {0} in {1}ms and found {2} words and {3} lines", source, sw.ElapsedMilliseconds, result.NumberOfWords, result.NumberOfLines);

            return ParseResult.Create(source, result);
        }
                       
        private void ProcessChunk(IEnumerable<string> rows, TextStatistics data)
        {
            foreach (var row in rows)
            {
                var words = filterService.FilterRow(row);
                                
                foreach (var word in words)
                    data.InsertWord(word);
            }
        }
    }
}
