using AbyssBenchmarkLib;
using AbyssBenchmarkLib.Measurements;
using AbyssBenchmarkProject;
using System;
using System.Collections.Generic;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        var testList = new List<int>() { 1, 5, 3, 2, 10 };

        SimpleMeasurement.MeasureTime(() =>
        {
            testList = testList.Order().ToList();
        }, "Unnordered list");

        SimpleMeasurement.MeasureTime(() =>
        {
            testList = testList.Order().ToList();
        }, "Ordered list");

        Console.WriteLine("-------------------------");

        var foo = new Md5VsSha256();

        Console.WriteLine("Benchmark methods in class");
        var results = AbyssBenchmarkRunner.RunAllBenchmarks(foo);
        Console.WriteLine(results);

        Console.WriteLine("Benchmark code section");
        List<Md5VsSha256> innerList = new List<Md5VsSha256>();
        string results2 = AbyssBenchmarkRunner.BenchmarkSection("Test", () =>
        {
            for (int i = 0; i < 50; i++)
            {
                innerList.Add(new Md5VsSha256());
            }
        });
        Console.WriteLine(results2);

        Console.WriteLine("Benchmark specific method");
        var results3 = AbyssBenchmarkRunner.RunBenchmarkMethod(foo, "Sha256");
        Console.WriteLine(results3);

        Console.WriteLine("Benchmark some delegate");
        var results4 = AbyssBenchmarkRunner.RunBenchmarkMethod(() => foo.Sha256());
        Console.WriteLine(results4);
    }
}