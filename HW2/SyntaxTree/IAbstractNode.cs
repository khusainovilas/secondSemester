// <copyright file="IAbstractNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Represents a node in the syntax tree.
/// </summary>
public interface IAbstractNode
{
    /// <summary>
    /// Calculates the value of the node.
    /// </summary>
    /// <returns>The calculated result.</returns>
    int Calculate();

    /// <summary>
    /// Returns the string representation of the node.
    /// </summary>
    /// <returns>String representation of the node.</returns>
    string ToStringRepresentation();
}