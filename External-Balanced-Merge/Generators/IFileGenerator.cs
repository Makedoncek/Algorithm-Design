namespace External_Balanced_Merge.Generators;

public interface IFileGenerator
{
    void GenerateBySize(string fileName, int megabytes, int minNum = 0, int maxNum = 1_000_000_000);
    void GenerateByLinesCount(string fileName, long linesCount, int minNum = 0, int maxNum = 1_000_000_000);
}
