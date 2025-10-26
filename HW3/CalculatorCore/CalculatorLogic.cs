// <copyright file="CalculatorLogic.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace CalculatorCore;

using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

/// <summary>
/// Handles the calculator's core logic: input processing, arithmetic operations, and display updates.
/// </summary>
public class CalculatorLogic : INotifyPropertyChanged
{
    private double? currentValue;
    private string? pendingOperator;
    private string? display = "0";
    private bool isNewInput = true;
    private string inputBuffer = string.Empty;
    private bool hasError;

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the value shown on the calculator screen.
    /// Automatically updates the UI when changed.
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

    /// <summary>
    /// Appends a digit or a decimal point to the current number.
    /// </summary>
    /// <param name="digit">The digit (0–9) or decimal point (".").</param>
    public void AppendDigit(string? digit)
    {
        if (this.hasError)
        {
            this.Clear();
        }

        if (digit != "." && !char.IsDigit(digit ?? string.Empty, 0))
        {
            return;
        }

        if (this.isNewInput)
        {
            this.inputBuffer = string.Empty;
            this.isNewInput = false;
        }

        if (digit == "." && this.inputBuffer.Contains('.'))
        {
            return;
        }

        this.inputBuffer += digit;
        this.Display = this.inputBuffer.Length > 0 ? this.inputBuffer : "0";
    }

    /// <summary>
    /// Sets the operator (+, -, *, /) and, if possible, performs the intermediate calculation.
    /// </summary>
    /// <param name="operator">The operator symbol.</param>
    public void SetOperator(string @operator)
    {
        if (this.hasError)
        {
            this.Clear();
        }

        if (@operator != "+" && @operator != "-" && @operator != "*" && @operator != "/")
        {
            return;
        }

        if (!string.IsNullOrEmpty(this.inputBuffer))
        {
            if (!double.TryParse(this.inputBuffer, out var number))
            {
                this.DisplayError();
                return;
            }

            if (this.currentValue == null)
            {
                this.currentValue = number;
            }
            else if (this.pendingOperator != null)
            {
                if (!TryCalculate(this.currentValue!.Value, number, this.pendingOperator, out var result))
                {
                    this.DisplayError();
                    return;
                }

                this.currentValue = result;
                this.Display = this.currentValue.ToString();
            }

            this.inputBuffer = string.Empty;
            this.isNewInput = true;
        }

        this.pendingOperator = @operator;
        this.Display = this.currentValue?.ToString() ?? "0";
    }

    /// <summary>
    /// Performs the final calculation and updates the display.
    /// </summary>
    public void Calculate()
    {
        if (this.hasError)
        {
            return;
        }

        if (this.pendingOperator == null || string.IsNullOrEmpty(this.inputBuffer))
        {
            return;
        }

        if (!double.TryParse(this.inputBuffer, out var secondNumber))
        {
            this.DisplayError();
            return;
        }

        if (this.currentValue == null)
        {
            this.currentValue = secondNumber;
            this.Display = secondNumber.ToString(CultureInfo.InvariantCulture);
            this.inputBuffer = string.Empty;
            this.isNewInput = true;
            this.pendingOperator = null;
            return;
        }

        if (!TryCalculate(this.currentValue.Value, secondNumber, this.pendingOperator, out var result))
        {
            this.DisplayError();
            return;
        }

        this.currentValue = result;
        this.Display = result.ToString(CultureInfo.InvariantCulture);

        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.pendingOperator = null;
    }



    /// <summary>
    /// Completely clears the calculator state.
    /// </summary>
    public void Clear()
    {
        this.currentValue = null;
        this.pendingOperator = null;
        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.hasError = false;
        this.Display = "0";
    }

    /// <summary>
    /// Clears the current entry while keeping the stored result and operator.
    /// </summary>
    public void ClearEnter()
    {
        if (this.hasError)
        {
            this.Clear();
            return;
        }

        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.Display = this.currentValue?.ToString() ?? "0";
    }

    /// <summary>
    /// Deletes the last entered digit.
    /// </summary>
    public void Backspace()
    {
        if (this.hasError)
        {
            this.Clear();
            return;
        }

        if (this.isNewInput || this.inputBuffer.Length <= 0)
        {
            return;
        }

        this.inputBuffer = this.inputBuffer[..^1];
        this.Display = this.inputBuffer.Length > 0 ? this.inputBuffer : "0";
    }

    /// <summary>
    /// Attempts to calculate a result using two operands and an operator.
    /// </summary>
    private static bool TryCalculate(double first, double second, string @operator, out double result)
    {
        try
        {
            result = @operator switch
            {
                "+" => first + second,
                "-" => first - second,
                "*" => first * second,
                "/" => second != 0 ? first / second : throw new DivideByZeroException(),
                _ => throw new InvalidOperationException("Unknown operator"),
            };
            return true;
        }
        catch
        {
            result = 0;
            return false;
        }
    }

    /// <summary>
    /// Displays "Error" and blocks further input until user starts new entry.
    /// </summary>
    private void DisplayError()
    {
        this.Display = "Error";
        this.hasError = true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
