using System;
using System.Diagnostics;

namespace AbyssBenchmarkLib.Measurements;

public class SimpleMeasurement
{
    /// <summary>
    /// Measure elapsed time of the given code
    /// </summary>
    /// <param name="action">Some code to execute</param>
    /// <param name="logMessage">Log message to print</param>
    /// <returns></returns>
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
