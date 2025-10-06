// <copyright file="Trie.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace Trie;

/// <summary>
/// realization of Trie.
/// </summary>
public class Trie
{
    private readonly TrieNode root = new();

    /// <summary>
    /// Gets the number of words added to the tree.
    /// </summary>
    public int WordCount { get; private set; }

    /// <summary>
    /// Add a new word into the trie.
    /// </summary>
    /// <param name="word">The word to be inserted.</param>
    /// <returns>Returns false if the word is already present.</returns>
    public bool Add(string? word)
    {
        if (word is null)
        {
            return false;
        }

        var current = this.root;

        foreach (var letter in word)
        {
            if (!current.Children.TryGetValue(letter, out var value))
            {
                value = new TrieNode();
                current.Children[letter] = value;
            }

            current = value;
            current.PrefixCount++;
        }

        if (current.IsEnd)
        {
            return false;
        }

        current.IsEnd = true;
        this.WordCount++;
        return true;
    }

    /// <summary>
    /// Determines whether the trie contains a specific word.
    /// </summary>
    /// <param name="word">The word to look for.</param>
    /// <returns>Returns true if the word exists in the trie.</returns>
    public bool Contains(string? word)
    {
        if (word is null || string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        var current = this.root;

        foreach (var letter in word)
        {
            if (!current.Children.TryGetValue(letter, out var value))
            {
                return false;
            }

            current = value;
        }

        return current.IsEnd;
    }

    /// <summary>
    /// Deletes a word from the trie.
    /// </summary>
    /// <param name="word">The word to delete.</param>
    /// <returns>Returns true if the word was present in the trie.</returns>
    public bool Remove(string? word)
    {
        if (word is null || string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        var current = this.root;
        var path = new Stack<TrieNode>();

        foreach (var letter in word)
        {
            if (!current.Children.TryGetValue(letter, out var value))
            {
                return false;
            }

            path.Push(current);
            current = value;
        }

        if (!current.IsEnd)
        {
            return false;
        }

        current.IsEnd = false;
        this.WordCount--;

        foreach (var trieNode in path)
        {
            trieNode.PrefixCount--;
        }

        current.PrefixCount--;

        return true;
    }

    /// <summary>
    /// Returns the number of words in the trie that start with a given prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>The count of words that begin with the specified prefix.</returns>
    public int HowManyStartsWithPrefix(string? prefix)
    {
        if (prefix is null || string.IsNullOrWhiteSpace(prefix))
        {
            return this.WordCount;
        }

        var current = this.root;

        foreach (var letter in prefix)
        {
            if (!current.Children.TryGetValue(letter, out var value))
            {
                return 0;
            }

            current = value;
        }

        return current.PrefixCount;
    }

    private class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new();

        public bool IsEnd { get; set; }

        public int PrefixCount { get; set; }
    }
}
