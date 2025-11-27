// <copyright file="SyntaxTreeTests.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree.Tests;

/// <summary>
/// tests for syntax tree.
/// </summary>
public class SyntaxTreeTests
{
    private StringWriter consoleOutput;

    [SetUp]
    public void SetUp()
    {
        this.consoleOutput = new StringWriter();
        Console.SetOut(this.consoleOutput);
    }

    [TearDown]
    public void TearDown()
    {
        this.consoleOutput.Dispose();
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

    /// <summary>
    /// Test parsing a valid expression with multiplication and addition.
    /// </summary>
    [Test]
    public void Parser_Parse_ValidExpression_ReturnsCorrectTreeAndResult()
    {
        const string input = "(* (+ 1 1) 2)";
        var tree = Parser.Parse(input);
        Assert.Multiple(() =>
        {
            Assert.That(tree.ToStringRepresentation(), Is.EqualTo("(* (+ 1 1) 2)"));
            Assert.That(tree.Calculate(), Is.EqualTo(4));
        });
    }

    /// <summary>
    /// Test parsing an expression with negative numbers.
    /// </summary>
    [Test]
    public void Parser_Parse_NegativeNumber_ReturnsCorrectResult()
    {
        const string input = "(+ -5 3)";
        var tree = Parser.Parse(input);
        Assert.Multiple(() =>
        {
            Assert.That(tree.ToStringRepresentation(), Is.EqualTo("(+ -5 3)"));
            Assert.That(tree.Calculate(), Is.EqualTo(-2));
        });
    }

    /// <summary>
    /// Test parsing an empty input throws ArgumentException.
    /// </summary>
    [Test]
    public void Parser_Parse_EmptyInput_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parser.Parse(string.Empty));
    }

    /// <summary>
    /// Test parsing an invalid token throws ArgumentException.
    /// </summary>
    [Test]
    public void Parser_Parse_InvalidToken_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parser.Parse("(+ 1 a)"));
    }

    /// <summary>
    /// Test parsing unbalanced parentheses throws ArgumentException.
    /// </summary>
    [Test]
    public void Parser_Parse_UnbalancedParentheses_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Parser.Parse("(( 1 2)"));
    }

    /// <summary>
    /// Test parsing division by zero throws DivideByZeroException in Calculate.
    /// </summary>
    [Test]
    public void Parser_Parse_DivisionByZero_ThrowsDivideByZeroException()
    {
        const string input = "(/ 10 0)";
        var tree = Parser.Parse(input);
        Assert.That(tree.ToStringRepresentation(), Is.EqualTo("(/ 10 0)"));
        Assert.Throws<DivideByZeroException>(() => tree.Calculate());
    }

    /// <summary>
    /// Test NumberNode calculation and string representation.
    /// </summary>
    [Test]
    public void NumberNode_CalculateAndToString_ReturnsCorrectValues()
    {
        var node = new NumberNode(-5);
        Assert.Multiple(() =>
        {
            Assert.That(node.Calculate(), Is.EqualTo(-5));
            Assert.That(node.ToStringRepresentation(), Is.EqualTo("-5"));
        });
    }

    /// <summary>
    /// Test BinaryOperationNode calculation and string representation.
    /// </summary>
    [Test]
    public void BinaryOperationNode_CalculateAndToString_ReturnsCorrectValues()
    {
        var node = new BinaryOperationNode('+', new NumberNode(1), new NumberNode(2));
        Assert.Multiple(() =>
        {
            Assert.That(node.Calculate(), Is.EqualTo(3));
            Assert.That(node.ToStringRepresentation(), Is.EqualTo("(+ 1 2)"));
        });
    }

    /// <summary>
    /// Test subtraction (non-commutative operation) works correctly.
    /// </summary>
    [Test]
    public void Parser_Parse_Subtraction_ReturnsCorrectResult()
    {
        const string input = "(- 10 4)";
        var tree = Parser.Parse(input);
        Assert.Multiple(() =>
        {
            Assert.That(tree.ToStringRepresentation(), Is.EqualTo("(- 10 4)"));
            Assert.That(tree.Calculate(), Is.EqualTo(6));
        });
    }

    /// <summary>
    /// Test division (non-commutative operation) works correctly and doesn't swap operands.
    /// </summary>
    [Test]
    public void Parser_Parse_Division_ReturnsCorrectResult()
    {
        const string input = "(/ 10 2)";
        var tree = Parser.Parse(input);
        Assert.Multiple(() =>
        {
            Assert.That(tree.ToStringRepresentation(), Is.EqualTo("(/ 10 2)"));
            Assert.That(tree.Calculate(), Is.EqualTo(5));
        });
    }
}