using BenchmarkDotNet.Running;
using Guid.Benchmark;

var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

Benchmark benchmark = new();

await benchmark.Execute();
