using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Markdown.MarkdownEnumerable.Tags
{
    internal class SimpleTagInfo : TagInfo
    {
        public override int MaximalPossiblePartsCount => 1;

        [NotNull]
        public override string GetRepresentation()
        {
            switch (Tag)
            {
                case Tag.Strong:
                    return "__";
                case Tag.Italic:
                    return "_";
                case Tag.None:
                    return "";
                default:
                    throw new ArgumentException($"Unknown tag:{Tag}");
            }
        }

        public SimpleTagInfo(Tag tag, TagPosition tagPosition) : base(tag, tagPosition, 0)
        {
            if (tag == Tag.Hyperlink)
                throw new ArgumentException($"Tag should be simple. {tag} is not a simple tag.");
        }

        public override bool Fits(string markdown, int position, out int positionAfterEnd, TagInfo previousTag = null)
        {
            if (!base.Fits(markdown, position, out positionAfterEnd))
                return false;
            var positionBefore = position - 1;
            var positionAfter = position + GetRepresentation().Length;

            var positionsBeforeAndAfter = new[] { positionBefore, positionAfter };
            if (IsAnySymbolAtAnyPosition(markdown, positionsBeforeAndAfter, MarkdownParsingUtils.Underscore + MarkdownParsingUtils.Digits))
                return false;

            return AreGoodPositionsForTag(TagPosition, markdown, positionBefore, positionAfter);
        }

        private static bool AreGoodPositionsForTag(TagPosition tagPosition, string markdown, int positionBefore, int positionAfter)
        {
            if (tagPosition == TagPosition.None)
                return false;

            if (tagPosition == TagPosition.Opening)
            {
                var correctAtPositionBefore = IsPositionOutOfRange(markdown, positionBefore) || char.IsWhiteSpace(markdown, positionBefore);
                var correctAtPositionAfter = IsPositionOutOfRange(markdown, positionAfter) || !char.IsWhiteSpace(markdown, positionAfter);
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

        private static bool IsPositionOutOfRange(string markdown, int position)
        {
            return position >= markdown.Length || position < 0;
        }
    }
}
