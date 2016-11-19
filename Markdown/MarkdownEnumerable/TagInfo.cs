using System;

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

        public TagInfo GetOfNextType()
        {
            return new TagInfo(Tag, Tag == Tag.Hyperlink 
                ? GetNextTagTypeWithMiddle(TagType)
                : GetNextTagTypeWithoutMiddle(TagType));
        }

        public TagType GetNextTagTypeWithMiddle(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.Opening:
                    return TagType.Middle;
                case TagType.Middle:
                    return TagType.Closing;
                case TagType.None:
                    return TagType.None;
                default:
                    throw new ArgumentException($"Incorrect tag type:{tagType}");
            }
        }

        public TagType GetNextTagTypeWithoutMiddle(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.Opening:
                    return TagType.Closing;
                case TagType.None:
                    return TagType.None;
                default:
                    throw new ArgumentException($"Incorrect tag type{tagType}");
            }
        }

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

        public static bool operator ==(TagInfo left, TagInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TagInfo left, TagInfo right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"Tag: {Tag}, TagType: {TagType}";
        }
    }
}
