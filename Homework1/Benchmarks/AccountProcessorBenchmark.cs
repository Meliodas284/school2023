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
		_processor.CalculatePerformed(_account);
	}
}
