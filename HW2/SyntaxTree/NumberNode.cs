// <copyright file="NumberNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Represents a number node in the syntax tree.
/// </summary>
public class NumberNode(int value) : IAbstractNode
{
    /// <inheritdoc/>
    public int Calculate() => value;

    /// <inheritdoc/>
    public string ToStringRepresentation() => value.ToString();
}