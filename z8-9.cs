using System;
using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        MyArrayList<string> tags = new MyArrayList<string>();

        foreach (string line in File.ReadLines("input.txt"))
        {
            ExtractTags(line, tags);
        }

        RemoveDuplicates(tags);

        Console.WriteLine("Уникальные теги:");
        foreach (string tag in tags.ToArray(new string[0]))
            Console.WriteLine(tag);
    }
    static void ExtractTags(string line, MyArrayList<string> tags)
    {
        var matches = Regex.Matches(
            line,
            @"<\s*/?[A-Za-z][A-Za-z0-9]*\s*>");

        foreach (Match m in matches)
            tags.Add(m.Value);
    }
    static void RemoveDuplicates(MyArrayList<string> list)
    {
        for (int i = 0; i < list.Size(); i++)
        {
            string normalizedI = NormalizeTag(list.Get(i));

            for (int j = i + 1; j < list.Size(); j++)
            {
                if (normalizedI == NormalizeTag(list.Get(j)))
                {
                    list.Remove(j);
                    j--;
                }
            }
        }
    }
    static string NormalizeTag(string tag)
    {
        tag = tag.ToLower();
        tag = tag.Replace("/", "");
        return tag;
    }
}

public class MyArrayList<T>
{
    private T[] elementData;
    private int size;

    private const int DEFAULT_CAPACITY = 10;
    public MyArrayList()
    {
        elementData = new T[DEFAULT_CAPACITY];
        size = 0;
    }
    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        size = a.Length;
    }
    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentException("capacity < 0");

        elementData = new T[capacity];
        size = 0;
    }
    public void Add(T e)
    {
        EnsureCapacity(size + 1);
        elementData[size++] = e;
    }
    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        EnsureCapacity(size + a.Length);
        foreach (var item in a)
            Add(item);
    }
    public void Clear()
    {
        elementData = new T[elementData.Length];
        size = 0;
    }
    public bool Contains(object o)
    {
        return IndexOf(o) >= 0;
    }
    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            if (!Contains(item))
                return false;

        return true;
    }
    public bool IsEmpty() => size == 0;
    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index < 0)
            return false;

        Remove(index);
        return true;
    }
    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
            while (Remove(item)) { }
    }
    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(a, elementData[i]) < 0)
            {
                Remove(i);
                i--;
            }
        }
    }
    public int Size() => size;
    public object[] ToArray()
    {
        object[] result = new object[size];
        Array.Copy(elementData, result, size);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < size)
            a = new T[size];

        Array.Copy(elementData, a, size);
        return a;
    }
    public void Add(int index, T e)
    {
        CheckIndexForAdd(index);
        EnsureCapacity(size + 1);

        Array.Copy(elementData, index, elementData, index + 1, size - index);
        elementData[index] = e;
        size++;
    }
    public void AddAll(int index, T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        CheckIndexForAdd(index);
        EnsureCapacity(size + a.Length);

        Array.Copy(elementData, index, elementData, index + a.Length, size - index);
        Array.Copy(a, 0, elementData, index, a.Length);
        size += a.Length;
    }
    public T Get(int index)
    {
        CheckIndex(index);
        return elementData[index];
    }
    public int IndexOf(object o)
    {
        for (int i = 0; i < size; i++)
            if (Equals(elementData[i], o))
                return i;

        return -1;
    }
    public int LastIndexOf(object o)
    {
        for (int i = size - 1; i >= 0; i--)
            if (Equals(elementData[i], o))
                return i;

        return -1;
    }
    public T Remove(int index)
    {
        CheckIndex(index);

        T removed = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, size - index - 1);
        elementData[--size] = default;
        return removed;
    }
    public T Set(int index, T e)
    {
        CheckIndex(index);
        T old = elementData[index];
        elementData[index] = e;
        return old;
    }
    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        MyArrayList<T> result = new MyArrayList<T>(toIndex - fromIndex);
        for (int i = fromIndex; i < toIndex; i++)
            result.Add(elementData[i]);

        return result;
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity <= elementData.Length)
            return;

        int newCapacity = (int)(elementData.Length * 1.5) + 1;
        if (newCapacity < minCapacity)
            newCapacity = minCapacity;

        T[] newArray = new T[newCapacity];
        Array.Copy(elementData, newArray, size);
        elementData = newArray;
    }

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    private void CheckIndexForAdd(int index)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
}
