using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Text;

namespace Fuse8_ByteMinds.SummerSchool.Benchmarks;

[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringInternBenchmark
{
	private readonly List<string> _words = new();
	public StringInternBenchmark()
	{
		foreach (var word in File.ReadLines(@".\SpellingDictionaries\ru_RU.dic"))
			_words.Add(string.Intern(word));
	}

	[Benchmark(Baseline = true)]
	[ArgumentsSource(nameof(SampleData))]
	public bool WordIsExists(string word)
		=> _words.Any(item => word.Equals(item, StringComparison.Ordinal));

	[Benchmark]
	[ArgumentsSource(nameof(SampleData))]
	public bool WordIsExistsIntern(string word)
	{
		var internedWord = string.Intern(word);
		return _words.Any(item => ReferenceEquals(internedWord, item));
	}

	public IEnumerable<string> SampleData()
	{
		yield return new StringBuilder().Append("Щёлк").Append("ово").ToString();
		yield return new StringBuilder().Append("Энд").Append("рю").ToString();
		yield return new StringBuilder().Append("прист").Append("роганный/AS").ToString();

		yield return _words[8];
		yield return _words[_words.Count / 2];
		yield return _words[^1];

		yield return "ъвъвъвъвъ";
		yield return new StringBuilder().Append("даст").Append("инпорье").ToString();
	}
}

// Результаты:
// |             Method |             word |           Mean |        Error |       StdDev | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
// |------------------- |----------------- |---------------:|-------------:|-------------:|------:|--------:|-----:|-------:|----------:|------------:|
// | WordIsExistsIntern |      дастинпорье | 3,470,879.3 ns | 62,648.14 ns | 55,535.94 ns |  0.70 |    0.01 |    1 |      - |     130 B |        0.98 |
// |       WordIsExists |      дастинпорье | 4,944,835.9 ns | 10,145.31 ns |  7,920.79 ns |  1.00 |    0.00 |    2 |      - |     132 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern |        ёкающий/A | 3,465,293.7 ns | 63,020.46 ns | 58,949.37 ns |  0.70 |    0.02 |    1 |      - |     130 B |        0.98 |
// |       WordIsExists |        ёкающий/A | 4,917,525.2 ns | 60,298.69 ns | 56,403.43 ns |  1.00 |    0.00 |    2 |      - |     132 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern |    полиморфизм/J | 1,721,816.7 ns | 33,812.63 ns | 29,974.01 ns |  0.69 |    0.01 |    1 |      - |     129 B |        0.99 |
// |       WordIsExists |    полиморфизм/J | 2,500,543.8 ns | 15,536.47 ns | 12,973.66 ns |  1.00 |    0.00 |    2 |      - |     130 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern | пристроганный/AS | 1,506,750.7 ns | 29,706.72 ns | 29,175.97 ns |  0.71 |    0.02 |    1 |      - |     129 B |        0.99 |
// |       WordIsExists | пристроганный/AS | 2,125,987.8 ns | 32,815.05 ns | 30,695.21 ns |  1.00 |    0.00 |    2 |      - |     130 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// |       WordIsExists |            Чуя/H |       336.5 ns |      6.45 ns |      7.17 ns |  1.00 |    0.00 |    1 | 0.0610 |     128 B |        1.00 |
// | WordIsExistsIntern |            Чуя/H |       414.0 ns |      7.92 ns |      7.41 ns |  1.23 |    0.03 |    2 | 0.0610 |     128 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern |          Щёлково |     1,667.3 ns |     28.40 ns |     26.57 ns |  0.85 |    0.03 |    1 | 0.0610 |     128 B |        1.00 |
// |       WordIsExists |          Щёлково |     1,976.9 ns |     39.32 ns |     43.71 ns |  1.00 |    0.00 |    2 | 0.0610 |     128 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern |        ъвъвъвъвъ | 3,461,286.9 ns | 65,491.65 ns | 64,321.55 ns |  0.71 |    0.02 |    1 |      - |     129 B |        0.98 |
// |       WordIsExists |        ъвъвъвъвъ | 4,891,690.1 ns | 81,282.94 ns | 76,032.12 ns |  1.00 |    0.00 |    2 |      - |     132 B |        1.00 |
// |                    |                  |                |              |              |       |         |      |        |           |             |
// | WordIsExistsIntern |            Эндрю |     2,251.8 ns |     43.46 ns |     40.65 ns |  0.87 |    0.02 |    1 | 0.0610 |     128 B |        1.00 |
// |       WordIsExists |            Эндрю |     2,589.0 ns |     51.05 ns |     50.14 ns |  1.00 |    0.00 |    2 | 0.0610 |     128 B |        1.00 |
