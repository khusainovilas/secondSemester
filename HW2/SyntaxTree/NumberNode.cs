// <copyright file="NumberNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Represents a number node in the syntax tree.
/// </summary>
public class NumberNode(int value) : AbstractNode
{
    private readonly int value = value;

    /// <inheritdoc/>
    public override int Calculate()
    {
        return this.value;
    }

    /// <inheritdoc/>
    public override string ToStringRepresentation()
    {
        return this.value.ToString();
    }
}