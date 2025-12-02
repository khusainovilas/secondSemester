// <copyright file="Add.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Add operation.
/// </summary>
public class Add : Operation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Add"/> class.
    /// </summary>
    /// <param name="left">Left operand of the operation.</param>
    /// <param name="right">Right operand of the operation.</param>
    public Add(IAbstractNode left, IAbstractNode right)
        : base(left, right)
    {
    }

    /// <inheritdoc/>
    protected override char Symbol => '+';

    /// <inheritdoc/>
    public override int Calculate() => this.Left.Calculate() + this.Right.Calculate();
}