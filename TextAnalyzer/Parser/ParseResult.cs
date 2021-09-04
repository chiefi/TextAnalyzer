using TextAnalyzer.Data;

namespace TextAnalyzer.Parser
{
    public class ParseResult
    {
        private ParseResult() { }

        public string Source { get; set; }
        public bool Successful { get; set; }
        public ITextStatistics Data { get; set; }
        public string Error { get; set; }

        public static ParseResult Create(string source, ITextStatistics data)
        {
            return new ParseResult
            {
                Successful = true,
                Source = source,
                Data = data
            };
        }

        public static ParseResult Fail(string source, string error)
        {
            return new ParseResult
            {
                Successful = false,
                Source = source,
                Error = error
            };
        }
    }
}
