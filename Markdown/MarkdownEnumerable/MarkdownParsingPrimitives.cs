using System;
using Markdown.TagsRepresentation;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownParsingPrimitives
    {
        private static readonly MarkdownTagsRepresentation MarkdownRepresentation = new MarkdownTagsRepresentation();

        public static bool IsOpeningTag(Tag tag, string markdown, int position)
        {
            var tagRepresentation = MarkdownRepresentation.GetOpeningTag(tag);
            var positionAfterTagEnd = position + tagRepresentation.Length;
            if (positionAfterTagEnd > markdown.Length)
                return false;
            if (markdown.Substring(position, tagRepresentation.Length) != tagRepresentation)
                return false;
            if (positionAfterTagEnd == markdown.Length + 1)
                return true;
            return markdown[positionAfterTagEnd] != ' ';
        }
    }
}
