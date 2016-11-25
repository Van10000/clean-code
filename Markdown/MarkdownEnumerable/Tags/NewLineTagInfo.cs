using System.Linq;

namespace Markdown.MarkdownEnumerable.Tags
{
    internal class NewLineTagInfo : TagInfo
    {
        public const int MinimalSpaceSymbolsNumberBeforeNewLine = 2;

        public NewLineTagInfo() : base(Tag.NewLine, TagPosition.None, 0)
        {
        }

        public override bool ShouldClose() => false;

        public override int MaximalPossiblePartsCount => 1;

        public override string GetRepresentation() => null;

        public override bool IsSingle() => true;

        public override bool IsNotLastPartOfTag() => false;

        public override TagInfo GetOfNextType() => None;

        public override bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag)
        {
            positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, position, char.IsWhiteSpace);
            var symbolsBetween = markdown.Substring(position, positionAfterEnd - position);
            var exactlyOneNewLine = symbolsBetween.Count(MarkdownParsingUtils.IsNextLineSymbol) == 1;
            var nextLineSymbolPosition = symbolsBetween.IndexOfAny(MarkdownParsingUtils.NextLineSymbols.ToCharArray());
            var enoughSpaces = (nextLineSymbolPosition >= MinimalSpaceSymbolsNumberBeforeNewLine) || (previousTag?.Tag == Tag.Header);
            positionAfterEnd = position + nextLineSymbolPosition + 1;

            return exactlyOneNewLine && enoughSpaces;
        }
    }
}
