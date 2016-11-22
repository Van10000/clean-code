using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class ParsingUtils_Should
    {
        [TestCase(Tag.Strong, 0, " __c", 1, ExpectedResult = true, TestName = "Simple strong tag")]
        [TestCase(Tag.Strong, 0, " __c", 2, ExpectedResult = false, TestName = "Strong tag at wrong position")]
        [TestCase(Tag.Strong, 0, " _c", 1, ExpectedResult = false, TestName = "Italic is not strong")]
        [TestCase(Tag.Italic, 0, " _d", 1, ExpectedResult = true, TestName = "Simple italic opening tag")]
        [TestCase(Tag.Italic, 0, " __d", 1, ExpectedResult = false, TestName = "Strong is not italic")]
        [TestCase(Tag.Italic, 0, "_ d", 0, ExpectedResult = false, TestName = "Gap between tag and character")]
        [TestCase(Tag.Italic, 0, "_", 0, ExpectedResult = true, TestName = "Italic in one symbol string")]
        [TestCase(Tag.Italic, 0, "a__ ", 1, ExpectedResult = false, TestName = "Italic when space after '__'")]
        [TestCase(Tag.Italic, 0, "_1", 0, ExpectedResult = false, TestName = "Tag before number")]
        [TestCase(Tag.Strong, 0, "2__", 1, ExpectedResult = false, TestName = "Tag after number")]
        [TestCase(Tag.Italic, 0, "a_a_a", 1, ExpectedResult = false, TestName = "Tag between non space symbols")]
        [TestCase(Tag.Italic, 0, "__a__", 4, ExpectedResult = false, TestName = "Italic near to _")]
        [TestCase(Tag.Hyperlink, HyperlinkTagConstants.VALUE_PART, "[a](b)", 0, ExpectedResult = true, TestName = "Simple hyperlink start")]
        public bool DetectOpeningTag(Tag tag, int tagPart, string str, int pos)
        {
            var tagInfo = new TagInfo(tag, new TagType(TagPosition.Opening, tagPart));
            return MarkdownParsingUtils.IsCorrectTag(tagInfo, str, pos);
        }

        [TestCase(Tag.Italic, 0, "c_", 1, ExpectedResult = true, TestName = "Simple italic closing tag")]
        [TestCase(Tag.Strong, 0, "__", 0, ExpectedResult = true, TestName = "Lonely strong tag")]
        [TestCase(Tag.Italic, 0, "a _b", 2, ExpectedResult = false, TestName = "Gap between character and tag")]
        [TestCase(Tag.Italic, 0, "a_ b", 1, ExpectedResult = true, TestName = "Gap in another side")]
        public bool DetectClosingTag(Tag tag, int tagPart, string str, int pos)
        {
            var tagInfo = new TagInfo(tag, new TagType(TagPosition.Closing, tagPart));
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
        [TestCase("a/b/c/", "/d/e/", ExpectedResult = "a/b/c/d/e/", TestName = "Both strings with '/' at the end")]
        public string CombineLinks(string baseUrl, string relativePath)
        {
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativePath);
        }
    }
}
