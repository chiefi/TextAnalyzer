using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TextAnalyzer.Reader
{
    public interface IDocumentReader
    {
        Task<ReadResult> GetTextAsync(string path);
    }

    public class DocumentReader : IDocumentReader
    {
        private HttpClient client = new HttpClient();
                
        public async Task<ReadResult> GetTextAsync(string path)
        {
            Log.Logger.Debug("Reading {0}...", path);
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var result = IsUrl(path)
                   ? await GetTextFromUrl(path)
                   : await GetTextFromFile(path);

                Log.Logger.Debug("Read {0} in {1}ms and found {2} lines", path, sw.ElapsedMilliseconds, result.Length);
                return ReadResult.Create(result);
            }
            catch (Exception e)
            {
                Log.Logger.Debug("Failed to read {0} in {1}ms with error {2}", path, sw.ElapsedMilliseconds, e.Message);
                return ReadResult.Fail(e.Message);
            }
        }

        public bool IsUrl(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async Task<string[]> GetTextFromUrl(string url)
        {
            var result = await client.GetStringAsync(url);

            return result.Split('\n');
        }

        private async Task<string[]> GetTextFromFile(string file)
        {
            return await File.ReadAllLinesAsync(file);
        }
    }
}
