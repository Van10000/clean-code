using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class ParsingPrimitives_Should
    {
        [TestCase(Tag.Strong, " __c", 1, ExpectedResult = true)]
        [TestCase(Tag.Strong, " __c", 2, ExpectedResult = false)]
        [TestCase(Tag.Strong, " _c", 1, ExpectedResult = false)]
        [TestCase(Tag.Strong, " __ ", 1, ExpectedResult = false)]
        public bool Detect_peningTag(Tag tag, string str, int pos)
        {
            return MarkdownParsingPrimitives.IsOpeningTag(tag, str, pos);
        }
    }
}
