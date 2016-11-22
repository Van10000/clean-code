using FluentAssertions;
using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class MarkdownEnumerableExtensions_Should
    {
        [Test]
        public void SkipCharacters()
        {
            var markdown = new StringMarkdownEnumerable("abcde");

            markdown.SkipCharacters(2);

            markdown.GetNextChar().Should().Be('c');
        }

        [TestCase("ab _d", Tag.Italic, TagPosition.Opening, 0, 'd', TestName = "Italic opening tag")]
        [TestCase("ab_ d", Tag.Italic, TagPosition.Closing, 0, ' ', TestName = "Italic closing tag")]
        [TestCase("__d", Tag.Strong, TagPosition.Opening, 0, 'd', TestName = "Strong opening tag")]
        public void ParseUntilMeetTag(string markdown, Tag tag, TagPosition tagPosition, int tagPart, char symbolAfterTag)
        {
            var markdownEnumerable = ParseUntil(markdown, tag, new TagType(tagPosition, tagPart));

            markdownEnumerable.GetNextChar().Should().Be(symbolAfterTag);
        }

        [TestCase("_a", Tag.None, TagPosition.Opening, 0, TestName = "Tag is none")]
        [TestCase("_a", Tag.Italic, TagPosition.None, 0, TestName = "Tag position is none")]
        [TestCase("__", Tag.Strong, TagPosition.Opening, 0, TestName = "Tag is at the end")]
        [TestCase("abcd", Tag.Italic, TagPosition.Opening, 0, TestName = "No such tag")]
        [TestCase("abcd __e", Tag.Strong, TagPosition.Closing, 0, TestName = "Wrong tag position")]
        public void ParseUntilEnd(string markdown, Tag tag, TagPosition tagPosition, int tagPart)
        {
            var markdownEnumerable = ParseUntil(markdown, tag, new TagType(tagPosition, tagPart));

            markdownEnumerable.IsFinished().Should().BeTrue();
        }

        [TestCase("a _b", Tag.Italic, TagPosition.Opening, 0)]
        public void IfStoppedAtTag_ReturnCorrectStoppedAt(string markdown, Tag tag, TagPosition tagPosition, int tagPart)
        {
            var stoppedAt = ParseUntilGetStoppedAt(markdown, tag, new TagType(tagPosition, tagPart));

            stoppedAt.Tag.Should().Be(tag);
            stoppedAt.TagPosition.Should().Be(tagPosition);
        }

        [TestCase("a_ b", Tag.Strong, TagPosition.Opening, 0)]
        public void IfFinished_ReturnNoneStoppedAt(string markdown, Tag tag, TagPosition tagPosition, int tagPart)
        {
            var stoppedAt = ParseUntilGetStoppedAt(markdown, tag, new TagType(tagPosition, tagPart));

            stoppedAt.Tag.Should().Be(Tag.None);
            stoppedAt.TagPosition.Should().Be(TagPosition.None);
        }

        [Test]
        public void ParseUntilManyTags()
        {
            var tags = new[]
            {
                new TagInfo(Tag.Strong, TagPosition.Opening, 0),
                new TagInfo(Tag.Strong, TagPosition.Closing, 0),
                new TagInfo(Tag.Italic, TagPosition.Opening, 0)
            };
            var markdownEnumerable = new StringMarkdownEnumerable("a_ __b");

            TagInfo stoppedAt;
            string parsed = markdownEnumerable.ParseUntil(tags, out stoppedAt);

            parsed.Should().Be("a_ ");
        }

        private StringMarkdownEnumerable ParseUntil(string markdown, Tag tag, TagType tagPosition)
        {
            TagInfo stoppedAt;
            return ParseUntil(markdown, tag, tagPosition, out stoppedAt);
        }

        private TagInfo ParseUntilGetStoppedAt(string markdown, Tag tag, TagType tagPosition)
        {
            TagInfo stoppedAt;
            ParseUntil(markdown, tag, tagPosition, out stoppedAt);
            return stoppedAt;
        }

        private StringMarkdownEnumerable ParseUntil(string markdown, Tag tag, TagType tagPosition, out TagInfo stoppedAt)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            var tagInfo = new TagInfo(tag, tagPosition);

            markdownEnumerable.ParseUntil(new[] { tagInfo }, out stoppedAt);

            return markdownEnumerable;
        }
    }
}
