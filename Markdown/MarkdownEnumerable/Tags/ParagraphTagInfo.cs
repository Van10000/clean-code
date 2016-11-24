using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MarkdownEnumerable.Tags
{
    class ParagraphTagInfo : TagInfo
    {
        public ParagraphTagInfo(TagPosition tagPosition) : base(Tag.Paragraph, tagPosition, 0)
        {
        }

        public override int MaximalPossiblePartsCount => 1;

        public override string GetRepresentation() => null;

        public override bool Fits(string markdown, int position, out int positionAfterEnd)
        {
            switch (TagPosition)
            {
                case TagPosition.Closing:
                    positionAfterEnd = MarkdownParsingUtils.FindNextNotWhiteSpace(markdown, position);
                    var symbolsBetween = markdown.Substring(position, positionAfterEnd - position);
                    return symbolsBetween.Count(MarkdownParsingUtils.NextLineSymbols.Contains) > 1;
                case TagPosition.Opening:
                    positionAfterEnd = MarkdownParsingUtils.FindNextNotWhiteSpace(markdown, position);
                    return true;
                default:
                    throw new InvalidOperationException("Tag position should eather be opening or closing.");
            }
        }
    }
}
