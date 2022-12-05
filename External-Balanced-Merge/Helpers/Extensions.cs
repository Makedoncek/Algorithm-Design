namespace External_Balanced_Merge;

public static class Extensions
{
    public static T NextAfter<T>(this List<T> source, T item)
    {
        int indexOfItem = source.IndexOf(item);
        return indexOfItem < source.Count - 1 ? source[indexOfItem + 1] : source[0];
    }

    public static bool EndOfStream(this BinaryReader source)
    {
        return source.BaseStream.Position == source.BaseStream.Length;
    }
}