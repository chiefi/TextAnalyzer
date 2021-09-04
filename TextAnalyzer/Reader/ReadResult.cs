namespace TextAnalyzer.Reader
{
    public class ReadResult
    {
        private ReadResult() { }

        public bool Successful { get; set; }
        public string[] Data { get; set; }
        public string Error { get; set; }

        public static ReadResult Create(string[] data)
        {
            return new ReadResult
            {
                Successful = true,
                Data = data
            };
        }

        public static ReadResult Fail(string error)
        {
            return new ReadResult
            {
                Successful = false,
                Error = error
            };
        }
    }
}
