using System;
using Markdown.MarkdownEnumerable;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class ParsingUtils_Should
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
        public bool DetectOpeningTag(Tag tag, string str, int pos)
        {
            var tagInfo = new TagInfo(tag, TagType.Opening);
            return MarkdownParsingUtils.IsCorrectTag(tagInfo, str, pos);
        }

        [TestCase(Tag.Italic, "c_", 1, ExpectedResult = true, TestName = "Simple italic closing tag")]
        [TestCase(Tag.Strong, "__", 0, ExpectedResult = true, TestName = "Lonely strong tag")]
        [TestCase(Tag.Italic, "a _b", 2, ExpectedResult = false, TestName = "Gap between character and tag")]
        [TestCase(Tag.Italic, "a_ b", 1, ExpectedResult = true, TestName = "Gap in another side")]
        public bool DetectClosingTag(Tag tag, string str, int pos)
        {
            var tagInfo = new TagInfo(tag, TagType.Closing);
            return MarkdownParsingUtils.IsCorrectTag(tagInfo, str, pos);
        }

        [TestCase("yandex.ru", ExpectedResult = "yandex.ru", TestName = "Simple link")]
        [TestCase("", ExpectedResult = "", TestName = "Empty link is also link")] // because of relative links
        [TestCase(" ya.ru ", ExpectedResult = "ya.ru", TestName = "Spaces in the beginning and at the and.")]
        [TestCase("ya. ru", ExpectedResult = null, TestName = "Incorrect link")]
        public string ReturnCorrectLink(string link)
        {
            return MarkdownParsingUtils.ToCorrectLink(link);
        }

        [TestCase("documents", "", ExpectedResult = "documents/", TestName = "Empty relative path")]
        [TestCase("a/b/c", "/d/e", ExpectedResult = "a/b/c/d/e", TestName = "Simple concatenation")]
        [TestCase("A/B/C", "D/E", ExpectedResult = "A/B/C/D/E", TestName = "Both strings without '/' at the end")]
        [TestCase("a/b/c/", "/d/e/", ExpectedResult ="a/b/c/d/e/", TestName = "Both strings with '/' at the end")]
        public string CombineLinks(string baseUrl, string relativePath)
        {
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativePath);
        }
    }
}
