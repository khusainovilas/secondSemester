// <copyright file="Divide.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Divide operation.
/// </summary>
public class Divide : Operation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Divide"/> class.
    /// </summary>
    /// <param name="left">Left operand of the operation.</param>
    /// <param name="right">Right operand of the operation.</param>
    public Divide(IAbstractNode left, IAbstractNode right)
        : base(left, right)
    {
    }

    /// <inheritdoc/>
    protected override char Symbol => '/';

    /// <inheritdoc/>
    public override int Calculate()
    {
        var divisor = this.Right.Calculate();
        if (divisor == 0)
        {
            throw new DivideByZeroException("Division by zero.");
        }

        return this.Left.Calculate() / divisor;
    }
}