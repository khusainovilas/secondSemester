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

        if (!AreParenthesesBalanced(input))
        {
            throw new ArgumentException("Unbalanced parentheses.");
        }

        var tokens = input
            .Replace('(', ' ')
            .Replace(')', ' ')
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var pos = 0;
        var node = ParseExpression(tokens, ref pos);

        if (pos != tokens.Length)
        {
            throw new ArgumentException("Extra tokens after parsing complete expression.");
        }

        return node;
    }

    private static IAbstractNode ParseExpression(string[] tokens, ref int pos)
    {
        if (pos >= tokens.Length)
        {
            throw new ArgumentException("Unexpected end of input.");
        }

        var token = tokens[pos++];
        if (int.TryParse(token, out var value))
        {
            return new NumberNode(value);
        }

        if (token.Length != 1 || !"+-*/".Contains(token[0]))
        {
            throw new ArgumentException($"Invalid operator: {token}");
        }

        var op = token[0];
        var left = ParseExpression(tokens, ref pos);
        var right = ParseExpression(tokens, ref pos);

        return new BinaryOperationNode(op, left, right);
    }

    private static bool AreParenthesesBalanced(string s)
    {
        var balance = 0;
        foreach (var c in s)
        {
            if (c == '(')
            {
                balance++;
            }

            if (c == ')')
            {
                balance--;
            }

            if (balance < 0)
            {
                return false;
            }
        }

        return balance == 0;
    }
}
