// <copyright file="NumberNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Represents a number node in the syntax tree.
/// </summary>
public class NumberNode(int value) : AbstractNode
{
    /// <inheritdoc/>
    public override int Calculate()
    {
        return value;
    }

    /// <inheritdoc/>
    public override string ToStringRepresentation()
    {
        return value.ToString();
    }
}