using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System;

namespace AbyssBenchmarkLib;

public class AbyssBenchmarkRunner
{
    /// <summary>
    /// Execute benchmark of all methods marked with AbyssBenchmark attribute in a given class object
    /// </summary>
    /// <param name="target">Class object</param>
    /// <returns></returns>
    public static string RunAllBenchmarks(object target)
    {
        var result = string.Empty;

        var methods = target.GetType()
            .GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(AbyssBenchmarkAttribute), false).Any())
            .ToList();

        foreach (var method in methods)
        {
            result += RunBenchmark(target, method);
        }

        return result;
    }

    /// <summary>
    /// Execute benchmark of a given method in a given class
    /// </summary>
    /// <param name="target">Class object</param>
    /// <param name="methodName">Name of the method to benchmark</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string RunBenchmarkMethod(object target, string methodName)
    {
        var method = target.GetType().GetMethod(methodName);
        if (method == null)
        {
            throw new ArgumentException($"Method {methodName} not found.");
        }

        WarmUp(() => method.Invoke(target, null));

        return RunBenchmark(target, method);
    }

    /// <summary>
    /// Execute benchmark of a given method using an action / delegate
    /// </summary>
    /// <param name="codeBlock">Delegate to execute</param>
    /// <returns></returns>
    public static string RunBenchmarkMethod(Action codeBlock)
    {
        WarmUp(codeBlock);

        var (elapsedTime, memoryUsed) = MeasureCodePerformance(codeBlock);

        return $"Execution time: {elapsedTime:F4} ms\n" +
               $"Used memory: {memoryUsed / (1024.0 * 1024.0):F4} MB";
    }

    /// <summary>
    /// Execute benchmark of a given section
    /// </summary>
    /// <param name="sectionName">Section name</param>
    /// <param name="codeBlock">Code block to benchmark</param>
    /// <param name="repetitions">Number of repetitions to warmup the benchmark</param>
    /// <returns></returns>
    public static string BenchmarkSection(string sectionName, Action codeBlock, int repetitions = 50)
    {
        WarmUp(codeBlock);

        double totalElapsedTime = 0;
        double totalMemoryUsed = 0;

        for (int i = 0; i < repetitions; i++)
        {
            var (elapsedTime, memoryUsed) = MeasureCodePerformance(codeBlock);
            totalElapsedTime += elapsedTime;
            totalMemoryUsed += memoryUsed;
        }

        double averageTime = totalElapsedTime / repetitions;
        double averageMemoryUsed = totalMemoryUsed / repetitions / (1024.0 * 1024.0);

        return $"Section: {sectionName}\n" +
               $"Execution time average: {averageTime:F4} ms\n" +
               $"Used memory average: {averageMemoryUsed:F4} MB";
    }

    private static void WarmUp(Action codeBlock)
    {
        for (int i = 0; i < 5; i++)
        {
            codeBlock();
        }
    }

    private static string RunBenchmark(object target, MethodInfo method)
    {
        WarmUp(() => method.Invoke(target, null));

        var (elapsedTime, memoryUsed) = MeasureMethodPerformance(target, method);

        return $"Method: {method.Name}\n" +
               $"Execution time: {elapsedTime:F4} ms\n" +
               $"Used memory: {memoryUsed:F4} MB\n\n";
    }

    private static (double, double) MeasureCodePerformance(Action codeBlock)
    {
        long initialMemory = GC.GetTotalMemory(true);
        var stopwatch = Stopwatch.StartNew();

        codeBlock();

        stopwatch.Stop();
        long finalMemory = GC.GetTotalMemory(true);
        long memoryUsed = finalMemory - initialMemory;

        return (stopwatch.Elapsed.TotalMilliseconds, memoryUsed);
    }

    private static (double, double) MeasureMethodPerformance(object target, MethodInfo method)
    {
        var stopwatch = Stopwatch.StartNew();
        long memoryBefore = GC.GetTotalMemory(true);

        method.Invoke(target, null);

        stopwatch.Stop();
        long memoryAfter = GC.GetTotalMemory(true);
        double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        double memoryUsed = (memoryAfter - memoryBefore) / (1024.0 * 1024.0);

        return (elapsedTime, memoryUsed);
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class AbyssBenchmarkAttribute : Attribute
{
}