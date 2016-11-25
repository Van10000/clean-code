using System;
using JetBrains.Annotations;

namespace Markdown.MarkdownEnumerable.Tags
{
    public abstract class TagInfo
    {
        public static TagInfo None => new SimpleTagInfo(Tag.None, TagPosition.None);

        public readonly Tag Tag;
        public readonly TagType TagType;

        public TagPosition TagPosition => TagType.TagPosition;
        public int TagPart => TagType.TagPart;
        public abstract int MaximalPossiblePartsCount { get; }

        /// <summary>
        /// If representation is ambiguous in subclass - method returns null.
        /// </summary>
        [CanBeNull]
        public abstract string GetRepresentation();

        public virtual bool ShouldClose() => TagPosition == TagPosition.Closing || TagPosition == TagPosition.None;

        public virtual bool IsSingle() => false;

        public bool IsOpening() => TagPosition == TagPosition.Opening;
        public bool IsClosing() => TagPosition == TagPosition.Closing;
        public bool IsNone() => this == None;

        public virtual bool IsNotLastPartOfTag() => TagPart < MaximalPossiblePartsCount - 1;
        

        protected TagInfo(Tag tag, TagPosition tagPosition, int tagPart)
        {
            Tag = tag;
            TagType = new TagType(tagPosition, tagPart);
        }

        protected TagInfo(Tag tag, TagType tagType)
        {
            Tag = tag;
            TagType = tagType;
        }

        public static TagInfo Create(Tag tag, TagPosition tagPosition, int tagPart)
        {
            return Create(tag, new TagType(tagPosition, tagPart));
        }

        public static TagInfo Create(Tag tag, TagType tagType)
        {
            switch (tag)
            {
                case Tag.Hyperlink:
                    return new HyperlinkTagInfo(tagType);
                case Tag.Paragraph:
                    return new ParagraphTagInfo(tagType.TagPosition);
                case Tag.NewLine:
                    return new NewLineTagInfo();
                case Tag.Header:
                    return new HeaderTagInfo(tagType.TagPosition);
                case Tag.Italic:
                case Tag.Strong:
                case Tag.None:
                    return new SimpleTagInfo(tag, tagType.TagPosition);
                default:
                    throw new ArgumentException($"Unknown tag:{tag}");
            }
        }

        public virtual TagInfo GetOfNextType()
        {
            if (IsNone())
                return None;
            if (TagPosition == TagPosition.Opening)
                return Create(Tag, TagPosition.Closing, TagPart);
            else if (TagPart < MaximalPossiblePartsCount - 1)
                return Create(Tag, TagPosition.Opening, TagPart + 1);
            throw new InvalidOperationException("There is no next tag after closing tag.");
        }

        public virtual bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag = null)
        {
            positionAfterEnd = -1;
            if (Tag == Tag.None || TagPosition == TagPosition.None)
                return false;

            var tagRepresentation = GetRepresentation();
            positionAfterEnd = position + tagRepresentation.Length;

            if (positionAfterEnd > markdown.Length)
                return false;
            return markdown.Substring(position, tagRepresentation.Length) == tagRepresentation;
        }

        protected bool Equals(TagInfo other)
        {
            return Tag == other.Tag && Equals(TagType, other.TagType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as TagInfo;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Tag * 397) ^ (TagType != null ? TagType.GetHashCode() : 0);
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
            return $"Tag: {Tag}, TagPosition: {TagPosition}, TagPart: {TagPart}";
        }
    }
}
