// <copyright file="TrieTests.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace Trie.Tests;

public class TrieTest
{
    private Trie trie;

    /// <summary>
    /// Sets up the trie instance for every test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.trie = new Trie();
    }

    /// <summary>
    /// Test that adding a new word returns true.
    /// </summary>
    [Test]
    public void Trie_Add_NewWords_ReturnsTrue()
    {
        var words = new List<string> { "he", "his", "her", "hi" };

        foreach (var word in words)
        {
            Assert.That(this.trie.Add(word), Is.True);
        }

        Assert.That(this.trie.WordCount, Is.EqualTo(words.Count));
    }

    /// <summary>
    /// Checks that re-adding an existing word returns false.
    /// </summary>
    [Test]
    public void Trie_Add_ExistingWord_ShouldReturnFalse()
    {
        this.trie.Add("he");
        this.trie.Add("his");

        Assert.Multiple(() =>
        {
            Assert.That(this.trie.Add("he"), Is.False);
            Assert.That(this.trie.Add("his"), Is.False);
        });
    }

    /// <summary>
    /// Checks that adding null, an empty string, or a string with spaces returns false.
    /// </summary>
    [Test]
    public void Add_NullOrEmpty_ShouldReturnFalse()
    {
        Assert.Multiple(() =>
        {
            Assert.That(this.trie.Add(null), Is.False);
            Assert.That(this.trie.Add(string.Empty), Is.False);
            Assert.That(this.trie.Add(" "), Is.False);
        });
    }

    /// <summary>
    /// Checks if WordCount is correct after adding new words.
    /// </summary>
    [Test]
    public void Add_NewWords_ShouldUpdateWordCount()
    {
        this.trie.Add("he");
        this.trie.Add("his");
        this.trie.Add("her");

        Assert.That(this.trie.WordCount, Is.EqualTo(3));
    }
}