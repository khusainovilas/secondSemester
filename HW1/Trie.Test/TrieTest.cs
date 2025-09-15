// <copyright file="TrieTest.cs" company="khusainovilas">
// Copyright (c) khusainovilas. All rights reserved.
// </copyright>

namespace Trie.Tests;

/// <summary>
/// Unit tests for Trie data structure.
/// </summary>
public class TrieTest
{
    private Trie trie;

    /// <summary>
    /// Initializes a new trie before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.trie = new Trie();
    }

    /// <summary>
    /// Tests Add method when inserting fresh words.
    /// </summary>
    [Test]
    public void Trie_Add_NewWords_ShouldReturnTrue()
    {
        List<string> words = ["apple", "app", "application"];

        foreach (var word in words)
        {
            Assert.That(this.trie.Add(word), Is.True);
        }
    }

    /// <summary>
    /// Tests WordCount after adding multiple words.
    /// </summary>
    [Test]
    public void Trie_WordCount_AfterAddingSeveralWords()
    {
        List<string> words = ["alpha", "beta", "gamma", "delta"];

        foreach (var word in words)
        {
            this.trie.Add(word);
        }

        Assert.That(this.trie.WordCount, Is.EqualTo(words.Count));
    }

    /// <summary>
    /// Tests Add method when the same word is inserted twice.
    /// </summary>
    [Test]
    public void Trie_Add_SameWordTwice_ShouldReturnFalse()
    {
        const string word = "matrix";

        this.trie.Add(word);

        Assert.That(this.trie.Add(word), Is.False);
    }

    /// <summary>
    /// Tests Add method after removing a word.
    /// </summary>
    [Test]
    public void Trie_Add_AfterRemovingWord_ShouldReturnTrue()
    {
        const string word = "sun";

        this.trie.Add(word);
        this.trie.Remove(word);

        Assert.That(this.trie.Add(word), Is.True);
    }

    /// <summary>
    /// Tests WordCount after removing words.
    /// </summary>
    [Test]
    public void Trie_WordCount_AfterRemovingSomeWords()
    {
        List<string> words = ["dog", "cat", "bird", "fish"];

        foreach (var word in words)
        {
            this.trie.Add(word);
        }

        this.trie.Remove("dog");
        this.trie.Remove("bird");

        var expectedCount = words.Count - 2;

        Assert.That(this.trie.WordCount, Is.EqualTo(expectedCount));
    }

    /// <summary>
    /// Tests WordCount of an empty trie.
    /// </summary>
    [Test]
    public void Trie_WordCount_OfEmptyTrie_ShouldBeZero()
    {
        const int expected = 0;

        Assert.That(this.trie.WordCount, Is.EqualTo(expected));
    }

    /// <summary>
    /// Tests Contains method after adding a word.
    /// </summary>
    [Test]
    public void Trie_Contains_AfterAddingWord_ShouldReturnTrue()
    {
        const string word = "cloud";

        this.trie.Add(word);

        Assert.That(this.trie.Contains(word), Is.True);
    }

    /// <summary>
    /// Tests Contains method with a word that was never added.
    /// </summary>
    [Test]
    public void Trie_Contains_NonExistingWord_ShouldReturnFalse()
    {
        const string word = "universe";

        Assert.That(this.trie.Contains(word), Is.False);
    }

    /// <summary>
    /// Tests Contains method after removing a word.
    /// </summary>
    [Test]
    public void Trie_Contains_AfterRemovingWord_ShouldReturnFalse()
    {
        const string word = "planet";

        this.trie.Add(word);
        this.trie.Remove(word);

        Assert.That(this.trie.Contains(word), Is.False);
    }

    /// <summary>
    /// Tests Remove method with a word that exists.
    /// </summary>
    [Test]
    public void Trie_Remove_ExistingWord_ShouldRemoveSuccessfully()
    {
        const string word = "river";

        this.trie.Add(word);
        this.trie.Remove(word);

        Assert.That(this.trie.Contains(word), Is.False);
    }

    /// <summary>
    /// Tests Remove method with a word that does not exist.
    /// </summary>
    [Test]
    public void Trie_Remove_NonExistingWord_ShouldReturnFalse()
    {
        const string word = "mountain";

        Assert.That(this.trie.Remove(word), Is.False);
    }
}