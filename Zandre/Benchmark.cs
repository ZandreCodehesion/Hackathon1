using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zandre;

public static class Benchmark
{
    public static async Task<string> CalculateBenchmark(int timeInMilliseconds)
    {
        var algorithmResult = DoWork();
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
            DoWork();
            counter++;
        }
        stopwatch.Stop();
        decimal totalSeconds = stopwatch.ElapsedTicks / 1_000_000_000;
        return $"{counter};{totalSeconds};";
    }

    public static int[] DoWork()
    {
        var result = new List<int>();
        var randomGenerator = new Random();
        result.Add(randomGenerator.Next((int)DateTime.UtcNow.Microsecond));
        result.Add(randomGenerator.Next((int)DateTime.UtcNow.Microsecond));
        result.Add(randomGenerator.Next((int)DateTime.UtcNow.Microsecond));
        result.Add(randomGenerator.Next((int)DateTime.UtcNow.Microsecond));
        result.Add(randomGenerator.Next((int)DateTime.UtcNow.Microsecond));
        return result.ToArray();
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
