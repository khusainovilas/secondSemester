// <copyright file="MyLinqTests.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace MyLinq.Test;

/// <summary>
/// Unit tests for MyLinq.
/// </summary>
public class MyLinqTests
{
    /// <summary>
    /// test method GetPrimes by taking first 5 numbers.
    /// </summary>
    [Test]
    public void Linq_GetPrimes_FirstFiveNumbers()
        => Assert.That(Linq.GetPrimes().Take(5), Is.EquivalentTo(new[] { 2L, 3L, 5L, 7L, 11L }));

    /// <summary>
    /// test method Take by taking first 4 elements of char array.
    /// </summary>
    [Test]
    public void Linq_Take_FirstFourElements()
        => Assert.That(new[] { 'a', 'b', 'c', 'd', 'e', 'f' }.Take(4), Is.EquivalentTo(new[] { 'a', 'b', 'c', 'd' }));

    /// <summary>
    /// test method Skip by skipping first 8 elements of char array.
    /// </summary>
    [Test]
    public void Linq_Skip_SkipEightElements()
    {
        var text = "Hello, World!".ToCharArray();
        Assert.That(text.Skip(8), Is.EquivalentTo("orld!".ToCharArray()));
    }

    /// <summary>
    /// test method take and skip combination by getting 5 elements after skipping first 3 primes.
    /// </summary>
    [Test]
    public void Linq_SkipAndTakeCombination()
        => Assert.That(Linq.GetPrimes().Skip(3).Take(5), Is.EquivalentTo(new[] { 7L, 11L, 13L, 17L, 19L }));

    /// <summary>
    /// test skip with negative count should return empty sequence.
    /// </summary>
    [Test]
    public void Linq_Skip_NegativeCount()
        => Assert.That(Linq.GetPrimes().Skip(-5), Is.Empty);

    /// <summary>
    /// test take with negative count should return empty sequence.
    /// </summary>
    [Test]
    public void Linq_Take_NegativeCount()
        => Assert.That(Linq.GetPrimes().Take(-10), Is.Empty);
}