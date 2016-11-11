namespace Markdown.MarkdownEnumerable
{
    public interface IMarkdownEnumerable
    {
        /// <summary>
        /// Returns tag starting from next position, or Tag.None if there's no tag starting there.
        /// </summary>
        Tag GetNextTag();
        
        /// <summary>
        /// Returns next char in enumerable and enumerates to next position.
        /// </summary>
        char GetNextChar();

        bool IsFinished();
    }

    public enum Tag
    {
        None, Strong, Italic
    }
}
