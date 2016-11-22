﻿using System;

namespace Markdown.MarkdownEnumerable.Tags
{
    public abstract class TagInfo
    {
        public static TagInfo None => Create(Tag.None, TagPosition.None, 0);

        public readonly Tag Tag;
        public readonly TagType TagType;

        public TagPosition TagPosition => TagType.TagPosition;
        public int TagPart => TagType.TagPart;
        public abstract int MaximalPossiblePartsCount { get; }

        public bool IsOpening() => TagPosition == TagPosition.Opening;
        public bool IsClosing() => TagPosition == TagPosition.Closing;
        public bool IsNone() => this == None;

        public bool IsNotLastPartOfTag() => TagPart < MaximalPossiblePartsCount - 1;

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
            if (tag == Tag.Hyperlink)
                return new HyperlinkTagInfo(tagType);
            return new SimpleTagInfo(tag, tagType.TagPosition);
        }

        public TagInfo GetOfNextType()
        {
            if (IsNone())
                return None;
            if (TagPosition == TagPosition.Opening)
                return Create(Tag, TagPosition.Closing, TagPart);
            else if (TagPart < MaximalPossiblePartsCount - 1)
                return Create(Tag, TagPosition.Opening, TagPart + 1);
            throw new InvalidOperationException("There is no next tag after closing tag.");
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