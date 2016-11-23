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
            throw new NotImplementedException();
        }
    }
}
