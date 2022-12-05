namespace External_Balanced_Merge;

public class FileWorker
{
    public static int[] GetArrayPart(int start, int size, string fileName)
    {
        var array = new int[size];
        using var reader = new BinaryReader(File.OpenRead(fileName));
        for (int i = 0; i < start; i++)
        {
            reader.ReadInt32();
        }

        for (int i = 0; i < size; i++)
        {
            array[i] = reader.ReadInt32();
        }

        return array;
    }

    public static void ShowContent(string fileName, int size)
    {
        var array = GetArrayPart(0, size, fileName);
        Console.Write("[ ");
        foreach (var i in array)
        {
            Console.Write(i + ", ");
        }
        Console.WriteLine("]");
    }
}
