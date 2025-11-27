// <copyright file="Program.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

using SyntaxTree;

try
{
    if (args.Length != 1)
    {
        throw new ArgumentException("Exactly one argument is expected - the file path to the file.");
    }

    var filePath = args[0];

    if (!File.Exists(filePath))
    {
        throw new FileNotFoundException("File not found.", filePath);
    }

    var input = File.ReadAllText(filePath).Trim();
    var tree = Parser.Parse(input);

    Console.WriteLine($"The tree: {tree.ToStringRepresentation()}");
    Console.WriteLine($"The result: {tree.Calculate()}");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: Incorrect expression. {ex.Message}");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Mistake: Division by zero.");
}
catch (Exception)
{
    Console.WriteLine("Error: An unknown error has occurred.");
}