using System;

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
                var beforeStart = MarkdownParsingUtils.FindPreviousNotFitting(markdown, position - 1, 
                    MarkdownParsingUtils.IsWhiteSpaceAndNotNextLineSymbol);
                if (beforeStart != -1 && !MarkdownParsingUtils.IsNextLineSymbol(markdown[beforeStart]))
                    return false;
                var positionAfterWhiteSpaces = MarkdownParsingUtils.FindNextNotFitting(markdown, position, 
                    MarkdownParsingUtils.IsWhiteSpaceAndNotNextLineSymbol);
                if (MarkdownParsingUtils.IsEndOfLine(markdown, positionAfterWhiteSpaces))
                    return false;
                var headerSymbolsEndPosition = MarkdownParsingUtils.FindNextNotFitting(markdown, positionAfterWhiteSpaces, ch => ch == HeaderSymbol);
                var headerSymbolsCount = headerSymbolsEndPosition - positionAfterWhiteSpaces;
                if (headerSymbolsCount < 1 || headerSymbolsCount > 6)
                    return false;
                if (headerSymbolsEndPosition != markdown.Length && !char.IsWhiteSpace(markdown[headerSymbolsEndPosition]))
                    return false;
                positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, headerSymbolsEndPosition, 
                    MarkdownParsingUtils.IsWhiteSpaceAndNotNextLineSymbol);
                HeaderLevel = headerSymbolsCount;
                return true;
            }
            else
            {
                var positionAfterWhiteSpaces = MarkdownParsingUtils.FindNextNotFitting(markdown, position,
                    MarkdownParsingUtils.IsWhiteSpaceAndNotNextLineSymbol);
                if (MarkdownParsingUtils.IsEndOfLine(markdown, positionAfterWhiteSpaces))
                {
                    positionAfterEnd = positionAfterWhiteSpaces;
                    return true;
                }
                if (markdown[positionAfterWhiteSpaces] != HeaderSymbol)
                    return false;
                var positionAfterHeaderSymbols = MarkdownParsingUtils.FindNextNotFitting(markdown,
                    positionAfterWhiteSpaces,
                    ch => ch == HeaderSymbol);
                positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, positionAfterHeaderSymbols,
                    MarkdownParsingUtils.IsWhiteSpaceAndNotNextLineSymbol);
                return MarkdownParsingUtils.IsEndOfLine(markdown, positionAfterEnd);
            }
        }
    }
}
