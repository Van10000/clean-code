namespace Markdown.MarkdownEnumerable.Tags
{
    public class TagType
    {
        public static TagType FirstOpeningTag => new TagType(TagPosition.Opening, 0); 

        public readonly TagPosition TagPosition;
        public readonly int TagPart;

        public TagType(TagPosition position, int tagPart)
        {
            TagPosition = position;
            TagPart = tagPart;
        }

        public bool Equals(TagType other)
        {
            return TagPosition == other.TagPosition && TagPart == other.TagPart;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TagType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) TagPosition * 397) ^ TagPart;
            }
        }

        public static bool operator ==(TagType left, TagType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TagType left, TagType right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"TagPosition: {TagPosition}, TagPart: {TagPart}";
        }
    }
}
