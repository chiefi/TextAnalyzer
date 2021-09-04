using System;
using System.Text;

namespace TextAnalyzer.Parser
{
    public interface IFilterService
    {
        string[] FilterRow(string row);
    }

    public class FilterService : IFilterService
    {

        public string[] FilterRow(string row)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < row.Length; i++)
            {
                // We need to count numbers like "30,000" and dates like "12.06.2016" as words
                if (i > 0 && i < row.Length - 1 &&
                    (row[i] == '.' || row[i] == ',') && char.IsDigit(row[i - 1]) && char.IsDigit(row[i + 1]))
                {
                    sb.Append(row[i]);
                    continue;
                }

                // We need to handle things like "up-to-date" as one word
                if (i > 0 && i < row.Length - 1 &&
                    row[i] == '-' && char.IsLetter(row[i - 1]) && char.IsLetter(row[i + 1]))
                {
                    sb.Append(row[i]);
                    continue;
                }

                // We need to count "55%" as a word
                if (i > 0 && row[i] == '%' && char.IsDigit(row[i - 1]))
                {
                    sb.Append(row[i]);
                    continue;
                }

                // We need to maintain apostrophes in words
                if (row[i] == '\'')
                {
                    sb.Append(row[i]);
                    continue;
                }

                // Cut away unwanted junk
                if (char.IsPunctuation(row[i]) || char.IsSymbol(row[i]))
                {
                    sb.Append(' ');
                    continue;
                }

                sb.Append(row[i]);
            }

            var words = sb.ToString().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
    }
}
