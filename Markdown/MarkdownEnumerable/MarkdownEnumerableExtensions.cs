using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownEnumerableExtensions
    {
        public static void SkipNextTag(this IMarkdownEnumerable markdown, TagType tagType)
        {
            switch (tagType)
            {
                case TagType.Opening:
                    markdown.SkipNextOpeningTag();
                    break;
                case TagType.Closing:
                    markdown.SkipNextClosingTag();
                    break;
                case TagType.None:
                    break; // ignore
            }
        }

        public static void SkipNextOpeningTag(this IMarkdownEnumerable markdown)
        {
            markdown.SkipTag(markdown.GetNextOpeningTag());

        }

        public static void SkipNextClosingTag(this IMarkdownEnumerable markdown)
        {
            markdown.SkipTag(markdown.GetNextClosingTag());
        }
        
        public static string ParseUntil(this IMarkdownEnumerable markdown, IEnumerable<TagInfo> tagInfos, out TagInfo stoppedAt)
        {
            var parsed = new StringBuilder();
            var ignoredUselessTags = tagInfos
                .Where(tagInfo => tagInfo.Tag != Tag.None && tagInfo.TagType != TagType.None)
                .ToList();
            while (!markdown.IsFinished())
            {
                foreach (var tagInfo in ignoredUselessTags)
                {
                    Tag nextTagOfType;
                    switch (tagInfo.TagType)
                    {
                        case TagType.Opening:
                            nextTagOfType = markdown.GetNextOpeningTag();
                            break;
                        case TagType.Closing:
                            nextTagOfType = markdown.GetNextClosingTag();
                            break;
                        default:
                            throw new ArgumentException($"Unknown tag type:{tagInfo.TagType}");
                    }
                    if (nextTagOfType == tagInfo.Tag)
                    {
                        markdown.SkipNextTag(tagInfo.TagType);
                        stoppedAt = tagInfo;
                        return parsed.ToString();
                    }
                }
                parsed.Append(markdown.GetNextChar());
            }
            stoppedAt = new TagInfo(Tag.None, TagType.None);
            return parsed.ToString();
        }

        public static void SkipCharacters(this IMarkdownEnumerable markdown, int positionsNumber)
        {
            for (int i = 0; i < positionsNumber; ++i)
                markdown.GetNextChar();
        }

        private static void SkipTag(this IMarkdownEnumerable markdown, Tag tag)
        {
            var tagRepresentation = MarkdownParsingPrimitives.GetTagRepresentation(tag);
            markdown.SkipCharacters(tagRepresentation.Length);
        }
    }
}
