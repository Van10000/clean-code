using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownParsingPrimitives
    {
        private const string Digits = "0123456789";
        private const char Underscore = '_';
        private const string SpaceSymbols = " \n\r\u0009";

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
            if (tag == Tag.None)
                throw new ArgumentException("Tag should not be Tag.None");
            if (tagType == TagType.None)
                return false;

            var tagRepresentation = GetTagRepresentation(tag);
            var positionAfterTagEnd = position + tagRepresentation.Length;
            var positionBeforeTagStart = position - 1;

            if (positionAfterTagEnd > markdown.Length)
                return false;
            var positionsBeforeAndAfter = new[] {positionBeforeTagStart, positionAfterTagEnd};
            if (IsAnySymbolAtAnyPosition(markdown, positionsBeforeAndAfter, Underscore + Digits))
                return false;
            if (markdown.Substring(position, tagRepresentation.Length) != tagRepresentation)
                return false;

            return AreGoodPositionsForTag(tagType, markdown, positionBeforeTagStart, positionAfterTagEnd);
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

        private static bool AreGoodPositionsForTag(TagType tagType, string markdown, int positionBefore, int positionAfter)
        {
            if (tagType == TagType.None)
                return false;
            if (tagType == TagType.Opening)
            {
                var correctAtPositionBefore =
                    IsPositionOutOfRange(markdown, positionBefore) || 
                    IsAnySymbolAtPosition(markdown, positionBefore, SpaceSymbols);
                var correctAtPositionAfter =
                    IsPositionOutOfRange(markdown, positionAfter) ||
                    IsNotAnySymbolAtPosition(markdown, positionAfter, SpaceSymbols);
                return correctAtPositionBefore && correctAtPositionAfter;
            }
            else if (tagType == TagType.Closing)
                return AreGoodPositionsForTag(TagType.Opening, markdown, positionAfter, positionBefore);
            else
                throw new ArgumentException($"Unknown tag type:{tagType}");
        }

        private static bool IsAnySymbolAtAnyPosition(string markdown, IEnumerable<int> positions, IEnumerable<char> symbols)
        {
            return positions.Any(pos => IsAnySymbolAtPosition(markdown, pos, symbols));
        }

        private static bool IsAnySymbolAtPosition(string markdown, int position, IEnumerable<char> symbols)
        {
            return symbols.Any(symbol => IsSymbolAtPosition(markdown, position, symbol));
        }

        private static bool IsSymbolAtPosition(string markdown, int position, char symbol)
        {
            if (IsPositionOutOfRange(markdown, position))
                return false;
            return markdown[position] == symbol;
        }

        private static bool IsNotAnySymbolAtPosition(string markdown, int position, IEnumerable<char> symbols)
            => symbols.All(symbol => IsNotSymbolAtPosition(markdown, position, symbol));

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
    }
}
