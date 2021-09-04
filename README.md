# TextAnalyzer
Simple application for reading and analyzing texts

[![.NET](https://github.com/chiefi/TextAnalyzer/actions/workflows/dotnet.yml/badge.svg)](https://github.com/chiefi/TextAnalyzer/actions/workflows/dotnet.yml)

### Features
- Reads text from local file or from web adress
- Parses text and prints word statistics
- Shows frequencies of words
- Shows longest words
- Supports doing analyzis on multiple texts in parallell

### Platforms
.NET 5

### License
Free for commercial use under the MIT License 2.0.

### Usage
```csharp
> TextAnalyzer.exe -s 12345.txt https://www.gutenberg.org/files/45839/45839.txt
```

```
  -s, --source       Required. Specify the texts to analyze.
  -f, --frequency    Specify the number of most frequent words that should be showed. Default 20.
  -l, --longest      Specify the number of longest words that should be showed. Default 10.

```
