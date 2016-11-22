using System.Text;
using Markdown.MarkdownEnumerable;
using NUnit.Framework;
using FluentAssertions;
using Markdown.MarkdownEnumerable.Tags;

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

        [TestCase("e__", 1, Tag.Strong, TagPosition.Closing, 0)]
        [TestCase(" f_ d ", 2, Tag.Italic, TagPosition.Closing, 0)]
        [TestCase("", 0, Tag.None, TagPosition.None, 0)]
        [TestCase("sadfasrw", 5, Tag.None, TagPosition.None, 0)]
        [TestCase(" __c__ ", 1, Tag.Strong, TagPosition.Opening, 0)]
        [TestCase("_", 0, Tag.Italic, TagPosition.Opening, 0)]
        [TestCase("asdb wer", 2, Tag.None, TagPosition.None, 0)]
        public void AtSpecifiedPosition_FindNextTag(string markdown, int position, Tag expectedTag, 
            TagPosition expectedPosition, int expectedPart)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            markdownEnumerable.SkipCharacters(position);
            var expectedTagInfo = new TagInfo(expectedTag, new TagType(expectedPosition, expectedPart));

            var returnedTagInfo = markdownEnumerable.GetNextTag(new[] { expectedTagInfo });

            returnedTagInfo.Should().Be(expectedTagInfo);
        }
    }
}
