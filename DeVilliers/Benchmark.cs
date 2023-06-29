using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zandre;

public static class Benchmark
{
    public static async Task<string> CalculateBenchmark(int timeInMilliseconds)
    {
        var algorithmResult = DoWork(1000000);
        var isSuccessfulAlgorithm = await VerifyAlgorithmOutput(algorithmResult);
        if (!isSuccessfulAlgorithm)
        {
            throw new Exception("The value should be positive");
        }
        var counter = 0;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        while (stopwatch.ElapsedMilliseconds < timeInMilliseconds)
        {
            DoWork(1000000);
            counter++;
        }
        stopwatch.Stop();
        decimal totalSeconds = stopwatch.ElapsedTicks / 1_000_000_000;
        return $"{counter};{totalSeconds};";
    }
    
    static int[] DoWork(int limit)
    {
        bool[] isPrime = new bool[limit + 1];
        for (int i = 2; i <= limit; i++)
        {
            isPrime[i] = true;
        }
        
        for (int i = 2; i * i <= limit; i++)
        {
            if (isPrime[i])
            {
                for (int j = (int)Math.Pow(i,2); j <= limit; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }
        
        List<int> primes = new List<int>();
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }
        
        return primes.ToArray();
    }
    
    public static async Task<bool> VerifyAlgorithmOutput(int[] algorithmOutput)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://hackathon-validator.vercel.app/api/verify");
        var content = new StringContent(JsonSerializer.Serialize(algorithmOutput), null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<VerificationResponse>();
        return result?.Success ?? false;
    }

}

public class VerificationResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
