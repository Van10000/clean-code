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
            if (IsFinished())
                return TagInfo.None;
            var possibleTagsList = possibleTags as IList<TagInfo> ?? possibleTags.ToList();
            var positionAfterEnd = -1;
            var result = possibleTagsList
                .FirstOrDefault(tag => tag.Fits(markdown, currentPosition, out positionAfterEnd))
                ?? TagInfo.None;
            if (result != TagInfo.None)
                currentPosition = positionAfterEnd;
            return result;
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
