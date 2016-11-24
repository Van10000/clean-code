using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    class ParsedNewLineTag : ParsedTag
    {
        public ParsedNewLineTag() : base(Tag.NewLine)
        {
        }

        public override string GetCurrentRepresentation()
        {
            return "<br>";
        }

        public override void AddValueOrProperty(string valueOrProperty, TagType tagType, string baseUrl)
        {
            if (valueOrProperty == "")
                return;
            throw new InvalidOperationException();
        }
    }
}
