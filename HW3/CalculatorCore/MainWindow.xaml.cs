// <copyright file="MainWindow.xaml.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace CalculatorCore;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Logic of the main calculator window.
/// </summary>
public partial class MainWindow
{
    private readonly CalculatorLogic calculatorLogic;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.calculatorLogic = new CalculatorLogic();
        this.DataContext = this.calculatorLogic;
    }

    /// <summary>
    /// Handles button clicks.
    /// </summary>
    /// <param name="sender">Button.</param>
    /// <param name="eventArguments">Event arguments.</param>
    private void Button_Click(object sender, RoutedEventArgs eventArguments)
    {
        if (sender is not Button button)
        {
            return;
        }

        var buttonContent = button.Content.ToString();

        if (string.IsNullOrEmpty(buttonContent))
        {
            return;
        }

        if (char.IsDigit(buttonContent, 0) || buttonContent == ".")
        {
            this.calculatorLogic.AppendDigit(buttonContent);
        }
        else
        {
            switch (buttonContent)
        {
            case "+":
            case "-":
            case "*":
            case "/":
                this.calculatorLogic.SetOperator(buttonContent);
                break;
            case "=":
                this.calculatorLogic.Calculate();
                break;
            case "C":
                this.calculatorLogic.Clear();
                break;
            case "CE":
                this.calculatorLogic.ClearEnter();
                break;
            case "←":
                this.calculatorLogic.Backspace();
                break;
        }
        }
    }
}