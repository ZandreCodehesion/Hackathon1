using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Patric;

public static class Program
{
    private static readonly ConcurrentBag<byte> Iterations = new();
    private static readonly Stopwatch Stopwatch= new();
    private const double MilliSeconds = 4990;
    
    public static async Task Main()
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };

        if (!await VerifyAlgorithmOutput(await DoWork()))
        {
            throw new Exception("The value should be positive");
        }
        
        //BENCHMARK Starts
        Stopwatch.Start();
        ParallelEx.While(options,
             () => Stopwatch.Elapsed.TotalMilliseconds < MilliSeconds, Body);
        ;
        var iterations = Iterations.Count;
        Stopwatch.Stop();
        var totalSeconds = Stopwatch.ElapsedTicks / 1_000_000_000m;
        Console.WriteLine($"{iterations};{totalSeconds};");
    }

    private static async void Body(ParallelLoopState _)
    {
        while (Stopwatch.Elapsed.TotalMilliseconds < MilliSeconds )
        {
            //await DoWork();
            for (var i = 0; i < 100000; i++)
            {
                Iterations.Add(1);

            }
        }
    }   


    private static async ValueTask<int[]> DoWork()
    {
        return new []
        {
           1
        };
    }

    private static async Task<bool> VerifyAlgorithmOutput(int[] algorithmOutput)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://hackathon-validator.vercel.app/api/verify");
        var content = new StringContent(JsonSerializer.Serialize(algorithmOutput), null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<VerificationResponse>();
        return result?.Success ?? throw new Exception("The value should be positive");
    }
}