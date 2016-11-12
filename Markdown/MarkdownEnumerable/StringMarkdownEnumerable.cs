using System;
using System.Collections.Generic;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        private readonly string markdown;
        private int currentPosition;
        private readonly Tag[] allPossibleTags = (Tag[])Enum.GetValues(typeof (Tag));

        public StringMarkdownEnumerable(string markdown)
        {
            this.markdown = markdown;
            currentPosition = 0;
        }

        public Tag GetNextOpeningTag()
        {
            return FirstTagMatchingOrNone(allPossibleTags, tag => MarkdownParsingPrimitives.IsOpeningTag(tag, markdown, currentPosition));
        }

        public Tag GetNextClosingTag()
        {
            return FirstTagMatchingOrNone(allPossibleTags, tag => MarkdownParsingPrimitives.IsClosingTag(tag, markdown, currentPosition));
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
            // ReSharper disable once LoopCanBeConvertedToQuery
            // because if Tag.None int value will change - linq would become wrong(linq is 'firstOrDefault')
            foreach (var tag in tags) 
                if (tag != Tag.None && prediate(tag))
                    return tag;
            return Tag.None;
        }
    }
}
