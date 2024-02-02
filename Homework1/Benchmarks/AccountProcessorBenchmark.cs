using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Fuse8_ByteMinds.SummerSchool.Domain;

namespace Fuse8_ByteMinds.SummerSchool.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class AccountProcessorBenchmark
{
	private readonly AccountProcessor _processor = new();
	private readonly BankAccount _account = new();

	[Benchmark]
	public void Calculate()
	{
		_processor.Calculate(_account);
	}

	[Benchmark]
	public void CalculatePerformed()
	{
		_processor.CalculatePerformed(in _account);
	}
}

// Результаты
// |             Method |       Mean |    Error |  StdDev | Rank |   Gen0 | Allocated |
// |------------------- |-----------:|---------:|--------:|-----:|-------:|----------:|
// | CalculatePerformed |   833.5 ns |  7.99 ns | 7.48 ns |    1 |      - |         - |
// |          Calculate | 2,021.7 ns | 10.17 ns | 8.49 ns |    2 | 3.2120 |    6720 B |
