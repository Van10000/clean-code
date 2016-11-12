using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MarkdownEnumerable
{
    internal class TagInfo
    {
        public readonly Tag Tag;
        public readonly TagType TagType;

        public TagInfo(Tag tag, TagType tagType)
        {
            TagType = tagType;
            Tag = tag;
        }
    }

    internal enum TagType
    {
        None, Opening, Closing
    }
}
