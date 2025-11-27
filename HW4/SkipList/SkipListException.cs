// <copyright file="SkipListException.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>
namespace SkipList;

/// <summary>
/// Exception for empty skip list operations.
/// </summary>
public class SkipListException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipListException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public SkipListException(string message)
        : base(message)
    {
    }
}