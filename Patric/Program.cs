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
    private const int Mil = 1_000_000;
    public static async Task Main()
    {
        var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };

        Console.WriteLine((await DoWork()).Count());
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
            for (var i = 0; i < 40; i++)
            {
                await DoWork();
                Iterations.Add(1);
            }
        }
    }   
    
    private static async ValueTask<int[]> DoWork()
    {
        var primes = new List<int>();
        
        await SieveOfAtkin(Mil,primes);
        
        return primes.ToArray();
    }

    private static async Task SieveOfAtkin(int limit, ICollection<int> primes)
    {
        if (limit > 2)
            primes.Add(2);
        if (limit > 3)
            primes.Add(3);

        var sieve = new bool[limit + 1];
        var limitSqrt = (int)Math.Sqrt(limit);

        for (var x = 1; x <= limitSqrt; x++)
        {
            var xx = x * x;

            var maxY = (int)Math.Sqrt(limit - xx);
            for (var y = 1; y <= maxY; y++)
            {
                var yy = y * y;

                var n = 4 * xx + yy;
                if (n <= limit)
                {
                    var mod12 = n % 12;
                    if (mod12 is 1 or 5)
                        sieve[n] ^= true;
                }

                n = 3 * xx + yy;
                if (n <= limit)
                {
                    int mod12 = n % 12;
                    if (mod12 == 7)
                        sieve[n] ^= true;
                }

                if (x <= y) continue;
                {
                    n = 3 * xx - yy;
                    if (n > limit) continue;
                    var mod12 = n % 12;
                    if (mod12 == 11)
                        sieve[n] ^= true;
                }
            }
        }

        for (var n = 5; n <= limitSqrt; n++)
        {
            if (!sieve[n]) continue;
            var nSquared = n * n;
            for (var k = nSquared; k <= limit; k += nSquared)
                sieve[k] = false;
        }


        for (var n = 5; n <= limit; n++)
            if (sieve[n])
                primes.Add(n);
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