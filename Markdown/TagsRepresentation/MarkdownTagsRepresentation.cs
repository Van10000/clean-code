using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal class MarkdownTagsRepresentation : ITagsRepresentation
    {
        public string GetOpeningTag(Tag tag)
        {
            switch (tag)
            {
                case Tag.Strong:
                    return "__";
                case Tag.Italic:
                    return "_";
                case Tag.None:
                    return "";
                default:
                    throw new ArgumentException($"Unknown tag:{tag}");
            }
        }

        public string GetClosingTag(Tag tag)
        {
            return GetOpeningTag(tag);
        }
    }
}
