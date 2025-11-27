// <copyright file="SkipListTests.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SkipList.Tests;

/// <summary>
/// Tests for SkipList.
/// </summary>
[TestFixture]
public class SkipListTests
{
    /// <summary>
    /// Checks that the constructor accepting the collection creates a sorted list.
    /// </summary>
    [Test]
    public void SkipList_Constructor_WithUnsortedArray_CreatesSortedList()
    {
        var list = new SkipList<int>([73, 12, 99, 33, 5]);

        Assert.That(list, Is.EqualTo(new[] { 5, 12, 33, 73, 99 }).AsCollection);
    }

    /// <summary>
    /// Checks that consecutive Add calls preserve the sorting order.
    /// </summary>
    [Test]
    public void SkipList_Add_MultipleStrings_KeepsSortedOrder()
    {
        var list = new SkipList<string>
        {
            "python",
            "java",
            "rust",
            "csharp",
            "go",
        };

        Assert.That(list, Is.EqualTo(new[] { "csharp", "go", "java", "python", "rust" }).AsCollection);
    }

    /// <summary>
    /// Checks the operation of Contains after adding 2000 elements.
    /// </summary>
    [Test]
    public void SkipList_Add_TwoThousandIntegers_ContainsReturnsCorrectResult()
    {
        var list = new SkipList<int>();

        for (var i = 2000; i >= 1; i--)
        {
            list.Add(i);
        }

        Assert.Multiple(() =>
        {
            Assert.That(list.Contains(42), Is.True);
            Assert.That(list.Contains(1337), Is.True);
            Assert.That(list.Contains(2000), Is.True);
            Assert.That(list.Contains(2001), Is.False);
        });
    }

    /// <summary>
    /// Checks that adding null throws an ArgumentNullException.
    /// </summary>
    [Test]
    public void SkipList_Add_NullReference_ThrowsArgumentNullException()
    {
        var list = new SkipList<string>();

        Assert.Throws<ArgumentNullException>(() => list.Add(null!));
    }

    /// <summary>
    /// Checks the correctness of the Count property after several inserts.
    /// </summary>
    [Test]
    public void SkipList_Count_AfterFourAdds_ReturnsFour()
    {
        var list = new SkipList<double>
        {
            9.81,
            2.718,
            3.14159,
            1.414,
        };

        Assert.That(list.Count, Is.EqualTo(4));
    }

    /// <summary>
    /// Checks that foreach returns the items in sorted order.
    /// </summary>
    [Test]
    public void SkipList_GetEnumerator_UnsortedInput_ReturnsSortedSequence()
    {
        var list = new SkipList<int>([500, 100, 400, 200, 300]);

        Assert.That(list, Is.EqualTo(new[] { 100, 200, 300, 400, 500 }).AsCollection);
    }

    /// <summary>
    /// Checks that changing the list during enumeration throws an InvalidOperationException.
    /// </summary>
    [Test]
    public void SkipList_GetEnumerator_ModifyDuringEnumeration_ThrowsInvalidOperationException()
    {
        var list = new SkipList<int>([10, 20, 30]);
        using var enumerator = list.GetEnumerator();

        enumerator.MoveNext();
        list.Add(999);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    /// <summary>
    /// Checks that Clear resets Count.
    /// </summary>
    [Test]
    public void SkipList_Clear_NonEmptyList_SetsCountToZero()
    {
        var list = new SkipList<int>([111, 222, 333]);
        list.Clear();

        Assert.That(list.Count, Is.Zero);
    }

    /// <summary>
    /// Checks that the collection is really empty after Clear.
    /// </summary>
    [Test]
    public void SkipList_Clear_NonEmptyList_BecomesEmptyCollection()
    {
        var list = new SkipList<string>(["alpha", "beta", "gamma"]);
        list.Clear();

        Assert.That(list, Is.Empty);
        Assert.That(list.Any(), Is.False);
    }

    /// <summary>
    /// Checks the operation of Contains for an existing element.
    /// </summary>
    [Test]
    public void SkipList_Contains_ExistingGuid_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var list = new SkipList<Guid>([Guid.NewGuid(), Guid.NewGuid()]) { guid };

        Assert.That(list.Contains(guid), Is.True);
    }

    /// <summary>
    /// Checks the correctness of the CopyTo if the array size is sufficient.
    /// </summary>
    [Test]
    public void SkipList_CopyTo_SufficientArray_CopiesElementsCorrectly()
    {
        var list = new SkipList<int>([7, 14, 21, 28]);
        var array = new int[7];

        list.CopyTo(array, 2);

        Assert.That(array, Is.EqualTo(new[] { 0, 0, 7, 14, 21, 28, 0 }).AsCollection);
    }

    /// <summary>
    /// Checks that CopyTo throws an exception if the array size is insufficient.
    /// </summary>
    [Test]
    public void SkipList_CopyTo_SmallArray_ThrowsArgumentOutOfRangeException()
    {
        var list = new SkipList<int>([1, 2, 3]);
        var small = new int[2];

        Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(small, 0));
    }

    /// <summary>
    /// Checks that the Remove of an existing element returns true and deletes it.
    /// </summary>
    [Test]
    public void SkipList_Remove_ExistingString_ReturnsTrueAndRemovesElement()
    {
        var list = new SkipList<string>(["wolf", "bear", "fox", "lynx"]);

        var removed = list.Remove("bear");

        Assert.Multiple(() =>
        {
            Assert.That(removed, Is.True);
            Assert.That(list.Contains("bear"), Is.False);
            Assert.That(list.Count, Is.EqualTo(3));
        });
    }

    /// <summary>
    /// Checks that the Remove of a non-existent element returns false.
    /// </summary>
    [Test]
    public void SkipList_Remove_NonExistingInteger_ReturnsFalse()
    {
        var list = new SkipList<int>([100, 200, 300]);

        var removed = list.Remove(999);

        Assert.That(removed, Is.False);
        Assert.That(list.Count, Is.EqualTo(3));
    }

    /// <summary>
    /// Checks whether indexOf is valid for an existing element.
    /// </summary>
    [Test]
    public void SkipList_IndexOf_ExistingInteger_ReturnsCorrectIndex()
    {
        var list = new SkipList<int>([150, 250, 350, 450]);

        Assert.That(list.IndexOf(350), Is.EqualTo(2));
    }

    /// <summary>
    /// Checks that indexOf returns -1 for the missing element.
    /// </summary>
    [Test]
    public void SkipList_IndexOf_NonExistingInteger_ReturnsMinusOne()
    {
        var list = new SkipList<int>([11, 22, 33]);

        Assert.That(list.IndexOf(44), Is.EqualTo(-1));
    }

    /// <summary>
    /// Checks that Insert always throws a NotSupportedException.
    /// </summary>
    [Test]
    public void SkipList_Insert_AnyIndex_ThrowsNotSupportedException()
    {
        var list = new SkipList<int>();

        Assert.Throws<NotSupportedException>(() => list.Insert(0, 123));
    }

    /// <summary>
    /// Checks the deletion of an element by index via RemoveAt.
    /// </summary>
    [Test]
    public void SkipList_RemoveAt_MiddleIndex_RemovesCorrectElement()
    {
        var list = new SkipList<int>([5, 10, 15, 20, 25]);

        list.RemoveAt(2);

        Assert.That(list, Is.EqualTo(new[] { 5, 10, 20, 25 }).AsCollection);
        Assert.That(list, Has.No.Member(15));
    }

    /// <summary>
    /// Checks the receipt of an element by a valid index.
    /// </summary>
    [Test]
    public void SkipList_Indexer_GetValidIndex_ReturnsCorrectValue()
    {
        var list = new SkipList<int>([8, 6, 7, 5, 9]);

        Assert.That(list[2], Is.EqualTo(7));
    }

    /// <summary>
    /// Checks that accessing an index outside the list throws an ArgumentOutOfRangeException.
    /// </summary>
    [Test]
    public void SkipList_Indexer_GetOutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        var list = new SkipList<int>([1, 2, 3]);

        Assert.Throws<ArgumentOutOfRangeException>(() => _ = list[-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = list[4]);
    }
}