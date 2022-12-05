namespace External_Balanced_Merge.Sorters;

public interface ISorter
{
    void Sort(string fileName, out string sortedFileName, int helpFilesCount = 3);
}