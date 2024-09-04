static class MyArrayPool<T>
{
    public static T[] Rent(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (length == 0)
        {
            return Array.Empty<T>();
        }
    }

    public static void Return(T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (array.Length == 0)
        {
            return;
        }
    }
}