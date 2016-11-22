namespace Markdown.MarkdownEnumerable.Tags
{
    public class HyperlinkTagInfo : TagInfo
    {
        public const int VALUE_PART = 0;
        public const int LINK_PART = 1;

        public override int MaximalPossiblePartsCount => 2;

        public HyperlinkTagInfo(TagPosition tagPosition, int tagPart) : base(Tag.Hyperlink, tagPosition, tagPart)
        {
        }

        public HyperlinkTagInfo(TagType tagType) : base(Tag.Hyperlink, tagType)
        {
        }
    }
}
