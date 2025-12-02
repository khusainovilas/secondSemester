// <copyright file="Operation.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Base class for all binary operations.
/// </summary>
public abstract class Operation : IAbstractNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Operation"/> class.
    /// </summary>
    /// <param name="left">Left operand of the operation.</param>
    /// <param name="right">Right operand of the operation.</param>
    protected Operation(IAbstractNode left, IAbstractNode right)
    {
        this.Left = left;
        this.Right = right;
    }

    /// <summary>
    /// Gets left child of operator.
    /// </summary>
    protected IAbstractNode Left { get; }

    /// <summary>
    /// Gets right child of operator.
    /// </summary>
    protected IAbstractNode Right { get; }

    /// <summary>
    /// Gets symbol of operation.
    /// </summary>
    protected abstract char Symbol { get; } 

    /// <inheritdoc/>
    public abstract int Calculate();

    /// <inheritdoc/>
    public string ToStringRepresentation()
        => $"({this.Symbol} {this.Left.ToStringRepresentation()} {this.Right.ToStringRepresentation()})";
}