using System;
using System.Linq;

namespace Markdown.MarkdownEnumerable.Tags
{
    internal class ParagraphTagInfo : TagInfo
    {
        public ParagraphTagInfo(TagPosition tagPosition) : base(Tag.Paragraph, tagPosition, 0)
        {
        }

        public override int MaximalPossiblePartsCount => 1;

        public override string GetRepresentation() => null;

        public override bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag = null)
        {
            switch (TagPosition)
            {
                case TagPosition.Closing:
                    positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, position, char.IsWhiteSpace);
                    var symbolsBetween = markdown.Substring(position, positionAfterEnd - position);
                    var newLinesCount = symbolsBetween.Count(MarkdownParsingUtils.IsNextLineSymbol);
                    int ignore;
                    var nextTagIsHeader = new HeaderTagInfo(TagPosition.Opening).Fits(markdown, positionAfterEnd, out ignore);
                    var enoughNewLines = newLinesCount > 1;
                    var enoughForNextHeader = newLinesCount == 1 || previousTag?.Tag == Tag.Paragraph;
                    return enoughNewLines || (enoughForNextHeader && nextTagIsHeader);
                case TagPosition.Opening:
                    positionAfterEnd = MarkdownParsingUtils.FindNextNotFitting(markdown, position, char.IsWhiteSpace);
                    return true;
                default:
                    throw new InvalidOperationException("Tag position should eather be opening or closing.");
            }
        }
    }
}
