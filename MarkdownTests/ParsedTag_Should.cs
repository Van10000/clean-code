using FluentAssertions;
using Markdown.MarkdownEnumerable.Tags;
using Markdown.TagsRepresentation;
using NUnit.Framework;

namespace MarkdownTests
{
    class ParsedTag_Should
    {
        [TestCase("text", Tag.Italic, ExpectedResult = "<em>text</em>", TestName = "Italic tag")]
        [TestCase("text", Tag.Hyperlink, ExpectedResult = "<a>text</a>", TestName = "Hyperlink tag without link")]
        public string ReturnCorrectRepresentation_WithNoProperties(string value, Tag tag)
        {
            var parsedTag = ParsedTag.Create(tag);
            parsedTag.Value = value;
            return parsedTag.GetCurrentRepresentation();
        }

        [Test]
        public void ReturnCorrectHyperlinkRepresentation()
        {
            var parsedTag = ParsedTag.Create(Tag.Hyperlink);
            parsedTag.Value = "some_text";
            parsedTag.AddProperty("href", "ya.ru");
            var expectedResult = "<a href=\"ya.ru\">some_text</a>";

            var actualResult = parsedTag.GetCurrentRepresentation();

            actualResult.Should().Be(expectedResult);
        }
    }
}
