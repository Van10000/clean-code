using System.Collections.Generic;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.MarkdownEnumerable
{
    public interface IMarkdownEnumerable
    {
        /// <summary>
        /// Returns TagInfo for tag starting from next position, or TagInfo.None if there's no tag starting there.
        /// </summary>
        TagInfo GetNextTag(IEnumerable<TagInfo> possibleTags);

        /// <summary>
        /// Returns next char in enumerable and enumerates to the next position.
        /// </summary>
        char GetNextChar();

        bool IsFinished();
    }
}
