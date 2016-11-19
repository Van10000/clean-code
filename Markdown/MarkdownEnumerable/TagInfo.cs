namespace Markdown.MarkdownEnumerable
{
    public class TagInfo
    {
        public static TagInfo None => new TagInfo(Tag.None, TagType.None);

        public readonly Tag Tag;
        public readonly TagType TagType;

        public TagInfo(Tag tag, TagType tagType)
        {
            TagType = tagType;
            Tag = tag;
        }

        public bool IsNone()
            => Tag == Tag.None && TagType == TagType.None;

        protected bool Equals(TagInfo other)
        {
            return Tag == other.Tag && TagType == other.TagType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TagInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Tag * 397) ^ (int) TagType;
            }
        }
    }
}
