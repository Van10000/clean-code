using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.MarkdownEnumerable
{
    public class StringMarkdownEnumerable : IMarkdownEnumerable
    {
        private readonly string markdown;

        private int currentPosition;

        public StringMarkdownEnumerable(string markdown)
        {
            this.markdown = markdown;
            currentPosition = 0;
        }

        public TagInfo GetNextTag(IEnumerable<TagInfo> possibleTags)
        {
            var possibleTagsList = possibleTags as IList<TagInfo> ?? possibleTags.ToList();
            return possibleTagsList
                .Where(tag => MarkdownParsingUtils.IsCorrectTag(tag, markdown, currentPosition))
                .FirstOrDefault(possibleTagsList.Contains) // possble to do it more efficiently, but doesn't matter here.
                ?? TagInfo.None;
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
    }
}
