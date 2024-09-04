class MyPool<T> where T: class,  new() 
{
    private T? _item;

    public T Rent() 
    {
        T? item = _item;
        if(item is null)
        {
            return new T();
        }
        else 
        {
            _item = null;
            return item;
        }
    }

    public void Return(T item)
    {
        _item ??= item;
    }
}

