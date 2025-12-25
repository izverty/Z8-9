using System;
using System.IO;
public class MyVector<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;

    private const int DEFAULT_CAPACITY = 10;

    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0)
            throw new ArgumentException("initialCapacity < 0");

        elementData = new T[initialCapacity];
        elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }
    public MyVector(int initialCapacity)
        : this(initialCapacity, 0)
    {
    }
    public MyVector()
        : this(DEFAULT_CAPACITY, 0)
    {
    }
    public MyVector(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        elementCount = a.Length;
        capacityIncrement = 0;
    }
    public void Add(T e)
    {
        EnsureCapacity(elementCount + 1);
        elementData[elementCount++] = e;
    }
    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        EnsureCapacity(elementCount + a.Length);
        foreach (var item in a)
            Add(item);
    }
    public void Clear()
    {
        elementData = new T[elementData.Length];
        elementCount = 0;
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
    public bool IsEmpty() => elementCount == 0;
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

        for (int i = 0; i < elementCount; i++)
        {
            if (Array.IndexOf(a, elementData[i]) < 0)
            {
                Remove(i);
                i--;
            }
        }
    }
    public int Size() => elementCount;
    public object[] ToArray()
    {
        object[] result = new object[elementCount];
        Array.Copy(elementData, result, elementCount);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < elementCount)
            a = new T[elementCount];

        Array.Copy(elementData, a, elementCount);
        return a;
    }
    public void Add(int index, T e)
    {
        CheckIndexForAdd(index);
        EnsureCapacity(elementCount + 1);

        Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        elementData[index] = e;
        elementCount++;
    }
    public void AddAll(int index, T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));

        CheckIndexForAdd(index);
        EnsureCapacity(elementCount + a.Length);

        Array.Copy(elementData, index, elementData, index + a.Length, elementCount - index);
        Array.Copy(a, 0, elementData, index, a.Length);
        elementCount += a.Length;
    }
    public T Get(int index)
    {
        CheckIndex(index);
        return elementData[index];
    }
    public int IndexOf(object o)
    {
        for (int i = 0; i < elementCount; i++)
            if (Equals(elementData[i], o))
                return i;

        return -1;
    }
    public int LastIndexOf(object o)
    {
        for (int i = elementCount - 1; i >= 0; i--)
            if (Equals(elementData[i], o))
                return i;

        return -1;
    }
    public T Remove(int index)
    {
        CheckIndex(index);

        T removed = elementData[index];
        Array.Copy(elementData, index + 1, elementData, index, elementCount - index - 1);
        elementData[--elementCount] = default;
        return removed;
    }
    public T Set(int index, T e)
    {
        CheckIndex(index);
        T old = elementData[index];
        elementData[index] = e;
        return old;
    }
    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();

        MyVector<T> result = new MyVector<T>(toIndex - fromIndex, capacityIncrement);
        for (int i = fromIndex; i < toIndex; i++)
            result.Add(elementData[i]);

        return result;
    }
    public T FirstElement()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Вектор пуст");

        return elementData[0];
    }
    public T LastElement()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Вектор пуст");

        return elementData[elementCount - 1];
    }
    public void RemoveElementAt(int pos)
    {
        Remove(pos);
    }
    public void RemoveRange(int begin, int end)
    {
        if (begin < 0 || end > elementCount || begin > end)
            throw new ArgumentOutOfRangeException();

        int count = end - begin;
        Array.Copy(elementData, end, elementData, begin, elementCount - end);
        elementCount -= count;
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity <= elementData.Length)
            return;

        int newCapacity;
        if (capacityIncrement > 0)
            newCapacity = elementData.Length + capacityIncrement;
        else
            newCapacity = elementData.Length * 2;

        if (newCapacity < minCapacity)
            newCapacity = minCapacity;

        T[] newArray = new T[newCapacity];
        Array.Copy(elementData, newArray, elementCount);
        elementData = newArray;
    }

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    private void CheckIndexForAdd(int index)
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
}
partial class Program
{
    static void Main()
    {
        MyVector<string> lines = new MyVector<string>();
        MyVector<string> ipVector = new MyVector<string>();
        using (StreamReader reader = new StreamReader("input.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        for (int i = 0; i < lines.Size(); i++)
        {
            ExtractIPsFromLine(lines.Get(i), ipVector);
        }
        using (StreamWriter writer = new StreamWriter("output.txt"))
        {
            for (int i = 0; i < ipVector.Size(); i++)
            {
                writer.WriteLine(ipVector.Get(i));
            }
        }

        Console.WriteLine("Готово. Найдено IP-адресов: " + ipVector.Size());
    }
    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    static bool IsValidIPv4(string s)
    {
        int parts = 0;
        int value = 0;
        int digits = 0;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (IsDigit(c))
            {
                value = value * 10 + (c - '0');
                digits++;

                if (digits > 3 || value > 255)
                    return false;
            }
            else if (c == '.')
            {
                if (digits == 0)
                    return false;

                parts++;
                value = 0;
                digits = 0;
            }
            else
            {
                return false;
            }
        }

        if (digits == 0)
            return false;

        parts++;
        return parts == 4;
    }

    static void ExtractIPsFromLine(string line, MyVector<string> result)
    {
        int n = line.Length;

        for (int i = 0; i < n; i++)
        {
            if (!IsDigit(line[i]))
                continue;

            int start = i;

            while (i < n && (IsDigit(line[i]) || line[i] == '.'))
                i++;

            int end = i;

            string candidate = line.Substring(start, end - start);

            bool leftOk = start == 0 || !IsDigit(line[start - 1]);
            bool rightOk = end == n || !IsDigit(line[end]);

            if (leftOk && rightOk && IsValidIPv4(candidate))
            {
                result.Add(candidate);
            }
        }
    }
}

