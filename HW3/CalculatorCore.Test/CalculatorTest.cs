// <copyright file="CalculatorTest.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace CalculatorCore.Test;

using NUnit.Framework;

/// <summary>
/// A set of unit tests to verify the business logic of the <see cref="CalculatorLogic"/> class.
/// These tests check arithmetic operations, error handling, and helper methods.
/// </summary>
public class CalculatorTest
{
    private CalculatorLogic calculator;

    /// <summary>
    /// Runs before each test to create a fresh calculator instance.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.calculator = new CalculatorLogic();
    }

    /// <summary>
    /// Tests that digits are appended correctly to the display.
    /// </summary>
    [Test]
    public void CalculatorLogic_AppendDigit_AppendsDigitsCorrectly()
    {
        this.calculator.AppendDigit("1");
        this.calculator.AppendDigit("2");

        Assert.That(this.calculator.Display, Is.EqualTo("12"));
    }

    /// <summary>
    /// Tests that invalid characters (non-numeric and not a dot) are ignored.
    /// </summary>
    [Test]
    public void CalculatorLogic_AppendDigit_InvalidCharacter_IsIgnored()
    {
        this.calculator.AppendDigit("a");

        Assert.That(this.calculator.Display, Is.EqualTo("0"));
    }

    /// <summary>
    /// Tests that only one decimal dot can be added.
    /// </summary>
    [Test]
    public void CalculatorLogic_AppendDigit_DoubleDot_IsIgnored()
    {
        this.calculator.AppendDigit("1");
        this.calculator.AppendDigit(".");
        this.calculator.AppendDigit(".");
        this.calculator.AppendDigit("5");

        Assert.That(this.calculator.Display, Is.EqualTo("1.5"));
    }

    /// <summary>
    /// Tests that addition returns the correct result.
    /// </summary>
    [Test]
    public void CalculatorLogic_SetOperator_Addition_ReturnsCorrectResult()
    {
        this.calculator.AppendDigit("2");
        this.calculator.SetOperator("+");
        this.calculator.AppendDigit("3");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("5"));
    }

    /// <summary>
    /// Tests that subtraction returns the correct result.
    /// </summary>
    [Test]
    public void CalculatorLogic_SetOperator_Subtraction_ReturnsCorrectResult()
    {
        this.calculator.AppendDigit("9");
        this.calculator.SetOperator("-");
        this.calculator.AppendDigit("4");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("5"));
    }

    /// <summary>
    /// Tests that multiplication returns the correct result.
    /// </summary>
    [Test]
    public void CalculatorLogic_SetOperator_Multiplication_ReturnsCorrectResult()
    {
        this.calculator.AppendDigit("6");
        this.calculator.SetOperator("*");
        this.calculator.AppendDigit("7");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("42"));
    }

    /// <summary>
    /// Tests that division returns the correct result.
    /// </summary>
    [Test]
    public void CalculatorLogic_SetOperator_Division_ReturnsCorrectResult()
    {
        this.calculator.AppendDigit("8");
        this.calculator.SetOperator("/");
        this.calculator.AppendDigit("2");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("4"));
    }

    /// <summary>
    /// Tests that division by zero results in an error message.
    /// </summary>
    [Test]
    public void CalculatorLogic_SetOperator_DivisionByZero_ShowsError()
    {
        this.calculator.AppendDigit("8");
        this.calculator.SetOperator("/");
        this.calculator.AppendDigit("0");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("Error"));
    }

    /// <summary>
    /// Tests that the Clear method resets the calculator state completely.
    /// </summary>
    [Test]
    public void CalculatorLogic_Clear_ResetsAllState()
    {
        this.calculator.AppendDigit("9");
        this.calculator.SetOperator("+");
        this.calculator.AppendDigit("1");
        this.calculator.Clear();

        Assert.That(this.calculator.Display, Is.EqualTo("0"));
    }

    /// <summary>
    /// Tests that ClearEnter only clears the current input, not the stored result.
    /// </summary>
    [Test]
    public void CalculatorLogic_ClearEnter_ClearsOnlyCurrentInput()
    {
        this.calculator.AppendDigit("9");
        this.calculator.SetOperator("+");
        this.calculator.AppendDigit("3");
        this.calculator.ClearEnter();

        Assert.That(this.calculator.Display, Is.EqualTo("9"));
    }

    /// <summary>
    /// Tests that Backspace removes the last entered digit.
    /// </summary>
    [Test]
    public void CalculatorLogic_Backspace_RemovesLastDigit()
    {
        this.calculator.AppendDigit("1");
        this.calculator.AppendDigit("2");
        this.calculator.AppendDigit("3");
        this.calculator.Backspace();

        Assert.That(this.calculator.Display, Is.EqualTo("12"));
    }

    /// <summary>
    /// Tests that Backspace on empty input keeps the display as "0".
    /// </summary>
    [Test]
    public void CalculatorLogic_Backspace_OnEmptyInput_KeepsZero()
    {
        this.calculator.Backspace();

        Assert.That(this.calculator.Display, Is.EqualTo("0"));
    }

    /// <summary>
    /// Tests that sequential operations use the previous result as the first operand.
    /// </summary>
    [Test]
    public void CalculatorLogic_Calculate_SequentialOperations_UsesPreviousResult()
    {
        this.calculator.AppendDigit("5");
        this.calculator.SetOperator("+");
        this.calculator.AppendDigit("5");
        this.calculator.Calculate();

        this.calculator.SetOperator("*");
        this.calculator.AppendDigit("2");
        this.calculator.Calculate();

        Assert.That(this.calculator.Display, Is.EqualTo("20"));
    }
}
