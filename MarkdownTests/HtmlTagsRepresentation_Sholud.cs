using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string Return_RightOpeningTag(Tag tag)
        {
            return representation.GetOpeningTag(tag);
        }

        [TestCase(Tag.None, ExpectedResult = "")]
        [TestCase(Tag.Italic, ExpectedResult = "</em>")]
        [TestCase(Tag.Strong, ExpectedResult = "</strong>")]
        public string Return_RightClosingTag(Tag tag)
        {
            return representation.GetClosingTag(tag);
        }
    }
}
