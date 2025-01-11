using System;
using System.Diagnostics;

namespace AbyssBenchmarkLib.Measurements;

public class SimpleMeasurement
{
    public static TimeSpan MeasureTime(Action action, string logMessage = "Elapsed time")
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        action.Invoke();

        timer.Stop();

        Console.WriteLine($"{logMessage}: {timer.Elapsed}");
        return timer.Elapsed;
    }
}
