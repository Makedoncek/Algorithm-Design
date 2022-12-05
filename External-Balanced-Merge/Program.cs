using System.Diagnostics;
using External_Balanced_Merge.Generators;
using External_Balanced_Merge.Sorters;

namespace External_Balanced_Merge;

internal class Program
{
    private static string _path = string.Empty;
    private static IFileGenerator _generator = null!;
    private static ISorter _sorter = null!;
    private const int linesCount = 256_000_000;

    public static void Main(string[] args)
    {
        Console.Write(new string('-', Console.BufferWidth));
        Console.WriteLine("Normal");
        Normal();
        Console.Write(new string('-', Console.BufferWidth));
        Console.WriteLine("Modified");
        Modified();
    }

    static void Normal()
    {
        _path = @"D:\ExternalBalancedMerge\Lab1\bin\Debug\net6.0\textFile.txt";

        _generator = new TextFileGenerator();
        _generator.GenerateBySize(_path, 10);
        Console.WriteLine("Generated");
        _sorter = new TextSorter();
        var sw = Stopwatch.StartNew();
        _sorter.Sort(_path, out string sortedFileName);
        sw.Stop();
        Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {sw.Elapsed.TotalSeconds}");
    }

    static void Modified()
    {
        _path = @"D:\ExternalBalancedMerge\Lab1\bin\Debug\net6.0\binFile.dat";

        _generator = new BinaryFileGenerator();
        _generator.GenerateByLinesCount(_path, linesCount);
        Console.WriteLine("Generated");
        _sorter = new BinarySorter();
        var sw = Stopwatch.StartNew();
        ((BinarySorter)_sorter).SortByParts(_path, "sorted.dat", linesCount, linesCount / 8);
        _sorter.Sort("sorted.dat", out string sortedFileName);
        sw.Stop();
        Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {sw.Elapsed.TotalSeconds}");
        FileWorker.ShowContent(sortedFileName, 20);
    }
}