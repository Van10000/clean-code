﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownParsingUtils
    {
        private const string Digits = "0123456789";
        private const char Underscore = '_';
        private const string SpaceSymbols = " \n\r\u0009\u00a0\u2000\u2001";

        public static bool IsCorrectTag(TagInfo tagInfo, string markdown, int position)
        {
            if (tagInfo.Tag == Tag.None || tagInfo.TagPosition == TagPosition.None)
                return false;

            var tagRepresentation = GetTagRepresentation(tagInfo);
            var positionAfterTagEnd = position + tagRepresentation.Length;
            var positionBeforeTagStart = position - 1;

            if (positionAfterTagEnd > markdown.Length)
                return false;
            if (markdown.Substring(position, tagRepresentation.Length) != tagRepresentation)
                return false;

            switch (tagInfo.Tag)
            {
                case Tag.Italic:
                case Tag.Strong:
                    return IsCorrectUnderscoreTag(tagInfo, markdown, positionBeforeTagStart, positionAfterTagEnd);
                case Tag.Hyperlink:
                    if (tagInfo.TagPosition == TagPosition.Opening && tagInfo.TagPart == 0)
                        return IsHyperlinkStart(markdown, positionAfterTagEnd);
                    return true; // It's not always true, but for algorithm it doesn't matter. We can check it here though, if we want.
                default:
                    throw new ArgumentException($"Unknown tag:{tagInfo.Tag}");
            }
        }

        public static string GetTagRepresentation(TagInfo tag)
        {
            switch (tag.Tag)
            {
                case Tag.Hyperlink:
                    return GetHyperlinkRepresentation(tag.TagType);
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

        public static string ToCorrectLink(string link)
        {
            var skippedSpaces = link
                .SkipWhile(char.IsWhiteSpace)
                .TakeWhile(ch => !char.IsWhiteSpace(ch))
                .Aggregate(new StringBuilder(), (builder, ch) => builder.Append(ch))
                .ToString();
            if (skippedSpaces.Count(ch => !char.IsWhiteSpace(ch)) != link.Count(ch => !char.IsWhiteSpace(ch)))
                return null;
            return skippedSpaces;
        }

        public static bool IsCorrectLink(string link)
        {
            return ToCorrectLink(link) != null;
        }

        public static string CombineLinks(string baseLink, string relativePath)
        {
            var builder = new StringBuilder();
            builder.Append(baseLink.LastOrDefault() == '/' ? baseLink.Substring(0, baseLink.Length - 1) : baseLink);
            builder.Append('/');
            builder.Append(relativePath.FirstOrDefault() == '/' ? relativePath.Substring(1) : relativePath);
            return builder.ToString();
        }

        /// <summary>
        /// Checks that hyperlink will be finished at some point.
        /// </summary>
        /// <param name="position">Position after '[' symbol</param>
        /// <returns></returns>
        private static bool IsHyperlinkStart(string markdown, int position)
        {
            var closingTag = GetHyperlinkRepresentation(new TagType(TagPosition.Closing, HyperlinkTagInfo.VALUE_PART));
            for (var i = position; i <= markdown.Length - closingTag.Length; ++i)
                if (markdown.Substring(i, closingTag.Length) == closingTag)
                {
                    var openingSecondPartTag = GetHyperlinkRepresentation(new TagType(TagPosition.Opening, HyperlinkTagInfo.LINK_PART));
                    if (i + openingSecondPartTag.Length >= markdown.Length || 
                        markdown.Substring(i + 1, openingSecondPartTag.Length) != openingSecondPartTag)
                        return false;
                    return IsHyperlinkSecondPart(markdown, i + closingTag.Length + openingSecondPartTag.Length);
                }
            return false;
        }

        /// <summary>
        /// Checks that hyperlink will be finished at some point.
        /// </summary>
        /// <param name="position">Position after "](" symbols</param>
        /// <returns></returns>
        private static bool IsHyperlinkSecondPart(string markdown, int position)
        {
            var builder = new StringBuilder();
            var endRepresentation = GetHyperlinkRepresentation(new TagType(TagPosition.Closing, HyperlinkTagInfo.LINK_PART));
            for (var i = position; i <= markdown.Length - endRepresentation.Length; ++i)
            {
                if (markdown.Substring(i, endRepresentation.Length) == endRepresentation)
                    return IsCorrectLink(builder.ToString());
                else
                    builder.Append(markdown[i]);
            }
            return false;
        }



        private static string GetHyperlinkRepresentation(TagType tagType)
        {
            switch (tagType.TagPosition)
            {
                case TagPosition.Opening:
                    return tagType.TagPart == HyperlinkTagInfo.VALUE_PART ? "[" : "(";
                case TagPosition.Closing:
                    return tagType.TagPart == HyperlinkTagInfo.VALUE_PART ? "]" : ")";
                default:
                    throw new ArgumentException($"Unknown tag type:{tagType.TagPosition}");
            }
        }

        private static bool IsCorrectUnderscoreTag(TagInfo tagInfo, string markdown, int positionBefore, int positionAfter)
        {
            var positionsBeforeAndAfter = new[] { positionBefore, positionAfter };
            if (IsAnySymbolAtAnyPosition(markdown, positionsBeforeAndAfter, Underscore + Digits))
                return false;

            return AreGoodPositionsForTag(tagInfo.TagPosition, markdown, positionBefore, positionAfter);
        }

        private static bool AreGoodPositionsForTag(TagPosition tagPosition, string markdown, int positionBefore, int positionAfter)
        {
            if (tagPosition == TagPosition.None)
                return false;

            if (tagPosition == TagPosition.Opening)
            {
                var correctAtPositionBefore = IsPositionOutOfRange(markdown, positionBefore) || IsAnySymbolAtPosition(markdown, positionBefore, SpaceSymbols);
                var correctAtPositionAfter = IsPositionOutOfRange(markdown, positionAfter) || IsNotAnySymbolAtPosition(markdown, positionAfter, SpaceSymbols);
                return correctAtPositionBefore && correctAtPositionAfter;
            }
            if (tagPosition == TagPosition.Closing)
                return AreGoodPositionsForTag(TagPosition.Opening, markdown, positionAfter, positionBefore);
            throw new ArgumentException($"Unknown tag type:{tagPosition}");
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

        private static bool IsNotAnySymbolAtPosition(string markdown, int position, IEnumerable<char> symbols) => symbols.All(symbol => IsNotSymbolAtPosition(markdown, position, symbol));

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
