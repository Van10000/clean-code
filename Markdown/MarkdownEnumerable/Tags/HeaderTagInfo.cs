using System;
using System.CodeDom;
using System.Linq;

namespace Markdown.MarkdownEnumerable.Tags
{
    internal class HeaderTagInfo : TagInfo
    {
        public const char HeaderSymbol = '#';
        public int HeaderLevel;

        public HeaderTagInfo(TagPosition tagPosition, int level = -1) : base(Tag.Header, tagPosition, 0)
        {
            HeaderLevel = level;
        }

        public override int MaximalPossiblePartsCount => 1;

        public override string GetRepresentation() => null;

        public override TagInfo GetOfNextType()
        {
            if (TagPosition == TagPosition.Opening)
                return new HeaderTagInfo(TagPosition.Closing, HeaderLevel);
            throw new InvalidOperationException($"There is no next type for {TagType} in header tag.");
        }

        public override bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag = null)
        {
            positionAfterEnd = -1;
            if (TagPosition == TagPosition.Opening)
            {
                int afterWhiteSpaces;
                if (IsEndOfLine(markdown, position, out afterWhiteSpaces))
                    return false;
                var headerSymbolsEndPosition = MarkdownParsingUtils.FindNextNotFitting(markdown, afterWhiteSpaces, HeaderSymbol.Equals);
                var headerSymbolsCount = headerSymbolsEndPosition - afterWhiteSpaces;
                if (!IsInRangeNotStrict(headerSymbolsCount, 1, 6))
                    return false;
                positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, headerSymbolsEndPosition, char.IsWhiteSpace);
                if (positionAfterEnd == headerSymbolsEndPosition)
                    return false; // need at least one white space
                HeaderLevel = headerSymbolsCount;
                return true;
            }
            else
            {
                if (IsEndOfLine(markdown, position, out positionAfterEnd))
                    return true;
                if (markdown[positionAfterEnd] != HeaderSymbol)
                    return false;
                var positionAfterHeaderSymbols = MarkdownParsingUtils.FindNextNotFitting(markdown, 
                    positionAfterEnd, HeaderSymbol.Equals);
                return IsEndOfLine(markdown, positionAfterHeaderSymbols, out positionAfterEnd);
            }
        }

        private static bool IsEndOfLine(string markdown, int position, out int positionAfterEnd)
        {
            positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, position, char.IsWhiteSpace);
            var endTagSubstr = markdown.Substring(position, positionAfterEnd - position);
            return endTagSubstr.Any(MarkdownParsingUtils.IsNextLineSymbol) || positionAfterEnd == markdown.Length;
        }

        private static bool IsInRangeNotStrict(int value, int leftBound, int rightBound)
            => leftBound <= value && value <= rightBound;
    }
}
