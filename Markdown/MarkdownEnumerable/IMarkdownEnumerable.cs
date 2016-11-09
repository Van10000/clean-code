namespace Markdown.MarkdownEnumerable
{
    public interface IMarkdownEnumerable
    {
        Tag GetNextTag();

        char GetNextChar();

        /// <summary>
        /// Because there are cases, when we need to parse the same place twice
        /// </summary>
        IMarkdownEnumerable GetCopy();

        bool IsFinished();
    }

    public enum Tag
    {
        None, Strong, Italic
    }
}
