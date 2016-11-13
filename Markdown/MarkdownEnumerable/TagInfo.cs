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

    // CR: One class = one file
    internal enum TagType
    {
        None, Opening, Closing
    }
}
