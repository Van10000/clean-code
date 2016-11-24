using System.Linq;

namespace Markdown.MarkdownEnumerable.Tags
{
    class NewLineTagInfo : TagInfo
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

        public override bool Fits(string markdown, int position, out int positionAfterEnd)
        {
            positionAfterEnd = MarkdownParsingUtils.FindNextNotWhiteSpace(markdown, position);
            var symbolsBetween = markdown.Substring(position, positionAfterEnd - position);
            return symbolsBetween.Count(MarkdownParsingUtils.NextLineSymbols.Contains) == 1 &&
                   symbolsBetween.IndexOfAny(MarkdownParsingUtils.NextLineSymbols.ToCharArray()) >= MinimalSpaceSymbolsNumberBeforeNewLine;
        }
    }
}
