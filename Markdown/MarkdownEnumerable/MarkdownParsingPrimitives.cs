using System;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownParsingPrimitives
    {
        public static bool IsOpeningTag(Tag tag, string markdown, int position)
        {
            return IsCorrectTag(tag, TagType.Opening, markdown, position);
        }

        public static bool IsClosingTag(Tag tag, string markdown, int position)
        {
            return IsCorrectTag(tag, TagType.Closing, markdown, position);
        }

        public static bool IsCorrectTag(Tag tag, TagType tagType, string markdown, int position)
        {
            if (!IsTag(tag, markdown, position))
                return false;
            if (tagType == TagType.None)
                return false;
            var tagRepresentation = GetTagRepresentation(tag);
            var positionAfterTagEnd = position + tagRepresentation.Length;
            var positionBeforeTagStart = position - 1;

            if (tagType == TagType.Opening)
                return IsSpaceAtPositionOrOutOfRange(markdown, positionBeforeTagStart) &&
                       IsNotSpaceAtPosition(markdown, positionAfterTagEnd);
            else
                return IsNotSpaceAtPosition(markdown, positionBeforeTagStart) &&
                       IsSpaceAtPositionOrOutOfRange(markdown, positionAfterTagEnd);
        }

        public static string GetTagRepresentation(Tag tag)
        {
            switch (tag)
            {
                case Tag.Strong:
                    return "__";
                case Tag.Italic:
                    return "_";
                case Tag.None:
                    return "";
                default:
                    throw new ArgumentException($"Unknown tag:{tag}");
            }
        }

        private static bool IsDigitAtPosition(string markdown, int position)
        {
            if (IsPositionOutOfRange(markdown, position))
                return false;
            return '0' <= markdown[position] && markdown[position] <= '9';
        }

        private static bool IsUnderscoreAtPosition(string markdown, int position)
        {
            if (IsPositionOutOfRange(markdown, position))
                return false;
            return markdown[position] == '_';
        }

        private static bool IsSpaceAtPositionOrOutOfRange(string markdown, int position)
        {
            if (IsPositionOutOfRange(markdown, position))
                return true;
            return markdown[position] == ' ';
        }

        private static bool IsNotSpaceAtPosition(string markdown, int position)
        {
            return IsNotSymbolAtPosition(markdown, position, ' ');
        }

        private static bool IsNotSymbolAtPosition(string markdown, int position, char symbol)
        {
            if (IsPositionOutOfRange(markdown, position))
                return true;
            return markdown[position] != symbol;
        }

        private static bool IsPositionOutOfRange(string markdown, int position)
        {
            return position >= markdown.Length || position < 0;
        }

        private static bool IsTag(Tag tag, string markdown, int position)
        {
            if (tag == Tag.None)
                throw new ArgumentException("Tag should not be Tag.None");
            if (tag == Tag.Italic && IsOpeningTag(Tag.Strong, markdown, position))
                return false;
            var tagRepresentation = GetTagRepresentation(tag);
            var positionAfterTagEnd = position + tagRepresentation.Length;
            if (positionAfterTagEnd > markdown.Length)
                return false;
            if (IsDigitAtPosition(markdown, positionAfterTagEnd) || IsDigitAtPosition(markdown, position - 1) ||
                IsUnderscoreAtPosition(markdown, positionAfterTagEnd) || IsUnderscoreAtPosition(markdown, position - 1))
                return false;
            return markdown.Substring(position, tagRepresentation.Length) == tagRepresentation;
        }
    }
}
