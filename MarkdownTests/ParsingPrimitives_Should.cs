﻿using System;
using Markdown.MarkdownEnumerable;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class ParsingPrimitives_Should
    {
        [TestCase(Tag.Strong, " __c", 1, ExpectedResult = true, TestName = "Simple strong tag")]
        [TestCase(Tag.Strong, " __c", 2, ExpectedResult = false, TestName = "Strong tag at wrong position")]
        [TestCase(Tag.Strong, " _c", 1, ExpectedResult = false, TestName = "Italic is not strong")]
        [TestCase(Tag.Italic, " _d", 1, ExpectedResult = true, TestName = "Simple italic opening tag")]
        [TestCase(Tag.Italic, " __d", 1, ExpectedResult = false, TestName = "Strong is not italic")]
        [TestCase(Tag.Italic, "_ d", 0, ExpectedResult = false, TestName = "Gap between tag and character")]
        [TestCase(Tag.Italic, "_", 0, ExpectedResult = true, TestName = "Italic in one symbol string")]
        [TestCase(Tag.Italic, "a__ ", 1, ExpectedResult = false, TestName = "Italic when space after '__'")]
        [TestCase(Tag.Italic, "_1", 0, ExpectedResult = false, TestName = "Tag before number")]
        [TestCase(Tag.Strong, "2__", 1, ExpectedResult = false, TestName = "Tag after number")]
        [TestCase(Tag.Italic, "a_a_a", 1, ExpectedResult = false, TestName = "Tag between non space symbols")]
        [TestCase(Tag.Italic, "__a__", 4, ExpectedResult = false, TestName = "Italic near to _")]
        public bool Detect_OpeningTag(Tag tag, string str, int pos)
        {
            return MarkdownParsingUtils.IsOpeningTag(tag, str, pos);
        }

        [TestCase(Tag.Italic, "c_", 1, ExpectedResult = true, TestName = "Simple italic closing tag")]
        [TestCase(Tag.Strong, "__", 0, ExpectedResult = true, TestName = "Lonely strong tag")]
        [TestCase(Tag.Italic, "a _b", 2, ExpectedResult = false, TestName = "Gap between character and tag")]
        [TestCase(Tag.Italic, "a_ b", 1, ExpectedResult = true, TestName = "Gap in another side")]
        public bool Detect_ClosingTag(Tag tag, string str, int pos)
        {
            return MarkdownParsingUtils.IsClosingTag(tag, str, pos);
        }

        [Test]
        public void ThrowException_IfNone()
        {
            Assert.Throws<ArgumentException>(() => MarkdownParsingUtils.IsOpeningTag(Tag.None, "asdasdff", 2));
        }
    }
}