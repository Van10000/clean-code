using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownEnumerableExtensions
    {
        public static string ParseUntil(this IMarkdownEnumerable markdown, IEnumerable<TagInfo> tagInfos, out TagInfo stoppedAt)
        {
            var parsed = new StringBuilder();
            var ignoredUselessTags = tagInfos
                .Where(tagInfo => !tagInfo.IsNone())
                .ToList();
            while (!markdown.IsFinished())
            {
                var currentTag = markdown.GetNextTag(ignoredUselessTags);
                if (!currentTag.IsNone())
                {
                    stoppedAt = currentTag;
                    return parsed.ToString();
                }
                parsed.Append(markdown.GetNextChar());
            }
            stoppedAt = TagInfo.None; // if finish
            return parsed.ToString();
        }

        public static void SkipCharacters(this IMarkdownEnumerable markdown, int positionsNumber)
        {
            for (int i = 0; i < positionsNumber; ++i)
                markdown.GetNextChar();
        }
    }
}
