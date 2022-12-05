namespace External_Balanced_Merge.Generators;

public class BinaryFileGenerator : IFileGenerator
{
    private readonly Random _random = new();

    public void GenerateBySize(string fileName, int megabytes, int minNum = 0, int maxNum = 1_000_000_000)
    {
        using var writer = new BinaryWriter(File.Open(fileName, FileMode.Create));

        for (int i = 0; i % 10_000 != 0 || !(new FileInfo(fileName).Length >= ByteConverter.MegabytesToBytes(megabytes)); i++)
        {
            writer.Write(_random.Next(minNum, maxNum));
        }
    }

    public void GenerateByLinesCount(string fileName, long linesCount, int minNum = 0, int maxNum = 1_000_000_000)
    {
        using var writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
        int currentCount = 0;

        while (currentCount++ != linesCount)
        {
            writer.Write(_random.Next(minNum, maxNum));
        }
    }
}