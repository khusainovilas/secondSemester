// <copyright file="Multiply.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Multiply operation.
/// </summary>
public class Multiply : Operation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Multiply"/> class.
    /// </summary>
    /// <param name="left">Left operand of the operation.</param>
    /// <param name="right">Right operand of the operation.</param>
    public Multiply(IAbstractNode left, IAbstractNode right)
        : base(left, right)
    {
    }

    /// <inheritdoc/>
    protected override char Symbol => '*';

    /// <inheritdoc/>
    public override int Calculate() => this.Left.Calculate() * this.Right.Calculate();
}