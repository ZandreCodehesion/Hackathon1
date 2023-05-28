using System.Diagnostics;

namespace Zandre;

public static class Benchmark
{
    public static string CalculateBenchmark(int timeInMilliseconds)
    {
        var verifiedValue = DoWork();
        if (verifiedValue < 0)
        {
            throw new Exception("The value should be positive");
        }
        var counter = 0;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (stopwatch.ElapsedMilliseconds < timeInMilliseconds)
        {
            DoWork();
            counter++;
        }
        stopwatch.Stop();
        decimal totalSeconds = stopwatch.ElapsedTicks / 1_000_000_000;
        return $"{counter};{totalSeconds};";
    }

    public static int DoWork()
    {
        var randomGenerator = new Random();
        var value = randomGenerator.Next((int)DateTime.UtcNow.Microsecond);
        return value;
    }

}
