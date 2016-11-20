using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownEnumerableExtensions
    {
        public static string ParseUntil(this IMarkdownEnumerable markdown, IEnumerable<TagInfo> tagInfos, out TagInfo stoppedAt)
        {
            var parsed = new StringBuilder();
            var ignoredUselessTags = tagInfos.Where(tagInfo => tagInfo.Tag != Tag.None && tagInfo.TagType != TagType.None).ToList();
            while (!markdown.IsFinished())
            {
                var currentTag = markdown.GetNextTag(ignoredUselessTags);
                if (!currentTag.IsNone())
                {
                    markdown.SkipTag(currentTag);
                    stoppedAt = currentTag;
                    return parsed.ToString();
                }
                parsed.Append(markdown.GetNextChar());
            }
            stoppedAt = new TagInfo(Tag.None, TagType.None); // if finish
            return parsed.ToString();
        }

        public static void SkipCharacters(this IMarkdownEnumerable markdown, int positionsNumber)
        {
            for (int i = 0; i < positionsNumber; ++i)
                markdown.GetNextChar();
        }

        private static void SkipTag(this IMarkdownEnumerable markdown, TagInfo tagInfo)
        {
            var tagRepresentation = MarkdownParsingUtils.GetTagRepresentation(tagInfo);
            markdown.SkipCharacters(tagRepresentation.Length);
        }
    }
}
