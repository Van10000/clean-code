using FluentAssertions;
using Markdown.MarkdownEnumerable.Tags;
using Markdown.TagsRepresentation;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class ParsedTag_Should
    {
        [TestCase("text", Tag.Italic, ExpectedResult = "<em>text</em>", TestName = "Italic tag")]
        [TestCase("text", Tag.Hyperlink, ExpectedResult = "<a>text</a>", TestName = "Hyperlink tag without link")]
        public string ReturnCorrectRepresentation_WithNoProperties(string value, Tag tag)
        {
            var parsedTag = ParsedTag.Create(TagInfo.Create(tag, TagPosition.None, 0)); // works bad with header tag
            parsedTag.Value = value;
            return parsedTag.GetCurrentRepresentation();
        }

        [Test]
        public void ReturnCorrectRepresentation_ForHeaderTag()
        {
            var parsedTag = new HeaderParsedTag(new HeaderTagInfo(TagPosition.Opening, 4));
            parsedTag.Value = "abc";

            parsedTag.GetCurrentRepresentation().Should().Be("<h4>abc</h4>");
        }

        [Test]
        public void ReturnCorrectHyperlinkRepresentation()
        {
            var parsedTag = ParsedTag.Create(new HyperlinkTagInfo(new TagType(TagPosition.None, 0)));
            parsedTag.Value = "some_text";
            parsedTag.AddProperty("href", "ya.ru");
            var expectedResult = "<a href=\"ya.ru\">some_text</a>";

            var actualResult = parsedTag.GetCurrentRepresentation();

            actualResult.Should().Be(expectedResult);
        }
    }
}
