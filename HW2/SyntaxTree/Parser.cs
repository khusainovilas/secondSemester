// <copyright file="Parser.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace SyntaxTree;

/// <summary>
/// Parser for building the syntax tree from a string representation.
/// </summary>
public static class Parser
{
    /// <summary>
    /// Parses the input string into a syntax tree node.
    /// </summary>
    /// <param name="input">The input string in prefix notation.</param>
    /// <returns>The root node of the syntax tree.</returns>
    public static IAbstractNode Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty or whitespace.");
        }

        var tokens = Tokenize(input);
        var index = 0;
        var result = ParseExpression(tokens, ref index);

        if (index != tokens.Count)
        {
            throw new ArgumentException("Extra tokens after parsing complete expression.");
        }

        return result;
    }

    /// <summary>
    /// Tokenizes the input string into a list of tokens.
    /// </summary>
    /// <param name="input">The input string to tokenize.</param>
    /// <returns>A list of tokens.</returns>
    private static List<string> Tokenize(string input)
    {
        var tokens = new List<string>();
        var currentIndex = 0;

        while (currentIndex < input.Length)
        {
            var currentChar = input[currentIndex];

            if (char.IsWhiteSpace(currentChar))
            {
                currentIndex++;
                continue;
            }

            switch (currentChar)
            {
                case '(':
                case ')':
                case '+':
                case '*':
                case '/':
                    tokens.Add(currentChar.ToString());
                    currentIndex++;
                    continue;

                case '-':
                {
                    if (currentIndex + 1 < input.Length && char.IsDigit(input[currentIndex + 1]))
                    {
                        var start = currentIndex;
                        currentIndex++;
                        while (currentIndex < input.Length && char.IsDigit(input[currentIndex]))
                        {
                            currentIndex++;
                        }

                        tokens.Add(input.Substring(start, currentIndex - start));
                    }
                    else
                    {
                        tokens.Add(currentChar.ToString());
                        currentIndex++;
                    }

                    continue;
                }
            }

            if (char.IsDigit(currentChar))
            {
                var start = currentIndex;
                while (currentIndex < input.Length && char.IsDigit(input[currentIndex]))
                {
                    currentIndex++;
                }

                tokens.Add(input.Substring(start, currentIndex - start));
                continue;
            }

            throw new ArgumentException($"Invalid character '{currentChar}' at position {currentIndex}.");
        }

        return tokens;
    }

    // <summary>
    // Parse a list of tokens.
    // </summary>
    // <param name="tokens">The list of tokens.</param>
    // <param name="index">The current token index.</param>
    // <returns>The parsed node.</returns>
    private static IAbstractNode ParseExpression(List<string> tokens, ref int index)
    {
        var operators = new Stack<string>();
        var nodes = new Stack<IAbstractNode>();

        while (index < tokens.Count)
        {
            var token = tokens[index];

            if (int.TryParse(token, out var value))
            {
                nodes.Push(new NumberNode(value));
                index++;
            }
            else if (token == "(")
            {
                operators.Push(token);
                index++;
            }
            else if ("+-*/".Contains(token))
            {
                if (operators.Count == 0 || operators.Peek() != "(")
                {
                    throw new ArgumentException($"Expected '(', found '{token}' at position {index}.");
                }

                operators.Push(token);
                index++;
            }
            else if (token == ")")
            {
                if (operators.Count < 2 || operators.Peek() == "(")
                {
                    throw new ArgumentException($"Missing operation before ')' at position {index}.");
                }

                var op = operators.Pop();
                if (operators.Peek() != "(")
                {
                    throw new ArgumentException($"Missing opening '(' before position {index}.");
                }

                operators.Pop();

                if (nodes.Count < 2)
                {
                    throw new ArgumentException($"Missing operands for operation '{op}' at position {index}.");
                }

                var right = nodes.Pop();
                var left = nodes.Pop();
                nodes.Push(new BinaryOperationNode(op[0], left, right));
                index++;
            }
            else
            {
                throw new ArgumentException($"Unexpected token '{token}' at position {index}.");
            }
        }

        if (operators.Count > 0)
        {
            throw new ArgumentException("Missing closing ')'.");
        }

        if (nodes.Count != 1)
        {
            throw new ArgumentException("Incomplete expression.");
        }

        return nodes.Pop();
    }
}
