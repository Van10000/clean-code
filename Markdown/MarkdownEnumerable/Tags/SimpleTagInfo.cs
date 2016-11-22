using System;

namespace Markdown.MarkdownEnumerable.Tags
{
    class SimpleTagInfo : TagInfo
    {
        public override int MaximalPossiblePartsCount => 1;

        public SimpleTagInfo(Tag tag, TagPosition tagPosition) : base(tag, tagPosition, 0)
        {
            if (tag == Tag.Hyperlink)
                throw new ArgumentException($"Tag should be simple. {tag} is not a simple tag.");
        }
    }
}
