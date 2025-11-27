// <copyright file="SkipList.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SkipList;

using System.Collections;

/// <summary>
/// Skip list implementation.
/// </summary>
/// <typeparam name="T">Type of elements in the list.</typeparam>
public class SkipList<T> : IList<T>
    where T : IComparable<T>
{
    private const int MaxLevel = 16;
    private readonly Random random = new();
    private readonly SkipListNode sentinel = new(default, null, null);
    private SkipListNode header;
    private SkipListNode baseHeader;
    private int currentVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipList{T}"/> class.
    /// </summary>
    public SkipList()
    {
        this.baseHeader = new SkipListNode(default, this.sentinel, this.sentinel);
        var curr = this.baseHeader;
        for (var level = 0; level < MaxLevel; level++)
        {
            curr = new SkipListNode(default, this.sentinel, curr);
        }

        this.header = curr;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipList{T}"/> class with elements from a collection.
    /// </summary>
    /// <param name="collection">The collection to initialize from.</param>
    public SkipList(IEnumerable<T> collection)
        : this()
    {
        ArgumentNullException.ThrowIfNull(collection);
        foreach (var element in collection)
        {
            this.Add(element);
        }
    }

    /// <inheritdoc/>
    public int Count { get; private set; }

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var curr = this.baseHeader.Next ?? throw new SkipListException("Cannot access item in empty skip list.");
            for (var i = 0; i < index; i++)
            {
                curr = curr.Next ?? throw new InvalidOperationException("Index exceeds list bounds.");
            }

            return curr.Value ?? throw new InvalidOperationException("Null value encountered at index.");
        }
        set => throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        var curr = this.baseHeader.Next;
        var startVersion = this.currentVersion;
        while (curr != this.sentinel && curr != null)
        {
            if (startVersion != this.currentVersion)
            {
                throw new InvalidOperationException("Collection modified during enumeration.");
            }

            yield return curr.Value ?? throw new NullReferenceException("Element value is null.");
            curr = curr.Next;
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <inheritdoc/>
    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var level = this.GenerateRandomLevel();
        var curr = this.header;
        var updateNodes = new SkipListNode[level];
        for (var i = 0; i <= MaxLevel - level; i++)
        {
            curr = curr.Down ?? curr;
        }

        for (var lvl = level - 1; lvl >= 0; lvl--)
        {
            while (curr != null && curr.Next != null && curr.Next != this.sentinel && curr.Next.Value != null &&
                   curr.Next.Value.CompareTo(item) < 0)
            {
                curr = curr.Next;
            }

            updateNodes[lvl] = curr ?? throw new InvalidOperationException("Cannot add null to skip list.");
            curr = curr.Down;
        }

        SkipListNode? lower = null;
        for (var lvl = 0; lvl < level; lvl++)
        {
            var newNode = new SkipListNode(item, updateNodes[lvl].Next, lower);
            updateNodes[lvl].Next = newNode;
            lower = newNode;
        }

        this.Count++;
        this.currentVersion++;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        this.baseHeader = new SkipListNode(default, this.sentinel, this.sentinel);
        var curr = this.baseHeader;
        for (var level = 1; level < MaxLevel; level++)
        {
            curr = new SkipListNode(default, this.sentinel, curr);
        }

        this.header = curr;
        this.Count = 0;
        this.currentVersion++;
    }

    /// <inheritdoc/>
    public bool Contains(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var curr = this.header;
        while (curr != null)
        {
            while (curr.Next != null && curr.Next != this.sentinel &&
                   curr.Next.Value != null &&
                   curr.Next.Value.CompareTo(item) < 0)
            {
                curr = curr.Next;
            }

            if (curr.Next != null && curr.Next != this.sentinel &&
                curr.Next.Value != null &&
                curr.Next.Value.CompareTo(item) == 0)
            {
                return true;
            }

            curr = curr.Down;
        }

        return false;
    }

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0 || arrayIndex >= array.Length || arrayIndex + this.Count > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        var curr = this.baseHeader.Next;
        var idx = arrayIndex;
        while (curr != this.sentinel && curr != null)
        {
            array[idx++] = curr.Value ?? throw new InvalidOperationException("Null element in list.");
            curr = curr.Next;
        }
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var curr = this.header;
        var removed = false;
        while (curr != null)
        {
            while (curr.Next != null && curr.Next != this.sentinel &&
                   curr.Next.Value != null &&
                   curr.Next.Value.CompareTo(item) < 0)
            {
                curr = curr.Next;
            }

            if (curr.Next != null && curr.Next != this.sentinel &&
                curr.Next.Value != null &&
                curr.Next.Value.CompareTo(item) == 0)
            {
                curr.Next = curr.Next.Next;
                removed = true;
            }

            curr = curr.Down;
        }

        if (removed)
        {
            this.Count--;
            this.currentVersion++;
        }

        return removed;
    }

    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var idx = 0;
        var curr = this.baseHeader.Next;
        while (curr != null && curr != this.sentinel)
        {
            if (curr.Value != null && curr.Value.CompareTo(item) == 0)
            {
                return idx;
            }

            if (curr.Value != null && curr.Value.CompareTo(item) > 0)
            {
                return -1;
            }

            curr = curr.Next;
            idx++;
        }

        return -1;
    }

    /// <inheritdoc/>
    public void Insert(int index, T item)
    {
        throw new NotSupportedException("Insertion at specific index not supported in sorted skip list.");
    }

    /// <inheritdoc/>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= this.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var item = this[index];
        this.Remove(item);
    }

    private int GenerateRandomLevel()
    {
        var lvl = 1;
        while (this.random.NextDouble() < 0.5 && lvl < MaxLevel)
        {
            lvl++;
        }

        return lvl;
    }

    private class SkipListNode(T? value, SkipListNode? next, SkipListNode? down)
    {
        public T? Value { get; set; } = value;

        public SkipListNode? Next { get; set; } = next;

        public SkipListNode? Down { get; set; } = down;
    }
}