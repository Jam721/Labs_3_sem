using System.Collections;

namespace Lab3.Additional;

public class SimpleStackItems<T>
{
    public T? Element { get; set; }
    public SimpleStackItems<T>? Next { get; set; }
}

public class SimpleStack<T> : IEnumerable<T>
{
    SimpleStackItems<T>? Head { get; set; }
    SimpleStackItems<T>? _tail;

    public bool IsEmpty { get { return Head == null; } }
    
    public void Add(T value)
    {
        if (IsEmpty)
        {
            _tail = Head = new SimpleStackItems<T>() { Element = value, Next = null };
        }
        else
        {
            var item = new SimpleStackItems<T>() {Element = value, Next = null};
            if (_tail != null) _tail.Next = item;
            _tail = item;
        }
    }
    public T Pop()
    {
        if (IsEmpty) throw new Exception("Пусто");
        var result = Head!.Element;
        Head = Head.Next;
        if (Head == null)
            _tail = null;
        return result ?? throw new InvalidOperationException();
    }

    public override string ToString()
    {
        string s = "";
        foreach (var i in this)
        {
            s += i + " ";
        }
            
        return s;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = Head;
        while (current != null)
        {
            if (current.Element != null) yield return current.Element;
            current = current.Next;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}