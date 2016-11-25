using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Markdown.MarkdownEnumerable.Tags
{
    internal class HyperlinkTagInfo : TagInfo
    {
        public const int VALUE_PART = 0;
        public const int LINK_PART = 1;

        private static readonly Dictionary<TagType, string> Representation = new Dictionary<TagType, string>
        {
            {new TagType(TagPosition.Opening, VALUE_PART), "["},
            {new TagType(TagPosition.Closing, VALUE_PART), "]"},
            {new TagType(TagPosition.Opening, LINK_PART), "("},
            {new TagType(TagPosition.Closing, LINK_PART), ")"}
        };
        
        public override int MaximalPossiblePartsCount => 2;

        [NotNull]
        public override string GetRepresentation() => Representation[TagType];

        public HyperlinkTagInfo(TagPosition tagPosition, int tagPart) : base(Tag.Hyperlink, tagPosition, tagPart)
        {
        }

        public HyperlinkTagInfo(TagType tagType) : base(Tag.Hyperlink, tagType)
        {
        }

        public override bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag = null)
        {
            if (!base.Fits(markdown, position, out positionAfterEnd))
                return false;
            if (TagType == new TagType(TagPosition.Opening, VALUE_PART))
                return IsHyperlinkStart(markdown, position + GetRepresentation().Length);
            return true;
        }

        /// <summary>
        /// Checks that hyperlink will be finished at some point.
        /// </summary>
        /// <param name="position">Position after '[' symbol</param>
        /// <returns></returns>
        private static bool IsHyperlinkStart(string markdown, int position)
        {
            var closingValuePart = new HyperlinkTagInfo(TagPosition.Closing, VALUE_PART);
            var closingRepr = closingValuePart.GetRepresentation();
            for (var i = position; i <= markdown.Length - closingRepr.Length; ++i)
                if (markdown.Substring(i, closingRepr.Length) == closingRepr)
                {
                    var openingSecondPartRepr = Representation[closingValuePart.GetOfNextType().TagType];
                    if (i + openingSecondPartRepr.Length >= markdown.Length ||
                        markdown.Substring(i + 1, openingSecondPartRepr.Length) != openingSecondPartRepr)
                        return false;
                    return IsHyperlinkSecondPart(markdown, i + closingRepr.Length + openingSecondPartRepr.Length);
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
            var endRepr = new HyperlinkTagInfo(TagPosition.Closing, LINK_PART).GetRepresentation();
            for (var i = position; i <= markdown.Length - endRepr.Length; ++i)
            {
                if (markdown.Substring(i, endRepr.Length) == endRepr)
                    return MarkdownParsingUtils.IsCorrectLink(builder.ToString());
                else
                    builder.Append(markdown[i]);
            }
            return false;
        }
    }
}
