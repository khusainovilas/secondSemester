// <copyright file="Linq.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace MyLinq;

/// <summary>
/// methods for IEnumerable collections.
/// </summary>
public static class Linq
{
    /// <summary>
    /// Generates an infinite lazy sequence of prime numbers.
    /// </summary>
    /// <returns>An IEnumerable of prime numbers.</returns>
    public static IEnumerable<long> GetPrimes()
    {
        yield return 2;

        var knownPrimes = new List<long> { 2 };

        long current = 3;

        while (true)
        {
            var isPrime = knownPrimes.TakeWhile(prime => prime * prime <= current).All(prime => current % prime != 0);

            if (isPrime)
            {
                yield return current;
                knownPrimes.Add(current);
            }

            checked
            {
                current += 2;
            }
        }
    }

    /// <summary>
    /// Extension method to take the first n elements from a sequence lazily.
    /// </summary>
    /// <typeparam name="T">Type of elements in the sequence.</typeparam>
    /// <param name="sequence">The source sequence.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <returns>An IEnumerable with the first n elements.</returns>
    public static IEnumerable<T> Take<T>(this IEnumerable<T> sequence, int count)
    {
        ArgumentNullException.ThrowIfNull(sequence);

        if (count <= 0)
        {
            yield break;
        }

        var taken = 0;
        foreach (var item in sequence)
        {
            yield return item;
            taken++;
            if (taken >= count)
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Extension method to skip the first n elements of a sequence lazily.
    /// </summary>
    /// <typeparam name="T">Type of elements in the sequence.</typeparam>
    /// <param name="sequence">The source sequence.</param>
    /// <param name="count">Number of elements to skip.</param>
    /// <returns>An IEnumerable without the first n elements.</returns>
    public static IEnumerable<T> Skip<T>(this IEnumerable<T> sequence, int count)
    {
        ArgumentNullException.ThrowIfNull(sequence);

        if (count <= 0)
        {
            if (count == 0)
            {
                foreach (var item in sequence)
                {
                    yield return item;
                }
            }

            yield break;
        }

        var skipped = 0;
        foreach (var item in sequence)
        {
            skipped++;
            if (skipped > count)
            {
                yield return item;
            }
        }
    }
}