using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class HtmlTagsRepresentation_Sholud
    {
        private readonly HtmlTagsRepresentation representation = new HtmlTagsRepresentation();

        [TestCase(Tag.None, ExpectedResult = "")]
        [TestCase(Tag.Italic, ExpectedResult = "<em>")]
        [TestCase(Tag.Strong, ExpectedResult = "<strong>")]
        [TestCase(Tag.Hyperlink, ExpectedResult = "<a href=\"")]
        public string ReturnRightOpeningTag(Tag tag)
        {
            return representation.GetRepresentation(new TagInfo(tag, TagType.Opening));
        }

        [TestCase(Tag.Hyperlink, ExpectedResult = "\">")]
        public string ReturnRightMiddleTag(Tag tag)
        {
            return representation.GetRepresentation(new TagInfo(tag, TagType.Middle));
        }

        [TestCase(Tag.None, ExpectedResult = "")]
        [TestCase(Tag.Italic, ExpectedResult = "</em>")]
        [TestCase(Tag.Strong, ExpectedResult = "</strong>")]
        [TestCase(Tag.Hyperlink, ExpectedResult = "</a>")]
        public string ReturnRightClosingTag(Tag tag)
        {
            return representation.GetRepresentation(new TagInfo(tag, TagType.Closing));
        }
    }
}
