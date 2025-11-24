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

        if (digit is "," or ".")
        {
            digit = ",";
        }

        if (digit != "," && !char.IsDigit(digit ?? string.Empty, 0))
        {
            return;
        }

        if (this.isNewInput)
        {
            this.inputBuffer = string.Empty;
            this.isNewInput = false;
        }

        switch (digit)
        {
            case "," when this.inputBuffer.Contains(','):
                return;
            case "," when string.IsNullOrEmpty(this.inputBuffer):
                this.inputBuffer = "0,";
                break;
            default:
                this.inputBuffer += digit;
                break;
        }

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
            if (!TryParseInput(this.inputBuffer, out var number))
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
                if (!TryCalculate(this.currentValue.Value, number, this.pendingOperator, out var result))
                {
                    this.DisplayError();
                    return;
                }

                this.currentValue = result;
                this.Display = ConvertToDisplay(result);
            }

            this.inputBuffer = string.Empty;
            this.isNewInput = true;
        }

        this.pendingOperator = @operator;
        this.Display = this.currentValue != null ? ConvertToDisplay(this.currentValue.Value) : "0";
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

        if (this.inputBuffer.EndsWith(','))
        {
            this.inputBuffer = this.inputBuffer[..^1];
        }

        if (!TryParseInput(this.inputBuffer, out var secondNumber))
        {
            this.DisplayError();
            return;
        }

        if (this.currentValue == null)
        {
            this.currentValue = secondNumber;
            this.Display = ConvertToDisplay(secondNumber);
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
        this.Display = ConvertToDisplay(result);

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

        if (this.isNewInput && string.IsNullOrEmpty(this.inputBuffer))
        {
            this.currentValue = null;
            this.pendingOperator = null;
            this.Display = "0";
            return;
        }

        this.inputBuffer = string.Empty;
        this.isNewInput = true;
        this.Display = this.currentValue != null ? ConvertToDisplay(this.currentValue.Value) : "0";
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

        if (this.isNewInput && string.IsNullOrEmpty(this.inputBuffer))
        {
            this.inputBuffer = this.Display ?? "0";
            this.isNewInput = false;
            this.currentValue = null;
            this.pendingOperator = null;
        }

        if (this.inputBuffer.Length <= 0)
        {
            this.Display = "0";
            return;
        }

        this.inputBuffer = this.inputBuffer[..^1];
        this.Display = this.inputBuffer.Length > 0 ? this.inputBuffer : "0";
    }

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

    private static bool TryParseInput(string input, out double number)
    {
        return double.TryParse(input, NumberStyles.Float, new CultureInfo("ru-RU"), out number);
    }

    private static string ConvertToDisplay(double number)
    {
        return number.ToString(new CultureInfo("ru-RU"));
    }

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
