using AbyssBenchmarkLib.Measurements;
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
    }
}