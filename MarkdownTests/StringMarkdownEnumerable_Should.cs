using System.Text;
using Markdown.MarkdownEnumerable;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTests
{
    [TestFixture]
    internal class StringMarkdownEnumerable_Should
    {
        [TestCase("abcde__f__e_e_ _d_ __e__", TestName = "Some long string")]
        [TestCase("", TestName = "Empty string")]
        [TestCase(@"ab\c\__d", TestName = "With escape symbols")]
        public void ReturnAllNextChars(string markdown)
        {
            var enumerable = new StringMarkdownEnumerable(markdown);

            var allChars = new StringBuilder();
            while (!enumerable.IsFinished())
                allChars.Append(enumerable.GetNextChar());

            allChars.ToString().Should().Be(markdown.Replace(@"\", ""));
        }

        [TestCase("e__", 1, Tag.Strong, TagType.Closing)]
        [TestCase(" f_ d ", 2, Tag.Italic, TagType.Closing)]
        [TestCase("", 0, Tag.None, TagType.None)]
        [TestCase("sadfasrw", 5, Tag.None, TagType.None)]
        [TestCase(" __c__ ", 1, Tag.Strong, TagType.Opening)]
        [TestCase("_", 0, Tag.Italic, TagType.Opening)]
        [TestCase("asdb wer", 2, Tag.None, TagType.None)]
        public void AtSpecifiedPosition_FindNextClosingTag(string markdown, int position, Tag expectedTag, TagType expectedType)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            markdownEnumerable.SkipCharacters(position);
            var expectedTagInfo = new TagInfo(expectedTag, expectedType);

            var returnedTagInfo = markdownEnumerable.GetNextTag(new[] { expectedTagInfo });

            returnedTagInfo.Should().Be(expectedTagInfo);
        }
    }
}
