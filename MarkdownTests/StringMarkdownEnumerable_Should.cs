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

        [TestCase(" __c__ ", 1, ExpectedResult = Tag.Strong)]
        [TestCase("_", 0, ExpectedResult = Tag.Italic)]
        [TestCase("asdb wer", 2, ExpectedResult = Tag.None)]
        public Tag AtSpecifiedPosition_ReturnNextOpeningTag(string markdown, int position)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            markdownEnumerable.SkipCharacters(position);

            return markdownEnumerable.GetNextOpeningTag();
        }

        [TestCase("e__", 1, ExpectedResult = Tag.Strong)]
        [TestCase(" f_ d ", 2, ExpectedResult = Tag.Italic)]
        [TestCase("", 0, ExpectedResult = Tag.None)]
        [TestCase("sadfasrw", 5, ExpectedResult = Tag.None)]
        public Tag AtSpecifiedPosition_ReturnNextClosingTag(string markdown, int position)
        {
            var markdownEnumerable = new StringMarkdownEnumerable(markdown);
            markdownEnumerable.SkipCharacters(position);

            return markdownEnumerable.GetNextClosingTag();
        }
    }
}
