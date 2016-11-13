namespace Markdown.MarkdownEnumerable
{
    public interface IMarkdownEnumerable
    {
        /// <summary>
        /// Returns opening tag starting from next position, or Tag.None if there's no tag starting there.
        /// </summary>
        Tag GetNextOpeningTag();

        /// <summary>
        /// Returns closing tag starting from next position, or Tag.None if there's no tag starting there.
        /// </summary>
        Tag GetNextClosingTag();
        
        /// <summary>
        /// Returns next char in enumerable and enumerates to the next position.
        /// </summary>
        char GetNextChar();

        bool IsFinished();
    }

    // CR: One class = one file
    public enum Tag
    {
        None, Strong, Italic
    }
}
