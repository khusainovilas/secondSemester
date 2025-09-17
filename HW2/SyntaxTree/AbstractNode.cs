// <copyright file="AbstractNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Abstract node of the syntax tree.
/// </summary>
public abstract class AbstractNode
{
    /// <summary>
    /// Calculates the value of the node.
    /// </summary>
    /// <returns>The calculated result.</returns>
    public abstract int Calculate();

    /// <summary>
    /// Returns the string representation of the node.
    /// </summary>
    /// <returns>String representation of the node.</returns>
    public abstract string ToStringRepresentation();
}