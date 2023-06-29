namespace Patric;

public static class ParallelEx
{
    private static readonly IEnumerable<bool> InfiniteSequence = Infinite();

    public static void While(
        ParallelOptions parallelOptions, Func<bool> condition, Action<ParallelLoopState> body)
    {
        Parallel.ForEach(InfiniteSequence, parallelOptions,
            (_, loopState) =>
            {
                if (condition()) body(loopState);
                else loopState.Stop();
            });
    }

    private static IEnumerable<bool> Infinite()
    {
        while (true) yield return true;
    }
}