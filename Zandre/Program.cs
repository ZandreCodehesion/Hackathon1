// See https://aka.ms/new-console-template for more information

using Zandre;

var result = await Benchmark.CalculateBenchmark(5000);
// output should be in the format: {number of parses};{total time taken in seconds};
Console.WriteLine(result);
