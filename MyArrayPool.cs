using System.Buffers;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;


// [SkipLocalsInit]
static int ComputeSomething(byte[] array)
{
    char[]? arrayPoolArray = null;
    Span<char> tmp = array.Length < 256 ?
        stackalloc char[array.Length] :
        (arrayPoolArray = ArrayPool<char>.Shared.Rent(array.Length));

    if (arrayPoolArray is not null)
    {
        ArrayPool<char>.Shared.Return(arrayPoolArray);
    }
    return 0;

}

static class MyArrayPool<T>
{
    [ThreadStatic]
    private static T[]?[] s_tls = new T[30][];
    private static readonly ConcurrentQueue<T[]>[] s_arrays = Enumerable.Range(0, 30).Select(_ => new ConcurrentQueue<T[]>()).ToArray();
    public static T[] Rent(int minimumLength)
    {

        // Array.MaxLength
        ArgumentOutOfRangeException.ThrowIfNegative(minimumLength);
        if (minimumLength == 0)
        {
            return Array.Empty<T>();
        }

        int index= BitOperations.Log2((uint) minimumLength - 1);
        ref T[]? tls = ref s_tls[index];
        if(tls is not null)
        {
            T[] tmpArray = tls;
            tls = null;
            return tmpArray;
        }


        ConcurrentQueue<T[]> queue = s_arrays[index];
        if (queue.TryDequeue(out T[]? array))
        {
            return array;
        }
        return new T[BitOperations.RoundUpToPowerOf2((uint)minimumLength)];
    }

    public static void Return(T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (array.Length == 0)
        {
            return;
        }
        if (!BitOperations.IsPow2(array.Length))
        {
            // this array did not come from the pool
            throw new Exception();
        }

        int index = BitOperations.Log2((uint) array.Length - 1);

        ref T[]? tls = ref s_tls[index];
        if(tls is null)
        {
            tls = array;
            return;
        } 

        ConcurrentQueue<T[]> queue = s_arrays[index];
        queue.Enqueue(array);
    }
}