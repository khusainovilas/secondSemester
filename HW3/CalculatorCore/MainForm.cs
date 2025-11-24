// <copyright file="MainForm.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace CalculatorCore;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// The main WinForms form calculator.
/// </summary>
public class MainForm : Form
{
    private readonly CalculatorLogic calculatorLogic;
    private readonly TextBox displayTextBox;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// Form constructor, initializes the UI and data binding.
    /// </summary>
    public MainForm()
    {
        this.calculatorLogic = new CalculatorLogic();
        this.Size = new Size(300, 400);
        this.Text = "CalculatorApp";
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        this.displayTextBox = new TextBox
        {
            Location = new Point(10, 10),
            Size = new Size(260, 40),
            Font = new Font("Arial", 20),
            TextAlign = HorizontalAlignment.Right,
            ReadOnly = true,
        };
        this.Controls.Add(this.displayTextBox);

        this.displayTextBox.DataBindings.Add(nameof(this.Text), this.calculatorLogic, "Display", false, DataSourceUpdateMode.OnPropertyChanged);

        var buttonPanel = new TableLayoutPanel
        {
            Location = new Point(10, 60),
            Size = new Size(260, 300),
            RowCount = 5,
            ColumnCount = 4,
        };
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        this.Controls.Add(buttonPanel);

        this.CreateButton(buttonPanel, "7", 0, 0);
        this.CreateButton(buttonPanel, "8", 0, 1);
        this.CreateButton(buttonPanel, "9", 0, 2);
        this.CreateButton(buttonPanel, "/", 0, 3);

        this.CreateButton(buttonPanel, "4", 1, 0);
        this.CreateButton(buttonPanel, "5", 1, 1);
        this.CreateButton(buttonPanel, "6", 1, 2);
        this.CreateButton(buttonPanel, "*", 1, 3);

        this.CreateButton(buttonPanel, "1", 2, 0);
        this.CreateButton(buttonPanel, "2", 2, 1);
        this.CreateButton(buttonPanel, "3", 2, 2);
        this.CreateButton(buttonPanel, "-", 2, 3);

        this.CreateButton(buttonPanel, "0", 3, 0);
        this.CreateButton(buttonPanel, ".", 3, 1);
        this.CreateButton(buttonPanel, "=", 3, 2);
        this.CreateButton(buttonPanel, "+", 3, 3);

        this.CreateButton(buttonPanel, "C", 4, 0);
        this.CreateButton(buttonPanel, "CE", 4, 1);
        this.CreateButton(buttonPanel, "←", 4, 2);
    }

    /// <inheritdoc />
    [AllowNull]
    public sealed override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>
    /// Creates a button and adds it to the grid, with a click handler.
    /// </summary>
    /// <param name="panel">Grid for the button.</param>
    /// <param name="content">Button content.</param>
    /// <param name="row">Row in the grid.</param>
    /// <param name="column">Column in the grid.</param>
    private void CreateButton(TableLayoutPanel panel, string content, int row, int column)
    {
        var button = new Button
        {
            Text = content,
            Font = new Font("Arial", 14),
            Dock = DockStyle.Fill,
        };
        button.Click += this.Button_Click!;
        panel.Controls.Add(button, column, row);
    }

    /// <summary>
    /// Handles a button click by calling logic methods.
    /// </summary>
    /// <param name="sender">Button.</param>
    /// <param name="e">Event arguments.</param>
    private void Button_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var buttonContent = button.Text;

        if (char.IsDigit(buttonContent, 0) || buttonContent == ".")
        {
            this.calculatorLogic.AppendDigit(buttonContent);
        }
        else
        {
            switch (buttonContent)
        {
            case "+" or "-" or "*" or "/":
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