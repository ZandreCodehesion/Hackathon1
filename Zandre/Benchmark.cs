using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zandre;

public static class Benchmark
{
    public static async Task<string> CalculateBenchmark(int timeInMilliseconds)
    {
        var randomGenerator = new Random();
        var algorithmResult = DoWork(randomGenerator);
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
            DoWork(randomGenerator);
            counter++;
        }
        stopwatch.Stop();
        decimal totalSeconds = stopwatch.ElapsedTicks / 1_000_000_000;
        return $"{counter};{totalSeconds};";
    }

    public static int[] DoWork(Random randomGenerator)
    {
        var result = new List<int>();
        var x = randomGenerator.Next(1_000);
        var y = randomGenerator.Next(1_000);
        var z = randomGenerator.Next(1_000);

        var length = Math.Sqrt((x * x) + (y * y) + (z * z));

        var xUnit = x / length;
        var yUnit = y / length;
        var zUnit = z / length;

        var lengthUNIT = Math.Sqrt((xUnit * xUnit) + (yUnit * yUnit) + (zUnit * zUnit));

        if (((lengthUNIT - 1) * (lengthUNIT - 1)) > 0.0003)
        {
            throw new Exception($"Unit Variable Calculation is wrong {lengthUNIT} from {x},{y},{z} to {xUnit},{yUnit},{zUnit}");
        }


        result.Add(x);
        result.Add(y);
        result.Add(z);
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
