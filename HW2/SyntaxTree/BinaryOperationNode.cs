// <copyright file="BinaryOperationNode.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Represents a binary operation node in the syntax tree.
/// </summary>
public class BinaryOperationNode : AbstractNode
{
    private readonly char operation;
    private readonly AbstractNode leftNode;
    private readonly AbstractNode rightNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryOperationNode"/> class.
    /// </summary>
    /// <param name="operation">The operation character (+, -, *, /).</param>
    /// <param name="leftNode">The left operand node.</param>
    /// <param name="rightNode">The right operand node.</param>
    public BinaryOperationNode(char operation, AbstractNode leftNode, AbstractNode rightNode)
    {
        this.operation = operation;
        this.leftNode = leftNode ?? throw new ArgumentNullException(nameof(leftNode));
        this.rightNode = rightNode ?? throw new ArgumentNullException(nameof(rightNode));
        this.Validate();
    }

    private void Validate()
    {
        if (!"+-*/".Contains(this.operation))
        {
            throw new ArgumentException("Invalid operator. Supported: +, -, *, /.", nameof(this.operation));
        }
    }

    /// <inheritdoc/>
    public override int Calculate()
    {
        var leftValue = this.leftNode.Calculate();
        var rightValue = this.rightNode.Calculate();

        return this.operation switch
        {
            '+' => leftValue + rightValue,
            '-' => leftValue - rightValue,
            '*' => leftValue * rightValue,
            '/' => rightValue != 0 ? leftValue / rightValue : throw new DivideByZeroException("Division by zero."),
            _ => throw new InvalidOperationException("Unsupported operation."),
        };
    }

    /// <inheritdoc/>
    public override string ToStringRepresentation()
    {
        return
            $"({this.operation} {this.leftNode.ToStringRepresentation()} {this.rightNode.ToStringRepresentation()})";
    }
}