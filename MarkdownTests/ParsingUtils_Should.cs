using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class ParsingUtils_Should
    {
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
        [TestCase("a/b/c/", "/d/e/", ExpectedResult = "a/b/c/d/e/", TestName = "Both strings with '/' at the end")]
        public string CombineLinks(string baseUrl, string relativePath)
        {
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativePath);
        }
    }
}
