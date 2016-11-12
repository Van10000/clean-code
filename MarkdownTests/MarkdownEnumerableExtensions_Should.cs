using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.MarkdownEnumerable;
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

        [Test]
        public void SkipNextOpeningTag()
        {
            var markdown = new StringMarkdownEnumerable("abc __de");

            markdown.SkipCharacters(4);
            markdown.SkipNextOpeningTag();

            markdown.GetNextChar().Should().Be('d');
        }

        [Test]
        public void SkipNextClosingTag()
        {
            var markdown = new StringMarkdownEnumerable(" abc__");

            markdown.SkipCharacters(4);
            markdown.SkipNextClosingTag();

            markdown.IsFinished().Should().BeTrue();
        }
        
        [TestCase("ab _d", Tag.Italic, TagType.Opening, 'd', TestName = "Italic opening tag")]
        [TestCase("ab_ d", Tag.Italic, TagType.Closing, ' ', TestName = "Italic closing tag")]
        [TestCase("__d", Tag.Strong, TagType.Opening, 'd', TestName = "Strong opening tag")]
        public void ParseUntilMeetTag(string markdown, Tag tag, TagType tagType, char symbolAfterTag)
        {
            var markdownEnumerable = ParseUntil(markdown, tag, tagType);

            markdownEnumerable.GetNextChar().Should().Be(symbolAfterTag);
        }

        [TestCase("_a", Tag.None, TagType.Opening, TestName = "Tag is none")]
        [TestCase("_a", Tag.Italic, TagType.None, TestName = "Tag type is none")]
        [TestCase("__", Tag.Strong, TagType.Opening, TestName = "Tag is at the end")]
        [TestCase("abcd", Tag.Italic, TagType.Opening, TestName = "No such tag")]
        [TestCase("abcd __e", Tag.Strong, TagType.Closing, TestName = "Wrong tag type")]
        public void ParseUntilEnd(string markdown, Tag tag, TagType tagType)
        {
            var markdownEnumerable = ParseUntil(markdown, tag, tagType);

            markdownEnumerable.IsFinished().Should().BeTrue();
        }

        [TestCase("a _b", Tag.Italic, TagType.Opening)]
        public void IfStoppedAtTag_ReturnCorrectStoppedAt(string markdown, Tag tag, TagType tagType)
        {
            var stoppedAt = ParseUntilGetStoppedAt(markdown, tag, tagType);

            stoppedAt.Tag.Should().Be(tag);
            stoppedAt.TagType.Should().Be(tagType);
        }

        [TestCase("a_ b", Tag.Strong, TagType.Opening)]
        public void IfFinished_ReturnNoneStoppedAt(string markdown, Tag tag, TagType tagType)
        {
            var stoppedAt = ParseUntilGetStoppedAt(markdown, tag, tagType);

            stoppedAt.Tag.Should().Be(Tag.None);
            stoppedAt.TagType.Should().Be(TagType.None);
        }

        [Test]
        public void ParseUntilManyTags()
        {
            var tags = new []
            {
                new TagInfo(Tag.Strong, TagType.Opening),
                new TagInfo(Tag.Strong, TagType.Closing),
                new TagInfo(Tag.Italic, TagType.Opening)
            };
            var markdownEnumerable = new StringMarkdownEnumerable("a_ __b");

            TagInfo stoppedAt;
            string parsed = markdownEnumerable.ParseUntil(tags, out stoppedAt);

            parsed.Should().Be("a_ ");
        }

        private StringMarkdownEnumerable ParseUntil(string markdown, Tag tag, TagType tagType)
        {
            TagInfo stoppedAt;
            return ParseUntil(markdown, tag, tagType, out stoppedAt);
        }

        private TagInfo ParseUntilGetStoppedAt(string markdown, Tag tag, TagType tagType)
        {
            TagInfo stoppedAt;
            ParseUntil(markdown, tag, tagType, out stoppedAt);
            return stoppedAt;
        }

        private StringMarkdownEnumerable ParseUntil(string markdown, Tag tag, TagType tagType, out TagInfo stoppedAt)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            var tagInfo = new TagInfo(tag, tagType);
            
            markdownEnumerable.ParseUntil(new[] { tagInfo }, out stoppedAt);

            return markdownEnumerable;
        }
    }
}
