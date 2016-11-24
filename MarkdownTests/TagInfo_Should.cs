using Markdown.MarkdownEnumerable.Tags;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class TagInfo_Should
    {
        [TestCase(Tag.Italic, TagPosition.Opening, 0, ExpectedResult = "_")]
        [TestCase(Tag.Italic, TagPosition.Closing, 0, ExpectedResult = "_")]
        [TestCase(Tag.Strong, TagPosition.Opening, 0, ExpectedResult = "__")]
        [TestCase(Tag.Strong, TagPosition.Opening, 0, ExpectedResult = "__")]
        [TestCase(Tag.None, TagPosition.None, 0, ExpectedResult = "")]
        [TestCase(Tag.Hyperlink, TagPosition.Opening, HyperlinkTagInfo.VALUE_PART, ExpectedResult = "[")]
        [TestCase(Tag.Hyperlink, TagPosition.Closing, HyperlinkTagInfo.VALUE_PART, ExpectedResult = "]")]
        [TestCase(Tag.Hyperlink, TagPosition.Opening, HyperlinkTagInfo.LINK_PART, ExpectedResult = "(")]
        [TestCase(Tag.Hyperlink, TagPosition.Closing, HyperlinkTagInfo.LINK_PART, ExpectedResult = ")")]
        public string RepresentationTest(Tag tag, TagPosition tagPosition, int tagPart)
        {
            return TagInfo.Create(tag, tagPosition, tagPart).GetRepresentation();
        }

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
        [TestCase(Tag.Hyperlink, HyperlinkTagInfo.VALUE_PART, "[a](b)", 0, ExpectedResult = true, TestName = "Simple hyperlink start")]
        [TestCase(Tag.Hyperlink, HyperlinkTagInfo.LINK_PART, "(a)", 0, ExpectedResult = true, TestName = "Do not check correctness for not first tagType")]
        [TestCase(Tag.Hyperlink, HyperlinkTagInfo.LINK_PART, "abc", 0, ExpectedResult = false, TestName = "Still check correct representation")]
        public bool DetectOpeningTag(Tag tag, int tagPart, string str, int pos)
        {
            var tagInfo = TagInfo.Create(tag, new TagType(TagPosition.Opening, tagPart));
            int positionAfter;
            return tagInfo.Fits(str, pos, out positionAfter);
        }

        [TestCase(Tag.Italic, 0, "c_", 1, ExpectedResult = true, TestName = "Simple italic closing tag")]
        [TestCase(Tag.Strong, 0, "__", 0, ExpectedResult = true, TestName = "Lonely strong tag")]
        [TestCase(Tag.Italic, 0, "a _b", 2, ExpectedResult = false, TestName = "Gap between character and tag")]
        [TestCase(Tag.Italic, 0, "a_ b", 1, ExpectedResult = true, TestName = "Gap in another side")]
        [TestCase(Tag.Hyperlink, HyperlinkTagInfo.VALUE_PART, "[abc](de)", 4, ExpectedResult = true, TestName = "Simple closing bracket")]
        [TestCase(Tag.Hyperlink, HyperlinkTagInfo.LINK_PART, "[abc](de)", 8, ExpectedResult = true, TestName = "Simple closing parenthesis")]
        public bool DetectClosingTag(Tag tag, int tagPart, string str, int pos)
        {
            var tagInfo = TagInfo.Create(tag, new TagType(TagPosition.Closing, tagPart));
            int positionAfter;
            return tagInfo.Fits(str, pos, out positionAfter);
        }
    }
}
