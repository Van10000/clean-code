using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        private static readonly Tag[] AllPossibleTags = (Tag[])Enum.GetValues(typeof(Tag));

        private readonly string markdown;

        private int currentPosition;

        public StringMarkdownEnumerable(string markdown)
        {
            this.markdown = markdown;
            currentPosition = 0;
        }

        public Tag GetNextOpeningTag()
        {
            return FirstTagMatchingOrNone(AllPossibleTags, tag => MarkdownParsingUtils.IsOpeningTag(tag, markdown, currentPosition));
        }

        public Tag GetNextClosingTag()
        {
            return FirstTagMatchingOrNone(AllPossibleTags, tag => MarkdownParsingUtils.IsClosingTag(tag, markdown, currentPosition));
        }

        public char GetNextChar()
        {
            if (IsFinished())
                throw new InvalidOperationException("Enumerable is finished");
            if (markdown[currentPosition] == '\\')
                currentPosition++;
            currentPosition++;
            return markdown[currentPosition - 1];
        }

        public bool IsFinished()
        {
            return currentPosition == markdown.Length;
        }
        
        private static Tag FirstTagMatchingOrNone(IEnumerable<Tag> tags, Predicate<Tag> prediate)
        {
            return tags
                .Where(tag => tag != Tag.None)
                .FirstOrDefault(prediate, Tag.None);
        }
    }
}
