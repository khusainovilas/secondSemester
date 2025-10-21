// <copyright file="CalculatorLogic.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace CalculatorCore;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
/// Calculator logic that processes the input of numbers, operators, and calculations.
/// </summary>
public class CalculatorLogic : INotifyPropertyChanged
{
    private double? currentValue;
    private string? pendingOperator;
    private string? display = "0";
    private bool isNewInput = true;
    private string? inputBuffer = string.Empty;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the value displayed on the calculator screen.
    /// Updates the UI via data binding when changing.
    /// </summary>
    public string? Display
    {
        get => this.display;
        private set
        {
            if (this.display == value)
            {
                return;
            }

            this.display = value;
            this.OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Adds a digit or dot to the current input number.
    /// </summary>
    /// <param name="digit">Digit (0-9) or dot (.).</param>
    public void AppendDigit(string? digit)
    {
        if (digit != "." && !char.IsDigit(digit ?? string.Empty, 0))
        {
            return;
        }

        if (this.isNewInput)
        {
            this.inputBuffer = string.Empty;
            this.isNewInput = false;
        }

        if (digit == "." && this.inputBuffer != null && this.inputBuffer.Contains('.'))
        {
            return;
        }

        this.inputBuffer += digit;
        this.Display = this.inputBuffer.Length > 0 ? this.inputBuffer : "0";
    }

    /// <summary>
    /// Sets the operator (+, -, *, /) and performs the calculation if there are two operands.
    /// </summary>
    /// <param name="operator">The operator.</param>
    public void SetOperator(string @operator)
    {
        if (@operator != "+" && @operator != "-" && @operator != "*" && @operator != "/")
        {
            return;
        }

        if (!string.IsNullOrEmpty(this.inputBuffer))
        {
            if (!double.TryParse(this.inputBuffer, out var number))
            {
                this.Display = "Error";
                this.Clear();
                return;
            }

            if (this.currentValue == null)
            {
                this.currentValue = number;
            }
            else if (this.pendingOperator != null)
            {
                try
                {
                    this.currentValue = CalculateResult(this.currentValue!.Value, number, this.pendingOperator);
                    this.Display = this.currentValue.ToString();
                }
                catch (Exception)
                {
                    this.Display = "Error";
                    this.Clear();
                    return;
                }
            }

            this.inputBuffer = string.Empty;
            this.isNewInput = true;
        }

        this.pendingOperator = @operator;
        this.Display = this.currentValue?.ToString() ?? "0";
    }

    /// <summary>
    /// Performs the final calculation.
    /// </summary>
    public void Calculate()
    {
        if (string.IsNullOrEmpty(this.inputBuffer) || this.pendingOperator == null)
        {
            return;
        }

        if (!double.TryParse(this.inputBuffer, out var secondNumber))
        {
            this.Display = "Error";
            this.Clear();
            return;
        }

        try
        {
            this.currentValue = CalculateResult(this.currentValue!.Value, secondNumber, this.pendingOperator);
            this.Display = this.currentValue.ToString();
        }
        catch (Exception)
        {
            this.Display = "Error";
            this.Clear();
            return;
        }

        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.pendingOperator = null;
    }

    /// <summary>
    /// Performs the calculation of two numbers with the specified operator.
    /// </summary>
    /// <param name="first">The first operand.</param>
    /// <param name="second">The second operand.</param>
    /// <param name="operator">The operator.</param>
    /// <returns>The result of the calculation.</returns>
    private static double CalculateResult(double first, double second, string @operator)
    {
        return @operator switch
        {
            "+" => first + second,
            "-" => first - second,
            "*" => first * second,
            "/" => second != 0 ? first / second : throw new DivideByZeroException(),
            _ => throw new InvalidOperationException("Unknown operator"),
        };
    }

    /// <summary>
    /// Completely resets the calculator state.
    /// </summary>
    public void Clear()
    {
        this.currentValue = null;
        this.pendingOperator = null;
        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.Display = "0";
    }

    /// <summary>
    /// Resets the current input, saving the result and the operator.
    /// </summary>
    public void ClearEnter()
    {
        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.Display = this.currentValue?.ToString() ?? "0";
    }

    /// <summary>
    /// Removes the last digit from the current input.
    /// </summary>
    public void Backspace()
    {
        if (this.isNewInput || this.inputBuffer is { Length: <= 0 })
        {
            return;
        }

        this.inputBuffer = this.inputBuffer?[..^1];
        this.Display = this.inputBuffer is { Length: > 0 } ? this.inputBuffer : "0";
    }
}